using GameSystem.Management;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GameSystem.Characters
{
    public class OverlordHand : MonoBehaviour
    {
        private GameObject _holdBlock;
        private Vector2 _movementConstraints;
        private float _speed = 25.0f;
        private float _horizontalMovement;
        private float _dropTimer;
        private float _dropDelay = 5.0f;
        private bool _hasBlock = true;

        public List<GameObject> _blocks = new List<GameObject>();

        private void Awake()
        {
            var spawnPos = new Vector3(0, GameLoop.Instance.Field.Rows / 2 * GameLoop.Instance.Field.PositionConverter.BlockScale.y, 0);
            transform.position = spawnPos;
            var fieldWidth = GameLoop.Instance.Field.Columns * GameLoop.Instance.Field.PositionConverter.BlockScale.x;
            _movementConstraints = new Vector2(-fieldWidth/2, fieldWidth/2);
            RandomBlock();
        }

        void FixedUpdate()
        {
            float movement = _horizontalMovement * _speed * Time.fixedDeltaTime;
            transform.position = transform.position + new Vector3(movement, 0.0f, 0.0f);
            float clampedX = Mathf.Clamp(transform.position.x, _movementConstraints.x, _movementConstraints.y);
            transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
            DropDelay();
        }

        public void DropDelay()
        {
            if (!_hasBlock)
            {
                _dropTimer += Time.deltaTime;
                if (_dropTimer >= _dropDelay)
                {
                    _hasBlock = true;
                    RandomBlock();
                }
            }
        }

        private void RandomBlock()
        {
            var key = UnityEngine.Random.Range(0, _blocks.Count);
            _holdBlock = _blocks[key];
        }

        private void Drop()
        {
            var position = new Vector3(Mathf.Round(gameObject.transform.position.x), gameObject.transform.position.y - gameObject.transform.localScale.y, gameObject.transform.position.z);
            Instantiate(_holdBlock, position, _holdBlock.transform.rotation);
        }

        // TODO: Remove the following methods if PlayerInput uses BroadcastMessages()
        //       Uncomment the follwing methods if PlayerInput uses Invoke Unity Events
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
        //       REMOVE the follwing methods if PlayerInput uses Invoke Unity Events
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
                    _hasBlock = false;
                    Drop();
                    _dropTimer = 0;
                    return;
                }
            }
        }
    }
}