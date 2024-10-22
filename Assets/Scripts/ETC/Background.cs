using System.Collections;
using DG.Tweening;
using UnityEngine;

public class Background : MonoBehaviour
{
    public static Background Instance { get; private set; }
    [SerializeField] AudioSource _audioSource;
    [SerializeField] AudioClip _syndicatesMusic;
    [SerializeField] AudioClip _thraxBossPhase1Music;
    [SerializeField] AudioClip _thraxBossPhase2Music;
    [SerializeField] AudioClip _thraxBossPhase3Music;
    [SerializeField] AudioClip _syndicatesBossPhase1Music;
    [SerializeField] AudioClip _syndicatesBossPhase2Music;
    [SerializeField] AudioClip _soloBossMusic;

    [SerializeField] AudioClip _countdownMusic;
    [SerializeField] AudioClip _thraxMusic;
    [SerializeField] AudioClip _crimsonFleetMusic;

    [SerializeField] AudioClip _invasionMusic;



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

    public void PlayInvasionMusic()
    {
        StartCoroutine(TransitionToMusic(_invasionMusic));
    }

    public void PlayHordeMusic(FactionType faction)
    {
        switch (faction)
        {
            case FactionType.Syndicates:
                PlaySyndicatesHordeMusic();
                break;
            case FactionType.ThraxArmada:
                PlayThraxArmadaHordeMusic();
                break;
            case FactionType.CrimsonFleet:
                PlayCrimsonFleetHordeMusic();
                break;
        }
    }

    public IEnumerator TransitionToMusic(AudioClip music)
    {
        _audioSource.DOFade(0f, 1f);
        yield return new WaitForSeconds(1f);
        _audioSource.Stop();
        _audioSource.clip = music;
        _audioSource.Play();
        _audioSource.DOFade(0.15f, 1f);
    }
    public void PlaySoloBossMusic()
    {
        StartCoroutine(TransitionToMusic(_soloBossMusic));
    }
    public void PlayCrimsonFleetHordeMusic()
    {
        StartCoroutine(TransitionToMusic(_crimsonFleetMusic));
    }

    public void PlayThraxArmadaHordeMusic()
    {
        StartCoroutine(TransitionToMusic(_thraxMusic));
    }

    public void PlaySyndicatesHordeMusic()
    {
        StartCoroutine(TransitionToMusic(_syndicatesMusic));
    }

    public void PlaySyndicatesBossPhase1Music()
    {
        StartCoroutine(TransitionToMusic(_syndicatesBossPhase1Music));
    }
    public void PlaySyndicatesBossPhase2Music()
    {
        StartCoroutine(TransitionToMusic(_syndicatesBossPhase2Music));
    }

    public void PlayThraxBossPhase1Music()
    {
        StartCoroutine(TransitionToMusic(_thraxBossPhase1Music));
    }

    public void PlayThraxBossPhase2Music()
    {
        StartCoroutine(TransitionToMusic(_thraxBossPhase2Music));
    }

    public void PlayThraxBossPhase3Music()
    {
        StartCoroutine(TransitionToMusic(_thraxBossPhase3Music));
    }

    public void PlayCountdownMusic()
    {
        StartCoroutine(TransitionToMusic(_countdownMusic));

    }
}
