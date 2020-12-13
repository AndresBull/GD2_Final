using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Views;

[SelectionBase]
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
            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                GameLoop.Instance.ConnectBlockView(GameLoop.Instance.Array, gameObject.transform.GetChild(i).GetComponent<BlockView>());
                gameObject.transform.GetChild(i).transform.position -= Vector3.up * _speed * Time.deltaTime;
            }
            yield return new WaitForSeconds(_dropDownDelay);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            this.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            this.gameObject.GetComponent<Rigidbody>().isKinematic = false;
            _speed = 0;
        }
    }
}
