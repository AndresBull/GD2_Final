﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BlockDropper : MonoBehaviour
{
    private GameObject HoldBlock;
    private float _speed = -25.0f;
    private float _horizontalMovement;
    private bool _hasBlock = true;
    private float _dropTimer;
    private float _dropDelay = 5.0f;
    public List<GameObject> _blocks = new List<GameObject>();
    private void Start()
    {
        RandomBlock();
    }
    void FixedUpdate()
    {
        float movement = _horizontalMovement * _speed * Time.fixedDeltaTime;
        transform.position += new Vector3(movement, 0.0f, 0.0f);
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
        var key = UnityEngine.Random.Range(0, 3);
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
                _dropTimer = 0;
                Drop();
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