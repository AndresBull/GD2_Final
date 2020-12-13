using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Ladder : MonoBehaviour
{
    [SerializeField]
    private GameObject _enter;
    [SerializeField]
    private GameObject _exit;
    private Rigidbody _rb;
    private Quaternion _target;
    private float _timer;

    public GameObject Owner { get; set; } = null;           // the character that owns the ladder

    public int Length { get; set; }

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();

        transform.rotation *= Quaternion.Euler(-10.0f, 0.0f, 0.0f);
        _target = Quaternion.Euler(15.0f + transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
        _timer = Time.time;
    }

    void FixedUpdate()
    {
        if (Time.time <= _timer + 1.0f)
        {
            print("Turning");
            transform.rotation = Quaternion.Slerp(transform.rotation, _target, Time.fixedDeltaTime * 0.5f);
            return;
        }
        if (Time.time >= _timer + 2.0f)
        {
            _rb.isKinematic = true;
        }
    }

    void SwitchPoints()
    {
        // switch enter and exit points when on ladder
    }
}
