using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class ClimberBehaviour : MonoBehaviour
{
    #region Fields
    [Header("Camera")][SerializeField]
    [Tooltip("The camera that renders the scene to the viewport. If no camera is assigned the object tagged \"MainCamera\" will be used.")]
    private GameObject _camera = null;

    [Header("Horizontal Movement")]
    [Tooltip("The maximum speed at which the climber moves around.")][SerializeField]
    private float _maxSpeed = 10f;

    [Header("Jumping")]
    [Tooltip("The kind of jumping system used by the caracter.")]
    private JumpMode _jumpMode = JumpMode.Simplified;
    [Tooltip("WARNING: PROTOTYPING ONLY\nThis multiplier can be used during prototyping to increase the jump to reach heights quickly. It will be ignored in the build. Set to 1 (one) to disable.")][SerializeField]
    private float _jumpMultiplier = 1f;
    [Tooltip("The height of the jump (in units).")][SerializeField]
    private float _jumpHeight = 0.5f;
    [Tooltip("The time interval in which a jump can be called before the player touches the ground (in seconds).")][SerializeField]
    private float _jumpDelay = 0.25f;

    [Header("Physics")]
    [Tooltip("The layers to test collision against. Default tests against everything.")][SerializeField]
    private LayerMask _collisionMask = ~0;
    [Tooltip("How far in front of the Climber must be tested for collision.")][SerializeField]
    private float _rayLength = 0.5f;

    [Header("Ladders")]
    [Tooltip("The ladder prefab for this character.")][SerializeField]
    private GameObject _ladderPrefab;
    [Tooltip("...")][SerializeField]
    private float _range = 1f;

    // Components
    private Rigidbody _rb;                                  // reference to the rigidbody component attached to this gameobject
    private Collider _col;                                  // reference to the (base) collider component attached to this gameobject;
    private Transform _cameraTransform;                     // holds the camera transform locally to preserve the original transform from changes
    private GameObject _ladder;
    
    // Structs
    private Vector3 _colExtents;                            // the extents of the collider
    private float _horizontalMovement;                      // float that stores the value of the movement on the x-axis
    private float _jumpTimer = 0.0f;                        // float to compare the current runtime to the jump call time to determine if the call happened inside the jump delay interval
    private readonly float _jumpForce = 3.0f;               // the force applied to the rigidbody at the start of a jump

    private bool _isJumping = false;                        // bool to determine if the character is jumping or not
    private bool _isHanging = false;                        // bool that keeps track of whether or not the Climber is hanging on a ledge or not
    private bool _hasLadder = true;                         // backup field that determines if the climber has its ladder or not

    public bool IsCarryingLadder
    {
        get => _hasLadder;
        private set => _hasLadder = value;
    }

    #endregion

    #region GameLoop
    private void Awake()
    {
        // set the camera object if it's not assigned in the inspector
        if (_camera == null)
        {
            _camera = GameObject.FindGameObjectWithTag("MainCamera");
        }
        ConvertCameraToWorldTransform();
    }

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _col = GetComponent<Collider>();
        _colExtents = _col.bounds.extents;

#if !UNITY_EDITOR
        JumpMultiplier = 1.0f;
#endif
    }

    void FixedUpdate()
    {
        float movement = _horizontalMovement * _maxSpeed * Time.fixedDeltaTime;
        transform.position += new Vector3(movement, 0.0f, 0.0f);

        CheckLedge();
        UseGravity();
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.blue;
    //    Gizmos.DrawRay(transform.position, transform.forward * _rayLength);
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawRay(transform.position + transform.up * _colExtents.x * 2, transform.forward * _rayLength);
    //}
    #endregion

    private void ConvertCameraToWorldTransform()
    {
        _cameraTransform = _camera.transform;
        _cameraTransform.forward = Vector3.ProjectOnPlane(_camera.transform.forward, Vector3.up);
    }

    private void CheckLedge() // this method will be replaced by a call to the grid to get the status of the block northeast
    {
        if (!IsGrounded() && _rb.velocity.y > 0 && _rb.velocity.y <= 0.1f)
        {
            // cast two rays, one to check if there is a block next to the climber, ...
            if (Physics.Raycast(new Ray(transform.position, transform.forward), out RaycastHit block, _rayLength, _collisionMask))
            {
                // a second one to check if there is NO block on top of the first block
                if (!Physics.Raycast(new Ray(transform.position + transform.up * _colExtents.x*2, transform.forward), out RaycastHit block2, _rayLength, _collisionMask))
                {
                    print("Hang");
                    _isHanging = true;
                    return;
                }
                _isHanging = false;
            }
        }
    }

    private void UseGravity()
    {
        if (IsGrounded() || _isHanging)
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

    private void Jump()
    {
        if (_jumpMode == JumpMode.Realistic)
        {
            _rb.AddForce(Physics.gravity.normalized * Mathf.Sqrt(2.0f * Physics.gravity.magnitude * _jumpHeight * _jumpMultiplier), ForceMode.Impulse);
        }
        else
        {
            _rb.AddForce(Vector3.up * _jumpForce * _jumpMultiplier * _jumpHeight, ForceMode.Impulse);
        }
        _jumpTimer = 0.0f;
        _isHanging = false;
    }

    private bool IsGrounded()
    {
        Physics.BoxCast(transform.position + (0.1f * Vector3.up), _colExtents, Vector3.down, out RaycastHit hit, transform.rotation, 0.1f, _collisionMask);
        return hit.collider != null && _rb.velocity.y <= 0;
    }

    #region Input
    public void OnMove(InputAction.CallbackContext context)
    {
        if (_isHanging)
        {
            return;
        }

        _horizontalMovement = context.ReadValue<float>();

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
        if (!context.ReadValueAsButton()) // ! is used to make sure the method gets called only once (need to read up on this bool to understand why)
        {
            if (IsCarryingLadder)
            {
                print("Place Ladder");
                GameObject ladder = Instantiate(_ladderPrefab, transform.position, transform.rotation);
                ladder.GetComponent<Ladder>().Owner = this.gameObject;
                IsCarryingLadder = false;
            }
            else
            {
                print("No ladder available");
            }
        }
    }

    public void OnPickupLadder(InputAction.CallbackContext context)
    {
        if (!context.ReadValueAsButton()) // ! is used to make sure the method gets called only once (need to read up on this bool to understand why)
        {
            if (!IsCarryingLadder)
            {
                // get all objects inside the range and filter for ladders
                var colliders = Physics.OverlapSphere(transform.position, _range);
                
                foreach (var collider in colliders)
                {
                    if (collider.gameObject.TryGetComponent(out Ladder ladderScript))
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


public enum JumpMode
{
    Realistic,
    Simplified
}