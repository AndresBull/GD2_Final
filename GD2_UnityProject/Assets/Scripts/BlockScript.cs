using BoardBase;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Views;

[SelectionBase]
public class BlockScript : MonoBehaviour
{
    private float _speed = 1f;
    private float _dropDownDelay = 1f;
    void Start()
    {
        StartCoroutine(Drop());
    }

    private IEnumerator Drop()
    {
        while (Application.isPlaying)
        {
            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                GameLoop.Instance.ConnectBlockViews(gameObject.transform.GetComponent<CombinedBlockView>());
                gameObject.transform.GetChild(i).transform.position -= Vector3.up * _speed;
            }
            yield return new WaitForSeconds(_dropDownDelay);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            this.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            this.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            _speed = 0;
        }
    }
}
