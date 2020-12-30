using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomlyShowScript : MonoBehaviour
{
    [SerializeField]
    private float _percentChanceObjectGetsShown = 20f;

    void Start()
    {
        float randomNumber = Random.Range(1, 101);

        if (!ShouldDecalGetShown(randomNumber))
            Destroy(this.gameObject);
    }

    private bool ShouldDecalGetShown(float randomNumber)
    {
        return _percentChanceObjectGetsShown>=randomNumber;
    }
}
