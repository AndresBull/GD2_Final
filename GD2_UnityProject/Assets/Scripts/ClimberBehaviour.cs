using Assets.Scripts;
using BoardBase;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Views;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class ClimberBehaviour : MonoBehaviour
{
    #region Fields

    [SerializeField]
    private MeshRenderer _meshRenderer;
    [SerializeField]
    private MeshFilter _meshFilter;

    [Header("Camera")] [SerializeField]
    [Tooltip("The camera that renders the scene to the viewport. If no camera is assigned the object tagged \"MainCamera\" will be used.")]
    private GameObject _camera = null;

    [Header("Horizontal Movement")]
    [Tooltip("The maximum speed at which the climber moves around.")] [SerializeField]
    private float _maxSpeed = 10f;

    [Header("Jumping")]
    [Tooltip("The time interval in which a jump can be called before the player touches the ground (in seconds).")] [SerializeField]
    private float _jumpDelay = 0.25f;
    [Tooltip("The height of the jump (in units).")] [SerializeField]
    private float _jumpHeight = 0.5f;
    [Tooltip("WARNING: PROTOTYPING ONLY\nThis multiplier can be used during prototyping to increase the jump to reach heights quickly. It will be ignored in the build. Set to 1 (one) to disable.")] [SerializeField]
    private float _jumpMultiplier = 1f;

    [Header("Physics")]
    [Tooltip("The layers to test collision against. Default tests against everything.")]
    [SerializeField]
    private LayerMask _collisionMask = ~0;
    [Tooltip("How far in front of the Climber must be tested for collision.")]
    [SerializeField]
    private float _rayLength = 0.5f;

    [Header("Ladders")]
    [Tooltip("The ladder prefab for this character.")] [SerializeField]
    private GameObject _ladderPrefab;
    [Tooltip("Interaction range between climber and ladder.")] [SerializeField]
    private float _range = 1f;
    [Tooltip("The max number of blocks a ladder can bridge vertically. I.e. the maximum length of the ladder.")] [SerializeField]
    private int _maxLadderHeight = 3;
    [HideInInspector]
    public bool IsClimbing = false;

    // Components
    private BlockArray _blockLayout;                        // reference to the array that represents the layout of the blocks
    private Collider _col;                                  // reference to the (base) collider component attached to this gameobject;
    private PlayerConfiguration _playerConfig;
    private Rigidbody _rb;                                  // reference to the rigidbody component attached to this gameobject
    private Transform _cameraTransform;                     // holds the camera transform locally to preserve the original transform from changes
    private Transform _ladderClimbed;                       // the current ladder being climbed
    private Transform _target;

    // Structs
    private Vector3 _colExtents;                            // the extents of the collider
    private float _horizontalMovement;                      // float that stores the value of the movement on the x-axis
    private float _jumpTimer = 0.0f;                        // float to compare the current runtime to the jump call time to determine if the call happened inside the jump delay interval
    private float _placementDistance = 0.5f;                // how far from the climber the ladder will be placed
    private readonly float _jumpForce = 3.0f;               // the force applied to the rigidbody at the start of a jump

    private bool _isHanging = false;                        // bool that keeps track of whether or not the Climber is hanging on a ledge or not
    private bool _isJumping = false;                        // bool to determine if the character is jumping or not
    private bool _hasLadder = true;                         // backup field that determines if the climber has his ladder or not
    private bool _isDead = false;

    public bool IsCarryingLadder
    {
        get => _hasLadder;
        private set => _hasLadder = value;
    }

    public bool IsDead
    { 
        get => _isDead;
        private set => _isDead = value;
    }

    #endregion
    public static FloodFill floodFiller;
    [SerializeField]
    private PositionConverter _positionConverter = null;
    #region GameLoop
    private void Awake()
    {
        // set the camera object if it's not assigned in the inspector
        if (_camera == null)
        {
            _camera = GameObject.FindGameObjectWithTag("MainCamera");
        }
        ConvertCameraToWorldTransform();

        // store the BlockArray in a field for easy access
        _blockLayout = GameLoop.Instance.Array;
    }

    private void Start()
    {
        // set up the components
        _rb = GetComponent<Rigidbody>();
        _col = GetComponent<Collider>();
        _colExtents = _col.bounds.extents;

        // disable the jumpmultiplier in the build
#if !UNITY_EDITOR
        JumpMultiplier = 1.0f;
#endif
    }

    private void FixedUpdate()
    {
        float movement = _horizontalMovement * _maxSpeed * Time.fixedDeltaTime;
        transform.position += new Vector3(movement, 0.0f, 0.0f);

        ClimbBlocks();
        UseGravity();
    }

    #endregion

    // can possibly be stored in another class / might be set to public so others can access this too
    private BlockPosition GetClimberBlockPosition()
    {
        return GameLoop.Instance.PositionConverter.ToBlockPosition(_blockLayout, transform.position);
    }


    private bool CheckLedgeAdjacentTo(BlockPosition sourceBlock, float direction)
    {
        // normalize direction
        int dirNormal = (int)(direction / Mathf.Abs(direction));

        BlockView adjacentBlock = _blockLayout.BlockAt(new BlockPosition(sourceBlock.X + (1 * dirNormal), sourceBlock.Y));

        if (adjacentBlock == null)
            return false;

        BlockView adjacentBlockNorth = _blockLayout.BlockAt(new BlockPosition(sourceBlock.X + (1 * dirNormal), sourceBlock.Y + 1));

        if (adjacentBlockNorth != null)
            return false;

        return true;
    }

    private bool FindSuitablePlacementOnSide(int direction, BlockPosition climberBlock, ref int ladderLength, ref bool isAtPreferredSide)
    {
        for (int i = 1; i <= _maxLadderHeight; i++)
        {
            BlockView blockNorth2 = _blockLayout.BlockAt(new BlockPosition(climberBlock.X, climberBlock.Y + i));
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
            BlockView blockNorthWest = _blockLayout.BlockAt(new BlockPosition(climberBlock.X - (1 * dirNormal), climberBlock.Y + i));
            BlockView blockUnderNorthWest = _blockLayout.BlockAt(new BlockPosition(climberBlock.X - (1 * dirNormal), climberBlock.Y + i - 1));

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
            BlockView blockNorthEast = _blockLayout.BlockAt(new BlockPosition(climberBlock.X + (1 * dirNormal), climberBlock.Y + i));
            BlockView blockUnderNorthEast = _blockLayout.BlockAt(new BlockPosition(climberBlock.X + (1 * dirNormal), climberBlock.Y + i - 1));

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

    private bool IsAtTopOfJump()
    {
        return !IsGrounded() && _rb.velocity.y >= 0 && _rb.velocity.y <= 0.1f;
    }

    private bool IsGrounded()
    {
        Physics.BoxCast(transform.position + (0.1f * Vector3.up), _colExtents, Vector3.down, out RaycastHit hit, transform.rotation, 0.1f, _collisionMask);
        return hit.collider != null && _rb.velocity.y <= 0;
    }

    private bool IsNearLedge(BlockPosition currentBlock)
    {
        // check ledge when moving in that direction
        if (_horizontalMovement != 0)
        {
            return CheckLedgeAdjacentTo(currentBlock, _horizontalMovement);
        }
        return false;
    }

    
    private float GetXLocationOnBlock()
    {
        return transform.position.x - Mathf.Floor(transform.position.x);
    }

    
    private void ClimbBlocks()
    {
        if (_horizontalMovement == 0)
            return;

        BlockPosition climberBlock = GetClimberBlockPosition();
        print(climberBlock.X + ", " + climberBlock.Y);
        if (IsAtTopOfJump() && IsNearLedge(climberBlock))
        {
            BlockView block;
            if (_horizontalMovement > 0)
            {
                block = _blockLayout.BlockAt(new BlockPosition(climberBlock.X + 1, climberBlock.Y));
            }
            else
            {
                block = _blockLayout.BlockAt(new BlockPosition(climberBlock.X - 1, climberBlock.Y));
            }
            ScaleBlock(block);
        }
    }

    private void ConvertCameraToWorldTransform()
    {
        _cameraTransform = _camera.transform;
        _cameraTransform.forward = Vector3.ProjectOnPlane(_camera.transform.forward, Vector3.up);
    }

    private void Jump()
    {
        _rb.AddForce(Vector3.up * _jumpForce * _jumpMultiplier * _jumpHeight, ForceMode.Impulse);
        _jumpTimer = 0.0f;
        _isHanging = false;
    }

    private void Move()
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

    private void ScaleBlock(BlockView block)
    {
        _isHanging = true;

        BlockPosition blockPosition = GameLoop.Instance.PositionConverter.ToBlockPosition(_blockLayout, block.transform.position);
        BlockPosition targetCell = new BlockPosition(blockPosition.X, blockPosition.Y + 1);
        Vector3 targetPosition = GameLoop.Instance.PositionConverter.ToWorldPosition(_blockLayout, targetCell);
        transform.position = targetPosition;
        _isHanging = false;
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
                return;
            }
            rotation = Quaternion.LookRotation(_cameraTransform.right, _cameraTransform.up);
        }
        else
        {
            print("Place Ladder East/Right");
            position.x += _placementDistance;
            if (!isAtPreferredSide)
            {
                rotation = Quaternion.LookRotation(_cameraTransform.right, _cameraTransform.up);
                return;
            }
            rotation = Quaternion.LookRotation(-_cameraTransform.right, _cameraTransform.up);
        }
        
        GameObject ladder = Instantiate(_ladderPrefab, position, rotation);
        ladder.GetComponent<Ladder>().Owner = this.gameObject;
        ladder.GetComponent<Ladder>().Length = ladderLength;
        IsCarryingLadder = false;
    }

    private void UseGravity()
    {
        if (IsGrounded() || _isHanging || IsClimbing)
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

    public void InitializePlayer(PlayerConfiguration config)
    {
        _playerConfig = config;
        _meshFilter.mesh = config.Character;
        _meshRenderer.material = config.PlayerMaterial;
    }

    public void KillPlayer()
    {
        var filledPositions = GameLoop.Instance.Array.GetAllArrayPositions();
        foreach (var floodedPosition in floodFiller._floodedPositions)
        {
            filledPositions.Remove(floodedPosition);
        }
        if (filledPositions.Contains(_positionConverter.ToBlockPosition(GameLoop.Instance.Array,this.gameObject.transform.position)))
        {
            this.gameObject.SetActive(false);
        }
    }

    #region Input
    public void OnMove(InputAction.CallbackContext context)
    {
        if (_isHanging)
        {
            return;
        }

        _horizontalMovement = context.ReadValue<float>();
        Move();
    }


    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.ReadValueAsButton())
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

    public void OnPlaceLadder(InputAction.CallbackContext context)
    {
        if (!context.ReadValueAsButton()) // ! is used to make sure the method gets called only once instead of twice (pressed: 1 -> 0; released: 0 -> 1)
        {
            if (IsCarryingLadder)
            {
                BlockPosition climberBlock = GetClimberBlockPosition();
                BlockView blockNorth = _blockLayout.BlockAt(new BlockPosition(climberBlock.X, climberBlock.Y + 1));

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

    public void OnPickupLadder(InputAction.CallbackContext context)
    {
        if (!context.ReadValueAsButton()) // ! is used to make sure the method gets called only once instead of twice (pressed: 1 -> 0; released: 0 -> 1)
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
                        if (ladderScript.Owner == this.gameObject)
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
    #endregion
}