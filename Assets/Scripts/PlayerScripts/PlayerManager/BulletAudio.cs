using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletAudio : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip shootingSound;
    // Start is called before the first frame update

    public void PlayOneShot(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
