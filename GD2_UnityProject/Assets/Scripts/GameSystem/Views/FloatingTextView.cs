using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingTextView : MonoBehaviour
{
    private float _destroyTextTime = 1f;

    void Start()
    {
        Destroy(this.gameObject, _destroyTextTime);
    }
}
