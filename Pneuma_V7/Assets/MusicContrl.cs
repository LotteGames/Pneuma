using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicContrl : MonoBehaviour
{
    public AudioClip[] AudioClip;

    public void PlayMusic(int Number)
    {
        GetComponent<AudioSource>().clip = AudioClip[Number];
        GetComponent<AudioSource>().Play();
    }
}
