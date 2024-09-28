using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;  // Singleton instance for easy access
    private Dictionary<AudioClip, AudioSource> playingClips = new Dictionary<AudioClip, AudioSource>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Method to play an AudioClip only if it is not already playing
    public void PlaySound(AudioSource source, AudioClip clip)
    {
        if (clip == null) return;

        // Check if the clip is already being played
        if (playingClips.ContainsKey(clip) && playingClips[clip].isPlaying)
        {
            // Clip is already playing, do not play it again
            Debug.Log("Clip already playing: " + clip.name);
            return;
        }

        // Play the clip and track it
        source.clip = clip;
        source.Play();

        // Add to the playing clips dictionary
        playingClips[clip] = source;

        // Start a coroutine to remove the clip once it has finished playing
        StartCoroutine(RemoveClipWhenFinished(source, clip));
    }

    // Coroutine to remove the clip from the playing list after it finishes
    private IEnumerator RemoveClipWhenFinished(AudioSource source, AudioClip clip)
    {
        yield return new WaitWhile(() => source.isPlaying);

        // Remove the clip from the dictionary once it's done playing
        if (playingClips.ContainsKey(clip))
        {
            playingClips.Remove(clip);
        }
    }
}
