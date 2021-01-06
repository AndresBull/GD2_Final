using BoardSystem;
using GameSystem.Management;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSystem.Props
{
    public class Ladder : MonoBehaviour
    {
        [SerializeField] private GameObject _exit = null;
        [SerializeField] private GameObject _enter = null;

        private Transform _model;
        private Quaternion _targetRot;
        private float _timer;

        public GameObject Owner { get; set; } = null;           // the character that owns the ladder
        public Transform Exit => _exit.transform;
        public Transform Enter => _enter.transform;
        public int Length { get; set; } = 1;

        void Awake()
        {
            _model = transform.GetChild(0).transform;
            _targetRot = Quaternion.Euler(-15.0f, transform.eulerAngles.y, transform.eulerAngles.z);

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
            if (Time.time <= _timer + 5.0f)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, _targetRot, Time.fixedDeltaTime * 4f);
                return;
            }
        }

        internal void Break()
        {
            // TODO: change to broken ladder
            Destroy(gameObject);
        }
    }
}