using System.Collections;
using UnityEngine;

public class Background : MonoBehaviour
{
    public static Background Instance { get; private set; }
    [SerializeField] AudioSource _audioSource;
    [SerializeField] AudioClip _backgroundMusic;
    [SerializeField] AudioClip _thraxBossPhase1Music;
    [SerializeField] AudioClip _thraxBossPhase2Music;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        _audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        PlayOriginalBackgroundMusic();
    }

    public void PlayBackgroundMusic(AudioClip backgroundMusic)
    {
        _audioSource.Stop(); // Make sure the previous audio stops
        _audioSource.clip = backgroundMusic;
        _audioSource.Play();
    }

    public void PlayOriginalBackgroundMusic()
    {
        PlayBackgroundMusic(_backgroundMusic);
    }

    public void PlayThraxBossPhase1Music()
    {
        Debug.Log("Playing Thrax Boss Phase 1 Music");
        PlayBackgroundMusic(_thraxBossPhase1Music);
    }

    public void PlayThraxBossPhase2Music()
    {
        Debug.Log("Playing Thrax Boss Phase 2 Music");
        PlayBackgroundMusic(_thraxBossPhase2Music);
    }
}
