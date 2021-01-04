using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    [Range(0.1f, 0.5f)]
    private float _shakeStrength = 0.15f;

    [Range(0.2f, 2f)]
    private float _shakeDuration = 0.5f;

    private float _strength, _duration, _fadeTime;
    private Vector3 _originalTransform, _newPosition;

    private void Start()
    {
        _originalTransform = transform.position;
        enabled = false;
    }

    private void Update()
    {
        _duration -= Time.deltaTime;

        float xShake = Random.Range(-1f, 1f) * _strength;
        float yShake = Random.Range(-1f, 1f) * _strength;

        _newPosition = _originalTransform + new Vector3(xShake, yShake, 0);

        _strength = Mathf.MoveTowards(_strength, 0f, Time.deltaTime * _fadeTime);

        if (_duration <= 0f)
        {
            EndShake();
        }
    }

    private void FixedUpdate()
    {
        transform.position = _newPosition;
    }


    private void EndShake()
    {
        transform.position = _originalTransform;
        _newPosition = _originalTransform;

        enabled = false;
    }

    public void SmallShake()
    {
        _strength = _shakeStrength;
        _duration = _shakeDuration;
        _fadeTime = _strength / _shakeDuration;

        enabled = true;
    }

    public void BigShake()
    {
        _strength = _shakeStrength * 2;
        _duration = _shakeDuration * 1.5f;
        _fadeTime = _strength / _shakeDuration;

        enabled = true;
    }
}
