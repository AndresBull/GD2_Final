﻿using BoardSystem;
using GameSystem.Management;
using GameSystem.Props;
using GameSystem.Views;
using SoundSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

namespace GameSystem.Characters
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public class ClimberBehaviour : MonoBehaviour
    {
        #region Fields
        [Header("Camera")]
        [Tooltip("The camera that renders the scene to the viewport. If no camera is assigned the object tagged \"MainCamera\" will be used.")]
        [SerializeField] private GameObject _camera = null;

        [Header("Horizontal Movement")]
        [Tooltip("The maximum speed at which the climber moves around.")]
        [SerializeField] private float _maxSpeed = 10f;

        [Header("Jumping")]
        [Tooltip("The time interval in which a jump can be called before the player touches the ground (in seconds).")]
        [SerializeField] private float _jumpDelay = 0.25f;
        [Tooltip("The height of the jump (in units).")]
        [SerializeField] private float _jumpHeight = 1f;

        [Header("Physics")]
        [Tooltip("The layers to test collision against. Default tests against everything.")]
        [SerializeField] private LayerMask _collisionMask = ~0;
        
        [Header("Animator")]
        [Tooltip("The sprite animator used for the climbers animations")]
        [SerializeField] private Animator _animator = null;

        [Header("Ladders")]
        [Tooltip("The ladder prefab for this character.")]
        [SerializeField] private GameObject _ladderPrefab = null;
        [Tooltip("Interaction range between climber and ladder.")]
        [SerializeField] private float _range = 1f;
        [Tooltip("The max number of blocks a ladder can bridge vertically. I.e. the maximum length of the ladder.")]
        [SerializeField] private int _maxLadderHeight = 3;

        public static FloodFill FloodFiller;

        [HideInInspector]
        public bool IsClimbing = false;

        // Components
        private BlockField _blockField = null;                         // reference to the array that represents the layout of the blocks
        private BlockFieldView _blockFieldView = null;                 // reference to the visual representation of the layout of the blocks
        private Animation _animation = null;
        private Collider _col = null;                                  // reference to the (base) collider component attached to this gameobject;
        private PlayerConfiguration _playerConfig = null;
        private Rigidbody _rb = null;                                  // reference to the rigidbody component attached to this gameobject
        private Transform _cameraTransform = null;                     // holds the camera transform locally to preserve the original transform from changes
        private Transform _model = null;

        // Structs
        private Vector3 _colExtents = Vector3.zero;                    // the extents of the collider
        private Vector2 _movementConstraints = Vector2.zero;
        private float _horizontalMovement = 0.0f;                      // float that stores the value of the movement on the x-axis
        private float _jumpTimer = 0.0f;                               // float to compare the current runtime to the jump call time to determine if the call happened inside the jump delay interval
        private float _rayLength = 0.4f;                              // the length of the ray that tests against collision in order to prevent 'sticky' colliders
        private readonly float _jumpForce = 6.0f;                      // the force applied to the rigidbody at the start of a jump
        private readonly float _ladderUnitDistance = 1.0f / Mathf.Tan(75.0f * Mathf.Deg2Rad);   // the 'constant' unit distance for a ladder placed under an angle of 75 degrees
        private int _highestYPositionReached = 0;

        private bool _canPush = true;
        private bool _isCarryingLadder = true;
        private bool _isGrounded = true;
        private bool _isJumping = false;                               // bool to determine if the character is jumping or not
        private bool _isDead = false;

        #endregion

        private bool IsCarryingLadder
        {
            get => _isCarryingLadder;
            set
            {
                _isCarryingLadder = value;
                PlayerConfigManager.Instance.ToggleLadderEquip(_playerConfig.PlayerIndex, _isCarryingLadder);
            }
        }

        #region Unity Lifecycle
        private void Awake()
        {
            // set the camera object if it's not assigned in the inspector
            if (_camera == null)
            {
                _camera = GameObject.FindGameObjectWithTag("MainCamera");
            }
            _cameraTransform = _camera.transform.GetChild(0);
            _cameraTransform.forward = Vector3.ProjectOnPlane(_camera.transform.forward, Vector3.up);

            // store the BlockField in a field for easy access
            _blockField = GameLoop.Instance.Field;
            _blockFieldView = GameLoop.Instance.FieldView;
        }

        private void Start()
        {
            // set up the components
            _rb = GetComponent<Rigidbody>();
            _col = GetComponent<Collider>();
            _animation = GetComponent<Animation>();
            _colExtents = _col.bounds.extents;
            _model = transform.GetChild(0);
            SetMovementConstraints();
        }

        private void FixedUpdate()
        {
            _isGrounded = IsGrounded();
            CheckIfNewHeightIsReached();

            var horizontalMovement = _horizontalMovement;
            if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y + _colExtents.y / 2, transform.position.z), transform.forward, _rayLength, _collisionMask)
                || Physics.Raycast(new Vector3(transform.position.x, transform.position.y - _colExtents.y / 2, transform.position.z), transform.forward, _rayLength, _collisionMask))
                horizontalMovement = 0f;

            if (_rb.velocity.y <= 0)
            {
                _rb.AddForce(Physics.gravity.normalized * _jumpHeight, ForceMode.Impulse);
            }

            if (horizontalMovement == 0f)
            {
                _animator.SetBool("IsWalking", false);
            }
            else
            {
                _animator.SetBool("IsWalking", true);
            }

            float movement = horizontalMovement * _maxSpeed * Time.fixedDeltaTime;
            transform.position += new Vector3(movement, 0.0f, 0.0f);

            float clampedX = Mathf.Clamp(transform.position.x, _movementConstraints.x, _movementConstraints.y);
            transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);

            UseGravity();
        }

        #endregion

        public void InitializePlayer(PlayerConfiguration config)
        {
            _playerConfig = config;
        }

        public void CheckIfTrapped()
        {
            List<BlockPosition> filledPositions = _blockField.GetAllFieldPositions();

            foreach (BlockPosition rimPosition in _blockField.GetAllRimPositions())
            {
                filledPositions.Add(rimPosition);
            }

            foreach (BlockPosition floodedPosition in FloodFiller.FloodedPositions)
            {
                filledPositions.Remove(floodedPosition);
            }

            if (filledPositions.Contains(_blockFieldView.PositionConverter.ToBlockPosition(_blockField, transform.position)))
            {
                if(this.gameObject)
                GetKilled();
                
            }
        }


        private BlockPosition GetClimberBlockPosition()
        {
            return _blockFieldView.PositionConverter.ToBlockPosition(_blockField, transform.position);
        }


        private bool HasFoundSuitablePlacementOnSide(int direction, BlockPosition climberBlock, ref int ladderLength)
        {
            int dirNormal = direction / Mathf.Abs(direction);
            ladderLength = 0;
            // determine the height of the ladder, based on the blocks on the side
            for (int i = _maxLadderHeight; i >= 1; i--)
            {
                Block blockNorthBeside = _blockField.BlockAt(new BlockPosition(climberBlock.X + dirNormal, climberBlock.Y + i));
                if (blockNorthBeside != null)
                {
                    continue;
                }

                Block blockUnderNorthBeside = _blockField.BlockAt(new BlockPosition(climberBlock.X + dirNormal, climberBlock.Y + i - 1));
                if (blockUnderNorthBeside == null)
                {
                    continue;
                }
                ladderLength = i;
                break;
            }

            // determine if all positions above the current cell are free to put a ladder in
            for (int i = 1; i <= ladderLength; i++)
            {
                Block blockNorth = _blockField.BlockAt(new BlockPosition(climberBlock.X, climberBlock.Y + i));
                if (blockNorth != null)
                {
                    ladderLength = 0;
                    break;
                }
            }

            if (ladderLength < 2)
            {
                print("Can't place ladder here. Placement blocked.");
                return false;
            }

            return true;
        }

        private bool IsGrounded()
        {
            Physics.BoxCast(transform.position + (0.1f * Vector3.up), _colExtents, Vector3.down, out RaycastHit hit, transform.rotation, 0.1f, _collisionMask);
            return hit.collider != null && _rb.velocity.y <= 0;
        }


        private float GetXLocationOnBlock()
        {
            return transform.position.x - Mathf.Floor(transform.position.x);
        }


        private void CheckIfNewHeightIsReached()
        {
            if (_isGrounded)
            {
                int currentYPosition = GetClimberBlockPosition().Y;

                if (_highestYPositionReached < currentYPosition)
                {
                    int scoreMultiplier = currentYPosition - _highestYPositionReached;
                    PointSystemScript.PlayerReachedNewHeight(_playerConfig.PlayerIndex, scoreMultiplier);
                    _highestYPositionReached = currentYPosition;
                }

                if (currentYPosition >= _blockField.Rows)
                    PlayerConfigManager.Instance.TriggerRoundOver(_playerConfig.PlayerIndex);
            }
        }

        private void GetKilled()
        {
            if (!_isDead)
            {
                PointSystemScript.PlayerGotKilled();
                // TODO: IDEA: every time a player dies, another random message is shown
                print($"Player {_playerConfig.PlayerIndex + 1} is out of the run.");
                gameObject.SetActive(false);
                _isDead = true;
            }
        }

        private void Jump()
        {
            _rb.AddForce(-Physics.gravity.normalized * _jumpHeight * _jumpForce, ForceMode.VelocityChange);
            _jumpTimer = 0.0f;
            SoundManager.Instance.PlayJump();
        }

        private void RotateToMoveDirection()
        {
            float direction = _horizontalMovement / Mathf.Abs(_horizontalMovement);
            if (direction <= 0.01f && direction >= -0.01f)
            {
                transform.rotation = Quaternion.LookRotation(-_cameraTransform.forward, _cameraTransform.up);
                _model.rotation = Quaternion.LookRotation(_cameraTransform.forward, _cameraTransform.up);
                return;
            }

            if (transform.rotation != Quaternion.LookRotation(direction * _cameraTransform.right, _cameraTransform.up))
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction * _cameraTransform.right, _cameraTransform.up), 0.5f);
            }
            _model.rotation = Quaternion.LookRotation(direction * -_cameraTransform.forward, _cameraTransform.up);
        }

        private void PickupLadder()
        {
            if (IsCarryingLadder)
            {
                return;
            }
            


            // get all objects inside the range and filter for ladders
            Collider[] colliders = Physics.OverlapSphere(transform.position, _range);

            foreach (Collider collider in colliders)
            {
                if (collider.transform.root.TryGetComponent(out Ladder ladderScript))
                {
                    if (ladderScript.Owner == gameObject)
                    {
                        _animator.SetTrigger("PickupLadder");
                        Destroy(ladderScript.gameObject);
                        IsCarryingLadder = true;
                        break;
                    }
                    continue;
                }
            }
        }

        private void DetermineLadderPosition()
        {
            BlockPosition climberBlock = GetClimberBlockPosition();
            bool placeLadderWest = false;
            bool placeLadderEast = false;
            int ladderLength = _maxLadderHeight;

            if (GetXLocationOnBlock() <= 0.5f)
            {
                placeLadderWest = HasFoundSuitablePlacementOnSide(-1, climberBlock, ref ladderLength);
                if (!placeLadderWest)
                {
                    placeLadderEast = HasFoundSuitablePlacementOnSide(1, climberBlock, ref ladderLength);
                }
            }
            else
            {
                placeLadderEast = HasFoundSuitablePlacementOnSide(1, climberBlock, ref ladderLength);
                if (!placeLadderEast)
                {
                    placeLadderWest = HasFoundSuitablePlacementOnSide(-1, climberBlock, ref ladderLength);
                }
            }

            if (placeLadderEast)
            {
                PlaceLadder(climberBlock, false, ladderLength);
            }

            if (placeLadderWest)
            {
                PlaceLadder(climberBlock, true, ladderLength);
            }
        }

        private void PlaceLadder(BlockPosition climberBlock, bool placeLadderWest, int ladderLength)
        {
            Vector3 cellPosition = _blockFieldView.PositionConverter.ToWorldPosition(_blockField, climberBlock);
            cellPosition.x = Mathf.Floor(cellPosition.x);
            cellPosition.y = transform.position.y - _colExtents.y;
            Quaternion rotation;

            if (placeLadderWest)
            {
                rotation = Quaternion.LookRotation(_cameraTransform.right, _cameraTransform.up);
                cellPosition.x += ladderLength * _ladderUnitDistance;
            }
            else
            {
                rotation = Quaternion.LookRotation(-_cameraTransform.right, _cameraTransform.up);
                cellPosition.x += _blockFieldView.PositionConverter.BlockScale.x - (ladderLength * _ladderUnitDistance);
            }

            _animator.SetTrigger("PlaceLadder");
            GameObject ladder = Instantiate(_ladderPrefab, cellPosition, rotation);
            ladder.GetComponent<Ladder>().Owner = gameObject;
            ladder.GetComponent<Ladder>().Length = ladderLength;
            IsCarryingLadder = false;
        }

        private void SetMovementConstraints()
        {
            float fieldWidth = _blockField.Columns * _blockFieldView.PositionConverter.BlockScale.x;

            float playerWidth = 1f; // set to ~model width

            float range = (fieldWidth - playerWidth) / 2;
            _movementConstraints = new Vector2(-range, range);
        }

        private void UseGravity()
        {
            if (_isGrounded || IsClimbing)
            {
                _rb.useGravity = false;
                _rb.velocity = new Vector3(_rb.velocity.x, 0.0f, 0.0f);
                if (_jumpTimer > Time.time)
                {
                    Jump();
                }
                _isJumping = false;
                return;
            }
            _rb.useGravity = true;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(new Vector3(transform.position.x, transform.position.y + _colExtents.y / 2, transform.position.z), transform.forward * _rayLength);
            Gizmos.DrawRay(new Vector3(transform.position.x, transform.position.y - _colExtents.y / 2, transform.position.z), transform.forward * _rayLength);
        }

        private IEnumerator PushCooldown(float timeInSec)
        {
            _canPush = false;
            PlayerConfigManager.Instance.ToggleSpecialUsed(_playerConfig.PlayerIndex, _canPush);

            yield return new WaitForSeconds(timeInSec);

            _canPush = true;
            PlayerConfigManager.Instance.ToggleSpecialUsed(_playerConfig.PlayerIndex, _canPush);
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }

        #region Input
        public void OnMove(InputValue value)
        {
            _horizontalMovement = value.Get<float>();

            RotateToMoveDirection();
        }

        public void OnJump(InputValue value)
        {
            if (value.isPressed)
            {
                if (!_isJumping && _isGrounded)
                {
                    _isJumping = true;
                    Jump();
                    return;
                }
                _jumpTimer = Time.time + _jumpDelay;
            }
        }

        public void OnPlaceLadder(InputValue value)
        {
            if (value.isPressed && IsGrounded())
            {
                if (IsCarryingLadder)
                {
                    DetermineLadderPosition();
                    return;
                }

                PickupLadder();
            }
        }

        public void OnOpenOptions(InputValue value)
        {
            if (value.isPressed)
            {
                if (_playerConfig.PlayerIndex != 0)
                    return;

                GameObject.Find("Canvas").GetComponent<OptionsMenu>().OpenOptions();
            }
        }

        public void OnPushBlock(InputValue value)
        {
            if (value.isPressed)
            {
                if (_horizontalMovement != 0 && _canPush)
                {
                    var direction = (int)Mathf.Sign(_horizontalMovement);

                    var checkBlockPos = GetClimberBlockPosition();
                    checkBlockPos.X += direction;
                    var checkBlock = _blockField.BlockAt(checkBlockPos);

                    //Need to maybe find a better way to link block to blockview
                    var checkblockView = _blockFieldView.CheckIfBlockViewContainsBlock(checkBlock);
                    if (checkblockView != null)
                    {
                        if (checkblockView.PushBlock(transform.position, direction))
                        {
                            _animator.SetTrigger("PlaceLadder");
                            StartCoroutine(PushCooldown(5));
                        }
                    }
                }
            }
        }
        #endregion
    }
}