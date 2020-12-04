using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockScript : MonoBehaviour
{
    private float _speed = 100.0f;
    private float _dropDownDelay = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Drop());
    }

    // Update is called once per frame
    

    private IEnumerator Drop()
    {
        while (Application.isPlaying)
        {
            this.transform.position -= Vector3.up * _speed * Time.deltaTime;
            yield return new WaitForSeconds(_dropDownDelay);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            _speed = 0;
        }
    }
}
