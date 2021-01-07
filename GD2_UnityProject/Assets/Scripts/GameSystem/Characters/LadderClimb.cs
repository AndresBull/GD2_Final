using GameSystem.Props;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GameSystem.Characters
{
    public class LadderClimb : MonoBehaviour
    {
        [Tooltip("The speed of the climber when mounting a ladder.")]
        [SerializeField] private float _climbSpeed = 0;

        private ClimberBehaviour _moveScript;
        private Transform _ladderClimbed;

        private int _direction = 0;

        private bool _isClimbingUp = false;
        private bool _isOnBottomTrigger = false;
        private bool _isOnTopTrigger = false;

        public bool IsClimbing
        {
            get => _moveScript.IsClimbing;
            set => _moveScript.IsClimbing = value;
        }

        #region Unity Lifecycle
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
        #endregion

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

        #region Input
        public void OnClimbLadder(InputValue value)
        {
            float direction = value.Get<float>();

            if (direction < 0.5f && direction > -0.5f)
                return;

            // normalize direction
            _direction = (int)(direction / Mathf.Abs(direction));
        }
        #endregion
    }
}