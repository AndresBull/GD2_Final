using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BlockDropper : MonoBehaviour
{
    public GameObject HoldBlock;
    private float _speed = 50.0f;
    private float _horizontalMovement;
    private bool _hasBlock = true;
    private float _dropTimer;
    private float _dropDelay;
    // Update is called once per frame
    void FixedUpdate()
    {
        float movement = _horizontalMovement * _speed * Time.fixedDeltaTime;
        transform.position += new Vector3(movement, 0.0f, 0.0f);
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
                return;
            }
        }
    }

    private void Drop()
    {
        Instantiate(HoldBlock, gameObject.transform.position, gameObject.transform.rotation);
    }
}
