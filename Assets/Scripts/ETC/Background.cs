using System.Collections;
using UnityEngine;

public class Background : MonoBehaviour
{
    public static Background Instance { get; private set; }
    [SerializeField] AudioSource _audioSource;
    [SerializeField] AudioClip _backgroundMusic;
    [SerializeField] AudioClip _thraxBossPhase1Music;
    [SerializeField] AudioClip _thraxBossPhase2Music;
    [SerializeField] AudioClip _thraxBossPhase3Music;


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
        StartCoroutine(PlayOriginalBackgroundMusic());
    }

    public IEnumerator FadeOut(float duration)
    {
        while (_audioSource.volume > 0)
        {
            _audioSource.volume -= Time.deltaTime * duration; // You can also multiply by a fade speed factor
            yield return null;
        }
        _audioSource.Stop();
    }

    public IEnumerator FadeIn(float duration)
    {
        _audioSource.volume = 0f;  // Ensure the volume starts from 0
        _audioSource.Play();
        while (_audioSource.volume < 0.15f)
        {
            _audioSource.volume += Time.deltaTime * duration; // You can also multiply by a fade speed factor
            yield return null;
        }
    }

    public IEnumerator TransitionBackgroundMusic(AudioClip backgroundMusic)
    {
        yield return StartCoroutine(FadeOut(0.1f)); // Ensure fade out completes
        _audioSource.clip = backgroundMusic;
        yield return new WaitForSeconds(0.1f);
        yield return StartCoroutine(FadeIn(0.1f)); // Start fading in after setting the new clip
    }

    public IEnumerator PlayOriginalBackgroundMusic()
    {
        Debug.Log("Playing Original Background Music");
        yield return StartCoroutine(TransitionBackgroundMusic(_backgroundMusic));
    }

    public IEnumerator PlayThraxBossPhase1Music()
    {
        Debug.Log("Playing Thrax Boss Phase 1 Music");
        yield return StartCoroutine(TransitionBackgroundMusic(_thraxBossPhase1Music));
    }

    public IEnumerator PlayThraxBossPhase2Music()
    {
        Debug.Log("Playing Thrax Boss Phase 2 Music");
        yield return StartCoroutine(TransitionBackgroundMusic(_thraxBossPhase2Music));
    }

    public IEnumerator PlayThraxBossPhase3Music()
    {
        Debug.Log("Playing Thrax Boss Phase 3 Music");
        yield return StartCoroutine(TransitionBackgroundMusic(_thraxBossPhase3Music));
    }
}
