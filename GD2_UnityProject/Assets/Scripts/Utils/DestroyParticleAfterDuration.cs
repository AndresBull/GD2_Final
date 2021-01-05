using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticleAfterDuration : MonoBehaviour
{
    private void Start()
    {
        Destroy(this.gameObject, this.GetComponent<ParticleSystem>().main.duration);
    }
}
