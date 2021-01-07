using GameSystem.Management;
using GameSystem.Views;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GameSystem.Characters
{
    public class OverlordHand : MonoBehaviour
    {
        [Header("Dropping")]
        [Tooltip("All possible types of blocks that can be dropped.")]
        [SerializeField] private List<GameObject> _blocks = null;
        [Tooltip("How long it takes for a new block to appear after one is dropped (in seconds).")]
        [SerializeField] private float _nextBlockDelay = 5.0f;
        [Tooltip("How long the block can be hold before it drops automatically (in seconds).")]
        [SerializeField] private float _maxHoldTime = 5.0f;

        [Header("Movement")]
        [Tooltip("The speed of the hand.")]
        [SerializeField] private float _speed = 10.0f;

        private PlayerConfiguration _playerConfig = null;

        private BlockView _holdBlockView;
        private BlockView _nextBlockView;

        private Vector2 _movementConstraints;
        private float _nextBlockTimer;
        private float _holdTimer;
        private float _horizontalMovement;

        private bool _hasBlock = true;
        private bool _canFastThrow = true;

        private void Awake()
        {
            Vector3 spawnPos = new Vector3(0, GameLoop.Instance.Field.Rows / 2 * GameLoop.Instance.FieldView.PositionConverter.BlockScale.y + 2, 0);
            transform.position = spawnPos;

            SetMovementConstraints(0);

            if (_nextBlockView == null)
                RandomNextBlock();

            RandomNextBlock();
        }

        public void SetPlayerConfig(PlayerConfiguration config)
        {
            _playerConfig = config;
        }

        private void SetMovementConstraints(int blockWidthAmount)
        {
            float fieldWidth = GameLoop.Instance.Field.Columns * GameLoop.Instance.FieldView.PositionConverter.BlockScale.x;

            float overlordWidth = 1f; // set to ~model width
            float blockWidth = blockWidthAmount * GameLoop.Instance.FieldView.PositionConverter.BlockScale.x;

            float maxWidth = Mathf.Max(overlordWidth, blockWidth);

            float range = (fieldWidth - maxWidth) / 2;
            _movementConstraints = new Vector2(-range, range);
        }

        private void Update()
        {
            if (GameLoop.Instance.StateMachine.CurrentState is PlayState)
                DropDelay();

            if (_holdTimer >= _maxHoldTime)
            {
                DropBlock();
                return;
            }
        }

        private void DropBlock()
        {
            _hasBlock = false;
            //_holdBlockView.gameObject.GetComponent<Collider>().enabled = true;
            _holdBlockView?.SlowDrop();
            _holdBlockView = null;
            _nextBlockTimer = 0;
            _holdTimer = 0;
        }

        private void FastDropBlock()
        {
            _hasBlock = false;
            //_holdBlockView.gameObject.GetComponent<Collider>().enabled = true;
            _holdBlockView?.FastDrop();
            _holdBlockView = null;
            _nextBlockTimer = 0;
            _holdTimer = 0;
        }

        private IEnumerator FastThrowCooldown(float timeInSec)
        {
            _canFastThrow = false;
            PlayerConfigManager.Instance.ToggleSpecialUsed(_playerConfig.PlayerIndex, _canFastThrow);

            yield return new WaitForSeconds(timeInSec);
            _canFastThrow = true;
            PlayerConfigManager.Instance.ToggleSpecialUsed(_playerConfig.PlayerIndex, _canFastThrow);

        }

        private void FixedUpdate()
        {
            // Hand movement
            float movement = _horizontalMovement * _speed * Time.fixedDeltaTime;
            transform.position = transform.position + new Vector3(movement, 0.0f, 0.0f);
            float clampedX = Mathf.Clamp(transform.position.x, _movementConstraints.x, _movementConstraints.y);
            transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);


            // Block movement
            _holdBlockView?.MoveAccordingToHand(transform.position);
        }

        public void DropDelay()
        {
            if (!_hasBlock)
            {
                _nextBlockTimer += Time.deltaTime;
                if (_nextBlockTimer >= _nextBlockDelay)
                {
                    RandomNextBlock();
                }
                return;
            }

            _holdTimer += Time.deltaTime;
        }

        private void RandomNextBlock()
        {
            _hasBlock = true;
            _holdBlockView = _nextBlockView;
            _nextBlockView = null;

            int key = UnityEngine.Random.Range(0, _blocks.Count);
            GameObject nextBlock = _blocks[key];

            Vector3 offscreenPosition = new Vector3(-20, 0, 0);
            GameObject go = Instantiate(nextBlock, offscreenPosition, nextBlock.transform.rotation);
            _nextBlockView = go.GetComponent<BlockView>();
            _nextBlockView.gameObject.GetComponent<Collider>().enabled = false;
        }

        // TODO: REMOVE the following methods if PlayerInput uses BroadcastMessages()
        //       UNCOMMMENT the following methods if PlayerInput uses Invoke Unity Events
        //public void OnMoveHand(InputAction.CallbackContext context)
        //{
        //    _horizontalMovement = context.ReadValue<float>();
        //}

        //public void OnDropBlock(InputAction.CallbackContext context)
        //{
        //    if (context.ReadValueAsButton())
        //    {
        //        if (_hasBlock)
        //        {
        //            _hasBlock = false;
        //            Drop();
        //            _dropTimer = 0;
        //            return;
        //        }
        //    }
        //}

        // TODO: USE the following methods if PlayerInput uses BroadcastMessages()
        //       REMOVE the following methods if PlayerInput uses Invoke Unity Events
        public void OnMoveHand(InputValue value)
        {
            _horizontalMovement = value.Get<float>();
        }

        public void OnDropBlock(InputValue value)
        {
            if (value.isPressed)
            {
                if (_hasBlock)
                {
                    DropBlock();
                }
            }
        }

        public void OnFastDropBlock(InputValue value)
        {
            if (value.isPressed)
            {
                if (_hasBlock && _canFastThrow)
                {
                    FastDropBlock();
                    StartCoroutine(FastThrowCooldown(10));
                }
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

        private void OnDestroy()
        {
            StopAllCoroutines();
        }
    }
}