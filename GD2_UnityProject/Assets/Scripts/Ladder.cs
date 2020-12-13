using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Ladder : MonoBehaviour
{
    [SerializeField]
    private GameObject _point1;
    [SerializeField]
    private GameObject _point2;
    [SerializeField]
    private GameObject _exit;
    private Rigidbody _rb;
    private Quaternion _target;
    private float _timer;
    private Transform _model;

    public GameObject Owner { get; set; } = null;           // the character that owns the ladder

    public Transform Point1 => _point1.transform;
    public Transform Point2 => _point2.transform;
    public Transform Exit => _exit.transform;

    public int Length { get; set; } = 1;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _model = transform.GetChild(0).transform;

        _model.rotation *= Quaternion.Euler(-15.0f, 0.0f, 0.0f);
        _target = Quaternion.Euler(15.0f + _model.eulerAngles.x, _model.eulerAngles.y, _model.eulerAngles.z);

        Vector3 scale = _model.localScale;
        scale.y = Length;
        _model.localScale = scale;

        _timer = Time.time;
    }

    void FixedUpdate()
    {
        if (Time.time <= _timer + 1.0f)
        {
            _model.rotation = Quaternion.Slerp(_model.rotation, _target, Time.fixedDeltaTime * 0.5f);
            return;
        }
        if (Time.time >= _timer + 2.0f)
        {
            _rb.isKinematic = true;
        }
    }
}
