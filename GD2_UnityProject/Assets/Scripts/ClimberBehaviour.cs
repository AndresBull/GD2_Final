using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class ClimberBehaviour : MonoBehaviour
{
    #region Fields
    [Header("Horizontal Movement")]
    [Tooltip("The maximum speed at which the climber moves around.")][SerializeField]
    private float _maxSpeed = 10f;
    
    [Header("Jumping")]
    [Tooltip("WARNING: PROTOTYPING ONLY\nThis multiplier can be used during prototyping to increase the jump to reach heights quickly. It will be ignored in the build. Set to 1 (one) to disable.")][SerializeField]
    private float _jumpMultiplier = 1f;
    [Tooltip("The height of the jump (in units).")][SerializeField]
    private float _jumpHeight = 0.5f;
    [Tooltip("The time interval in which a jump can be called before the player touches the ground (in seconds).")][SerializeField]
    private float _jumpDelay = 0.25f;

    [Header("Physics")]
    [Tooltip("The layers to test collision against. Default tests against everything.")][SerializeField]
    private LayerMask CollisionMask = ~0;

    private Rigidbody _rb;                                  // reference to the rigidbody component attached to this gameobject
    private Collider _col;                                  // reference to the (base) collider component attached to this gameobject;

    private Vector3 _colExtents;                            // the extents of the collider
    private float _horizontalMovement;                      // float that stores the value of the movement on the x-axis
    private float _jumpTimer = 0.0f;                        // float to compare the current runtime to the jump call time to determine if the call happened inside the jump delay interval
    private readonly float _jumpForce = 3.0f;               // the force applied to the rigidbody at the start of a jump

    private bool _isJumping = false;                        // bool to determine if the character is jumping or not
    #endregion

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
        UseGravity();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _horizontalMovement = context.ReadValue<float>();
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
        if (context.ReadValueAsButton())
        {
            print("Place Ladder");
        }
    }

    public void OnPickupLadder(InputAction.CallbackContext context)
    {
        if (context.ReadValueAsButton())
        {
            print("Pick up Ladder");
        }
    }

    private void UseGravity()
    {
        if (IsGrounded())
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
        _rb.AddForce(Vector3.up * _jumpForce * _jumpMultiplier * _jumpHeight, ForceMode.Impulse);
        _jumpTimer = 0.0f;
    }

    private bool IsGrounded()
    {
        Physics.BoxCast(transform.position + (0.1f * Vector3.up), _colExtents, Vector3.down, out RaycastHit hit, transform.rotation, 0.1f, CollisionMask);
        return hit.collider != null && _rb.velocity.y <= 0;
    }
}
