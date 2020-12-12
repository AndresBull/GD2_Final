using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Views;

public class BlockScript : MonoBehaviour
{
    private float _speed = 50.0f;
    private float _dropDownDelay = 0.5f;
    private GameObject _blockModel;
    private BlockView _blockView;

    void Start()
    {
        _blockView = gameObject.GetComponent<BlockView>();
        StartCoroutine(Drop());
    }

    private IEnumerator Drop()
    {
        while (Application.isPlaying)
        {
            this.transform.position -= Vector3.up * _speed * Time.deltaTime;
            GameLoop.Instance.ConnectBlockView(GameLoop.Instance.Array, _blockView);
            yield return new WaitForSeconds(_dropDownDelay);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            this.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            _speed = 0;
        }
    }
}
