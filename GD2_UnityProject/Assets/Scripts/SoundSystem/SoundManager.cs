using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class SoundManager : SingletonMonoBehaviour<SoundManager>
{
    /* 
     * Tutorial on this SoundManager;
     * Drop a mp3,wav, or other compatible type file in the Sounds folder
     * Add in the AudioSources a name for your soundeffect
     * Create a child in the SoundManager prefab with a SoundSource component (Turn of PlayOnAwake in editor)
     * Put the mp3 in a the AudioClip on that component
     * Assing that child to the AudioSource in the script of the prefab
     * Create a function like below
     * Call that function in any script using 
     * SoundManager.Instance.Play*InsertName*Sound();
     */

    [SerializeField]
    private AudioSource JumpSound, BlockLandSound;

    public void PlayJump()
    {
        JumpSound.Play();
    }

    public void PlayBlockLanded()
    {
        BlockLandSound.Play();
    }
}
