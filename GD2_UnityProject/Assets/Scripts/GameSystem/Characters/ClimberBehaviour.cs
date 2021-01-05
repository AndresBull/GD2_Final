using BoardSystem;
using GameSystem.Management;
using GameSystem.Props;
using GameSystem.Views;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GameSystem.Characters
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public class ClimberBehaviour : MonoBehaviour
    {
        #region Fields

        [SerializeField] private MeshRenderer _meshRenderer = null;
        [SerializeField] private MeshFilter _meshFilter = null;

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
        [SerializeField] private float _jumpHeight = 0.5f;

        [Header("Physics")]
        [Tooltip("The layers to test collision against. Default tests against everything.")]
        [SerializeField] private LayerMask _collisionMask = ~0;

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
        private BlockField _blockField;                         // reference to the array that represents the layout of the blocks
        private BlockFieldView _blockFieldView;                 // reference to the visual representation of the layout of the blocks
        private Collider _col;                                  // reference to the (base) collider component attached to this gameobject;
        private PlayerConfiguration _playerConfig;
        private Rigidbody _rb;                                  // reference to the rigidbody component attached to this gameobject
        private Transform _cameraTransform;                     // holds the camera transform locally to preserve the original transform from changes
        private Transform _ladderClimbed;                       // the current ladder being climbed

        // Structs
        private Vector3 _colExtents;                            // the extents of the collider
        private float _horizontalMovement;                      // float that stores the value of the movement on the x-axis
        private float _jumpTimer = 0.0f;                        // float to compare the current runtime to the jump call time to determine if the call happened inside the jump delay interval
        private float _placementDistance = 0.4f;                // how far from the climber the ladder will be placed
        private float _rayLength = 0.45f;                        // the length of the ray that tests against collision in order to prevent 'sticky' colliders
        private readonly float _jumpForce = 6.0f;               // the force applied to the rigidbody at the start of a jump
        private int _highestYPositionReached = 0;

        private bool _isJumping = false;                        // bool to determine if the character is jumping or not
        private bool _hasLadder = true;                         // backup field that determines if the climber has his ladder or not
        private Vector2 _movementConstraints;
        #endregion

        public bool IsCarryingLadder
        {
            get => _hasLadder;
            private set => _hasLadder = value;
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
            _colExtents = _col.bounds.extents;
            SetMovementConstraints();
        }

        private void FixedUpdate()
        {
            CheckIfNewHeightIsReached();
            
            if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y + _colExtents.y/2, transform.position.z), transform.forward, _rayLength, _collisionMask)
                || Physics.Raycast(new Vector3(transform.position.x, transform.position.y - _colExtents.y / 2, transform.position.z), transform.forward, _rayLength, _collisionMask))
                _horizontalMovement = 0.0f;

            if (_rb.velocity.y <= 0)
            {
                    _rb.AddForce(Physics.gravity.normalized * _jumpHeight, ForceMode.Impulse);
            }
            
            if (_isJumping)
            {
                // still working on this
                //if (_rb.velocity.y != 0)
                //{
                //    _horizontalMovement -= Mathf.Sign(_horizontalMovement) * Mathf.Abs(_rb.velocity.y);
                //}
            }

            float movement = _horizontalMovement * _maxSpeed * Time.fixedDeltaTime;
            transform.position += new Vector3(movement, 0.0f, 0.0f);

            float clampedX = Mathf.Clamp(transform.position.x, _movementConstraints.x, _movementConstraints.y);
            transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
            
            UseGravity();
        }

        #endregion

        public void InitializePlayer(PlayerConfiguration config)
        {
            _playerConfig = config;
            _meshFilter.mesh = config.Character;
            _meshRenderer.material = config.PlayerMaterial;
        }

        public void CheckIfTrapped()
        {
            List<BlockPosition> filledPositions = _blockField.GetAllFieldPositions();
            foreach (BlockPosition floodedPosition in FloodFiller.FloodedPositions)
            {
                filledPositions.Remove(floodedPosition);
            }
            if (filledPositions.Contains(_blockFieldView.PositionConverter.ToBlockPosition(_blockField, transform.position)))
            {
                GetKilled();
            }
        }


        private BlockPosition GetClimberBlockPosition()
        {
            return _blockFieldView.PositionConverter.ToBlockPosition(_blockField, transform.position);
        }

        
        private bool FindSuitablePlacementOnSide(int direction, BlockPosition climberBlock, ref int ladderLength, ref bool isAtPreferredSide)
        {
            for (int i = 1; i <= _maxLadderHeight; i++)
            {
                Block blockNorth2 = _blockField.BlockAt(new BlockPosition(climberBlock.X, climberBlock.Y + i));
                if (blockNorth2 != null)
                {
                    ladderLength = i;
                    print(ladderLength);
                    break;
                }
            }
            if (ladderLength < 2)
            {
                print("Can't place ladder here. Placement blocked.");
                return false;
            }

            int dirNormal = (int)(direction / Mathf.Abs(direction));
            // check west side
            for (int i = 2; i <= ladderLength; i++)
            {
                Block blockNorthWest = _blockField.BlockAt(new BlockPosition(climberBlock.X - (1 * dirNormal), climberBlock.Y + i));
                Block blockUnderNorthWest = _blockField.BlockAt(new BlockPosition(climberBlock.X - (1 * dirNormal), climberBlock.Y + i - 1));

                if (blockNorthWest == null && blockUnderNorthWest != null)
                {
                    ladderLength = i;
                    isAtPreferredSide = true;
                    print("Placed at preferred side.");
                    return true;
                }
            }

            // check east side
            for (int i = 2; i <= ladderLength; i++)
            {
                Block blockNorthEast = _blockField.BlockAt(new BlockPosition(climberBlock.X + (1 * dirNormal), climberBlock.Y + i));
                Block blockUnderNorthEast = _blockField.BlockAt(new BlockPosition(climberBlock.X + (1 * dirNormal), climberBlock.Y + i - 1));

                if (blockNorthEast == null && blockUnderNorthEast != null)
                {
                    ladderLength = i;
                    isAtPreferredSide = false;
                    print("Placed at opposite side.");
                    return true;
                }
            }

            print("No suitable placement found for ladder.");
            return false;
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
            int currentYPosition = GetClimberBlockPosition().Y;

            if (_highestYPositionReached < currentYPosition)
            {
                int scoreMultiplier = currentYPosition - _highestYPositionReached;
                PointSystemScript.PlayerReachedNewHeight(_playerConfig.PlayerIndex, scoreMultiplier);
                _highestYPositionReached = currentYPosition;
            }
        }

        private void GetKilled()
        {
            PointSystemScript.PlayerGotKilled();
            // TODO: IDEA: every time a player dies, another random message is shown
            print($"Player {_playerConfig.PlayerIndex + 1} is out of the run.");
            gameObject.SetActive(false);
        }

        private void Jump()
        {
            _rb.AddForce(-Physics.gravity.normalized * _jumpHeight * _jumpForce, ForceMode.VelocityChange);
            _jumpTimer = 0.0f;
            SoundManager.Instance.PlayJump();
        }

        private void RotateToMoveDirection()
        {
            float direction = Mathf.Clamp(_horizontalMovement, -1, 1);

            if (direction <= 0.01f && direction >= -0.01f)
            {
                transform.rotation = Quaternion.LookRotation(-_cameraTransform.forward, _cameraTransform.up);
                return;
            }

            if (transform.rotation != Quaternion.LookRotation(direction * _cameraTransform.right, _cameraTransform.up))
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction * _cameraTransform.right, _cameraTransform.up), 0.5f);
            }
        }

        private void PlaceLadderAt(BlockPosition climberBlock)
        {
            bool placeLadderWest;
            bool isPlacedOnPreferredSide = true;
            bool hasFoundPlacement;
            int ladderLength = _maxLadderHeight;

            if (GetXLocationOnBlock() >= 0.5f)
            {
                print("Looking for placement West/Left...");
                placeLadderWest = true;
                hasFoundPlacement = FindSuitablePlacementOnSide(1, climberBlock, ref ladderLength, ref isPlacedOnPreferredSide);
            }
            else
            {
                print("Looking for placement East/Right...");
                placeLadderWest = false;
                hasFoundPlacement = FindSuitablePlacementOnSide(-1, climberBlock, ref ladderLength, ref isPlacedOnPreferredSide);
            }

            if (hasFoundPlacement)
            {
                PlaceLadder(placeLadderWest, isPlacedOnPreferredSide, ladderLength);
            }
        }

        private void PlaceLadder(bool placeLadderLeft, bool isAtPreferredSide, int ladderLength)
        {
            Vector3 position = transform.position;
            position.y -= _colExtents.y;
            Quaternion rotation;

            if (placeLadderLeft)
            {
                print("Place Ladder West/Left");
                position.x -= _placementDistance;
                if (!isAtPreferredSide)
                {
                    rotation = Quaternion.LookRotation(-_cameraTransform.right, _cameraTransform.up);
                }
                else
                {
                    rotation = Quaternion.LookRotation(_cameraTransform.right, _cameraTransform.up);
                }
            }
            else
            {
                print("Place Ladder East/Right");
                position.x += _placementDistance;
                if (!isAtPreferredSide)
                {
                    rotation = Quaternion.LookRotation(_cameraTransform.right, _cameraTransform.up);
                }
                else
                {
                    rotation = Quaternion.LookRotation(-_cameraTransform.right, _cameraTransform.up);
                }
            }

            GameObject ladder = Instantiate(_ladderPrefab, position, rotation);
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
            if (IsGrounded() || IsClimbing)
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

        #region Input
        // TODO: REMOVE the following methods if PlayerInput uses BroadcastMessages()
        //       UNCOMMENT the follwing methods if PlayerInput uses Invoke Unity Events

        //public void OnMove(InputAction.CallbackContext context)
        //{
        //    if (_isHanging)
        //    {
        //        return;
        //    }

        //    _horizontalMovement = context.ReadValue<float>();
        //    Move();
        //}

        //public void OnJump(InputAction.CallbackContext context)
        //{
        //    if (context.ReadValueAsButton())
        //    {
        //        if (!_isJumping && IsGrounded())
        //        {
        //            _isJumping = true;
        //            Jump();
        //            return;
        //        }
        //        _jumpTimer = Time.time + _jumpDelay;
        //    }
        //}

        //public void OnPlaceLadder(InputAction.CallbackContext context)
        //{
        //    if (!context.ReadValueAsButton()) // ! is used to make sure the method gets called only once instead of twice (pressed: 1 -> 0; released: 0 -> 1)
        //    {
        //        if (IsCarryingLadder)
        //        {
        //            BlockPosition climberBlock = GetClimberBlockPosition();
        //            BlockView blockNorth = _blockField.BlockAt(new BlockPosition(climberBlock.X, climberBlock.Y + 1));

        //            if (blockNorth != null)
        //            {
        //                print("Can't place ladder here. Placement blocked.");
        //                return;
        //            }

        //            PlaceLadderAt(climberBlock);
        //        }
        //        else
        //        {
        //            print("No ladder available");
        //        }
        //    }
        //}

        //public void OnPickupLadder(InputAction.CallbackContext context)
        //{
        //    if (!context.ReadValueAsButton()) // ! is used to make sure the method gets called only once instead of twice (pressed: 1 -> 0; released: 0 -> 1)
        //    {
        //        if (!IsCarryingLadder)
        //        {
        //            BlockPosition climberBlock = GetClimberBlockPosition();

        //            // TODO: change this method to use the blocks assigend in teh PlaceLadder method instead of recalculating everything

        //            // get all objects inside the range and filter for ladders
        //            var colliders = Physics.OverlapSphere(transform.position, _range);

        //            foreach (var collider in colliders)
        //            {
        //                if (collider.transform.root.TryGetComponent(out Ladder ladderScript))
        //                {
        //                    if (ladderScript.Owner == this.gameObject)
        //                    {
        //                        Destroy(ladderScript.gameObject);
        //                        print("Picked up Ladder");
        //                        IsCarryingLadder = true;
        //                        break;
        //                    }
        //                    continue;
        //                }
        //            }
        //        }
        //    }
        //}

        //public void OnTryKill(InputAction.CallbackContext context)
        //{
        //    if (_blockField.PositionConverter.ToBlockPosition(_blockField, this.gameObject.transform.position).Y > _blockField.Rows)
        //    {
        //        PlayerConfigManager.Instance.SetPlayerAsOverlord(_playerConfig.PlayerIndex);
        //    }
        //}


        // TODO: USE the following methods if PlayerInput uses BroadcastMessages()
        //       REMOVE the follwing methods if PlayerInput uses Invoke Unity Events
        public void OnMove(InputValue value)
        {
            _horizontalMovement = value.Get<float>();

            RotateToMoveDirection();
        }

        public void OnJump(InputValue value)
        {
            if (value.isPressed)
            {
                if (!_isJumping && IsGrounded())
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
            if (value.isPressed)
            {
                if (IsCarryingLadder)
                {
                    BlockPosition climberBlock = GetClimberBlockPosition();
                    Block blockNorth = _blockField.BlockAt(new BlockPosition(climberBlock.X, climberBlock.Y + 1));

                    if (blockNorth != null)
                    {
                        print("Can't place ladder here. Placement blocked.");
                        return;
                    }

                    PlaceLadderAt(climberBlock);
                }
                else
                {
                    print("No ladder available");
                }
            }
        }

        public void OnPickupLadder(InputValue value)
        {
            if (value.isPressed)
            {
                if (!IsCarryingLadder)
                {
                    BlockPosition climberBlock = GetClimberBlockPosition();

                    // TODO: change this method to use the blocks assigend in teh PlaceLadder method instead of recalculating everything

                    // get all objects inside the range and filter for ladders
                    var colliders = Physics.OverlapSphere(transform.position, _range);

                    foreach (var collider in colliders)
                    {
                        if (collider.transform.root.TryGetComponent(out Ladder ladderScript))
                        {
                            if (ladderScript.Owner == gameObject)
                            {
                                Destroy(ladderScript.gameObject);
                                print("Picked up Ladder");
                                IsCarryingLadder = true;
                                break;
                            }
                            continue;
                        }
                    }
                }
            }
        }

        public void OnTryKill(InputValue value)
        {
            if (value.isPressed)
            {
                if (_blockFieldView.PositionConverter.ToBlockPosition(_blockField, transform.position).Y > _blockField.Rows)
                {
                    PlayerConfigManager.Instance.SetPlayerAsOverlord(_playerConfig.PlayerIndex);
                }
            }
        }
        #endregion
    }
}