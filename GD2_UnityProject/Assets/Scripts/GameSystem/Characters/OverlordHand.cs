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
        private BlockView _holdBlockView;
        private Vector2 _movementConstraints;
        private float _speed = 10.0f;
        private float _horizontalMovement;
        private float _dropTimer;
        private float _dropDelay = 5.0f;
        private bool _hasBlock = true;

        public List<GameObject> _blocks = new List<GameObject>();

        private void Awake()
        {
            var spawnPos = new Vector3(0, GameLoop.Instance.Field.Rows / 2 * GameLoop.Instance.FieldView.PositionConverter.BlockScale.y, 0);
            transform.position = spawnPos;

            SetMovementConstraints(0);

            RandomBlock();
        }

        private void SetMovementConstraints(int blockWidthAmount)
        {
            var fieldWidth = GameLoop.Instance.Field.Columns * GameLoop.Instance.FieldView.PositionConverter.BlockScale.x;

            var overlordWidth = 1f; //set to ~model width
            var blockWidth = blockWidthAmount * GameLoop.Instance.FieldView.PositionConverter.BlockScale.x;

            var maxWidth = Mathf.Max(overlordWidth, blockWidth);

            var range = (fieldWidth - maxWidth) / 2;
            _movementConstraints = new Vector2(-range, range);
        }

        private void Update()
        {
            DropDelay();

        }

        void FixedUpdate()
        {
            //Hand movement
            float movement = _horizontalMovement * _speed * Time.fixedDeltaTime;
            transform.position = transform.position + new Vector3(movement, 0.0f, 0.0f);
            float clampedX = Mathf.Clamp(transform.position.x, _movementConstraints.x, _movementConstraints.y);
            transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);


            //Block movement
            _holdBlockView?.MoveAccordingToHand(transform.position);
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
                    //SetMovementConstraints(_holdBlockView.BlockWidth);
                }
            }
        }

        private void RandomBlock()
        {
            var key = UnityEngine.Random.Range(0, _blocks.Count);
            var holdBlock = _blocks[key];

            var position = new Vector3(Mathf.Round(gameObject.transform.position.x), gameObject.transform.position.y - gameObject.transform.localScale.y, gameObject.transform.position.z);
            var go = Instantiate(holdBlock, position, holdBlock.transform.rotation);
            _holdBlockView = go.GetComponent<BlockView>();
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
                    _hasBlock = false;
                    _holdBlockView.StartCoroutine(_holdBlockView.Drop());
                    _holdBlockView = null;
                    _dropTimer = 0;
                    return;
                }
            }
        }
    }
}