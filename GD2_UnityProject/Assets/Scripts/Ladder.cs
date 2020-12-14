using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Ladder : MonoBehaviour
{
    [SerializeField]
    private GameObject _exit;
    [SerializeField]
    private GameObject _enter;
    private Rigidbody _rb;
    private Quaternion _targetRot;
    private float _timer;
    private Transform _model;

    public GameObject Owner { get; set; } = null;           // the character that owns the ladder

    public Transform Exit => _exit.transform;
    public Transform Enter => _enter.transform;

    public int Length { get; set; } = 1;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _model = transform.GetChild(0).transform;

        _model.rotation *= Quaternion.Euler(-15.0f, 0.0f, 0.0f);
        _targetRot = Quaternion.Euler(15.0f + _model.eulerAngles.x, _model.eulerAngles.y, _model.eulerAngles.z);

        _timer = Time.time;
    }

    private void Start()
    {
        Vector3 scale = _model.localScale;
        scale.y = Length;
        _model.localScale = scale;
    }

    void FixedUpdate()
    {
        if (Time.time <= _timer + 1.0f)
        {
            _model.rotation = Quaternion.Slerp(_model.rotation, _targetRot, Time.fixedDeltaTime * 0.5f);
            return;
        }
        if (Time.time >= _timer + 2.0f)
        {
            _rb.isKinematic = true;
        }
    }
}
