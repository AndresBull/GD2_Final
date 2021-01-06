using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicTransitionScript : MonoBehaviour
{
    private static MusicTransitionScript instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
