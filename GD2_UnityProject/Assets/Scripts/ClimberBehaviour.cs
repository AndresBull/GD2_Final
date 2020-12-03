using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class ClimberBehaviour : MonoBehaviour
{
    // ==============================----- NOTE -----==================================
    //
    // This script is still work in progress. It is not recommended to use to playtest.
    //
    // ==========================----- END OF NOTE -----===============================

    [Tooltip("The maximum speed at which the climber moves around.")]
    public float MaxSpeed = 10f;
    [Tooltip("WARNING: PROTOTYPING ONLY\nThis multiplier can be used during prototyping to increase the jump to reach heights quickly. Not for use in the build.")]
    public float JumpMultiplier = 1f;
    public float JumpHeight = 0.5f;

    private Vector3 _colExtents;
    private float _horizontalMovement;

    private Rigidbody _rb;
    private Collider _col;
    private bool _isJumping = false, _desiredJump;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _col = GetComponent<Collider>();
        _colExtents = _col.bounds.extents;
    }

    void FixedUpdate()
    {
        float movement = _horizontalMovement * MaxSpeed * Time.fixedDeltaTime;
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
            // save jump call until touching the ground
            _desiredJump = true;
            if (!_isJumping && IsGrounded())
            {
                _isJumping = true;
                _desiredJump = false;
                Jump();
            }
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
        Physics.BoxCast(transform.position + (0.1f * Vector3.up), _colExtents, Vector3.down, out RaycastHit hit, transform.rotation, 0.1f);
        return hit.collider != null && _rb.velocity.y <= 0;
    }

    private void UseGravity()
    {
        if (IsGrounded())
        {
            _rb.useGravity = false;
            _rb.velocity = Vector3.ProjectOnPlane(_rb.velocity, -Physics.gravity.normalized);
            if (_desiredJump)
            {
                Jump();
                _desiredJump = false;
            }
            _isJumping = false;
            return;
        }
        _rb.useGravity = true;
    }

    private void Jump()
    {
#if UNITY_EDITOR
        _rb.AddForce(-Physics.gravity * JumpMultiplier * JumpHeight, ForceMode.Impulse);
#endif
        //_rb.AddForce(-Physics.gravity.normalized, ForceMode.Force);
    }
}
