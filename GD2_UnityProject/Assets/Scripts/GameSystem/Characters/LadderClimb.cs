﻿using GameSystem.Props;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GameSystem.Characters
{
    public class LadderClimb : MonoBehaviour
    {
        [Tooltip("The speed of the climber when mounting a ladder.")][SerializeField]
        private float _climbSpeed = 0;

        private ClimberBehaviour _moveScript;
        private Transform _ladderClimbed;

        private bool _isClimbingUp = false;
        private bool _isOnTopTrigger = false;
        private bool _isOnBottomTrigger = false;
        private int _direction = 0;

        public bool IsClimbing
        {
            get => _moveScript.IsClimbing;
            set => _moveScript.IsClimbing = value;
        }

        private void Start()
        {
            _moveScript = GetComponent<ClimberBehaviour>();
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
            
            if (_isOnBottomTrigger)
            {
                if (IsClimbing && !_isClimbingUp)
                {
                    IsClimbing = false;
                    _moveScript.enabled = true;
                }
                else if (!IsClimbing && _direction == 1)
                {
                    _moveScript.enabled = false;
                    IsClimbing = true;
                    _isClimbingUp = true;
                    return;
                }
            }
            else if (_isOnTopTrigger)
            {
                if (IsClimbing && _isClimbingUp)
                {
                    IsClimbing = false;
                    _isClimbingUp = false;
                    _moveScript.enabled = true;
                }
                else if (!IsClimbing && _direction == -1)
                {
                    _moveScript.enabled = false;
                    IsClimbing = true;
                    _isClimbingUp = false;
                }
            }

            // TODO: add the possibility to change direction mid-way through
        }

        private void OnTriggerEnter(Collider lTrigger)
        {
            if (lTrigger.CompareTag("LEnterTrigger"))
            {
                _isOnBottomTrigger = true;
                _ladderClimbed = lTrigger.transform.root;
            }

            if (lTrigger.CompareTag("LExitTrigger"))
            {
                _isOnTopTrigger = true;
                _ladderClimbed = lTrigger.transform.root;
            }
        }

        private void OnTriggerExit(Collider lTrigger)
        {
            if (lTrigger.CompareTag("LEnterTrigger"))
            {
                _isOnBottomTrigger = false;
                _direction = 0;
            }

            if (lTrigger.CompareTag("LExitTrigger"))
            {
                _isOnTopTrigger = false;
                _direction = 0;
            }
        }

        // TODO: REMOVE the following methods if PlayerInput uses BroadcastMessages()
        //       USE the following methods if PlayerInput uses Invoke Unity Events

        //public void OnClimbLadder(InputAction.CallbackContext context)
        //{
        //    float direction = context.ReadValue<float>();

        //    if (direction < 0.5f && direction > -0.5f)
        //        return;

        //    normalize direction
        //    _direction = (int)(direction / Mathf.Abs(direction));
        //}

        // TODO: USE the following methods if PlayerInput uses BroadcastMessages()
        //       REMOVE the follwing methods if PlayerInput uses Invoke Unity Events
        public void OnClimbLadder(InputValue value)
        {
            float direction = value.Get<float>();

            if (direction < 0.5f && direction > -0.5f)
                return;

            // normalize direction
            _direction = (int)(direction / Mathf.Abs(direction));
        }

    }
}