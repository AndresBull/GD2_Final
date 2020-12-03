using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class ClimberBehaviour : MonoBehaviour
{
    [Header("Horizontal Movement")]
    [Tooltip("The maximum speed at which the climber moves around.")][SerializeField]
    private float _maxSpeed = 10f;
    
    [Header("Jumping")]
    [Tooltip("WARNING: PROTOTYPING ONLY\nThis multiplier can be used during prototyping to increase the jump to reach heights quickly. Not for use in the build. Set to 1 (one) to disable.")][SerializeField]
    private float _jumpMultiplier = 1f;
    [Tooltip("The height of the jump (in units).")][SerializeField]
    private float _jumpHeight = 0.5f;
    [Tooltip("The time interval in which a jump can be called before the player touches the ground (in seconds).")][SerializeField]
    private float _jumpDelay = 0.25f;

    [Header("Physics")]
    [Tooltip("The layers to test collision against. Default tests against everything.")][SerializeField]
    private LayerMask CollisionMask = ~0;

    private Vector3 _colExtents;
    private float _horizontalMovement;
    private float _jumpForce = 3.0f;
    private float _jumpTimer;

    private Rigidbody _rb;
    private Collider _col;
    private bool _isJumping = false;

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

    }

    public void OnPickupLadder(InputAction.CallbackContext context)
    {

    }

    private bool IsGrounded()
    {
        Physics.BoxCast(transform.position + (0.1f * Vector3.up), _colExtents, Vector3.down, out RaycastHit hit, transform.rotation, 0.1f, CollisionMask);
        return hit.collider != null && _rb.velocity.y <= 0;
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
}
