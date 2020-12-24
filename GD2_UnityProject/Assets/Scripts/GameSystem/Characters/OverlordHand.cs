using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GameSystem.Characters
{
    public class OverlordHand : MonoBehaviour
    {
        [SerializeField]
        [Range(0, 10)]
        private int _movementConstraint;

        private GameObject HoldBlock;
        private Vector3 spawnLocation = new Vector3(0, 4, 0);
        private Vector2 _movementConstraints;
        private float _speed = 25.0f;
        private float _horizontalMovement;
        private float _dropTimer;
        private float _dropDelay = 5.0f;
        private bool _hasBlock = true;

        public List<GameObject> _blocks = new List<GameObject>();

        private void Awake()
        {
            this.gameObject.transform.position = spawnLocation;
            _movementConstraints = new Vector2(-_movementConstraint, _movementConstraint);
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
            HoldBlock = _blocks[key];
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            _horizontalMovement = context.ReadValue<float>();
        }

        public void OnDrop(InputAction.CallbackContext context)
        {
            if (context.ReadValueAsButton())
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

        private void Drop()
        {
            var position = new Vector3(Mathf.Round(gameObject.transform.position.x), gameObject.transform.position.y - gameObject.transform.localScale.y, gameObject.transform.position.z);
            Instantiate(HoldBlock, position, HoldBlock.transform.rotation);
        }
    }
}