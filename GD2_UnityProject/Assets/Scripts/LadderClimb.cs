﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LadderClimb : MonoBehaviour
{
    [Tooltip("The speed of the climber when mounting a ladder.")][SerializeField]
    private float _climbSpeed;

    private ClimberBehaviour _climberScript;
    private Transform _ladderClimbed;

    private bool _isClimbingUp;
    private bool _isOnTopTrigger;
    private bool _isOnBottomTrigger;
    private int _direction;

    public bool IsClimbing
    {
        get => _climberScript.IsClimbing;
        set => _climberScript.IsClimbing = value;
    }

    private void Start()
    {
        _climberScript = GetComponent<ClimberBehaviour>();
    }

    private void FixedUpdate()
    {
        ToggleClimb();

        if (!IsClimbing)
            return;

        Transform target;
        if (_isClimbingUp)
        {
            target = _ladderClimbed.GetComponent<Ladder>().Exit;
        }
        else
        {
            target = _ladderClimbed.GetComponent<Ladder>().Enter;
        }
        transform.position = Vector3.Lerp(transform.position, target.position, Time.fixedDeltaTime * _climbSpeed);

        // TODO: rotate climber to face ladder
    }

    private void ToggleClimb()
    {
        if (!_isOnBottomTrigger && !_isOnTopTrigger && !IsClimbing)
            return;

        if (_isOnBottomTrigger && !_isClimbingUp)
        {
            _climberScript.enabled = true;
            IsClimbing = false;
            _isClimbingUp = false;
        }
        if (_isOnBottomTrigger && !IsClimbing)
        {
            if (_direction == 1)
            {
                _climberScript.enabled = false;
                IsClimbing = true;
                _isClimbingUp = true;
                return;
            }
        }
        else if (_isOnTopTrigger && _isClimbingUp)
        {
            _climberScript.enabled = true;
            IsClimbing = false;
            _isClimbingUp = false;
        }
        if (_isOnTopTrigger && !IsClimbing)
        {
            if (_direction == -1)
            {
                _climberScript.enabled = false;
                IsClimbing = true;
                _isClimbingUp = false;
            }
        }

        // TODO: add the possibility to change direction mid-way through
    }

    private void OnTriggerEnter(Collider lTrigger)
    {
        if (lTrigger.tag.Equals("LEnterTrigger"))
        {
            _isOnBottomTrigger = true;
            _ladderClimbed = lTrigger.transform.root;
        }

        if (lTrigger.tag.Equals("LExitTrigger"))
        {
            _isOnTopTrigger = true;
            _ladderClimbed = lTrigger.transform.root;
        }
    }

    private void OnTriggerExit(Collider lTrigger)
    {
        if (lTrigger.tag.Equals("LEnterTrigger"))
        {
            _isOnBottomTrigger = false;
        }

        if (lTrigger.tag.Equals("LExitTrigger"))
        {
            _isOnTopTrigger = false;
        }
    }

    public void OnStartClimb(InputAction.CallbackContext context)
    {
        float direction = context.ReadValue<float>();
        
        if (direction < 0.5f && direction > -0.5f)
            return;

        // normalize direction
        _direction = (int)(direction / Mathf.Abs(direction));
    }
}
