using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityUtils;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Faction))]
public abstract class Enemy : MonoBehaviour, ITargetable
{
    // References
    Health _health;
    Kinematics _kinematics;
    AttackManager _attackManager;
    TargetManager _targetManager;
    Faction _faction;
    AudioSource _audioSource;
    AbilityHolder _abilityHolder;

    // Animations 

    [SerializeField] List<ParticleSystem> _abilityParticles = new List<ParticleSystem>();
    [SerializeField] GameObject _buffedParticles;
    [SerializeField] AudioClip _abilitySound;
    [SerializeField] AudioClip _spawnSound;
    [SerializeField] string _spawnAnimation;

    [SerializeField] bool isThraxBoss = false;
    string _enemyID;
    int _priority;

    protected abstract void Attack();

    protected virtual void Awake()
    {

        TryGetComponent<AbilityHolder>(out AbilityHolder abilityHolder);
        _abilityHolder = abilityHolder;
        _faction = GetComponent<Faction>();
        _audioSource = GetComponent<AudioSource>();
        _attackManager = GetComponent<AttackManager>();
        _health = GetComponent<Health>();
        _targetManager = GetComponent<TargetManager>();
        _kinematics = GetComponent<Kinematics>();
        _faction.AddAllyFaction(_faction.factionType);
        _enemyID = Guid.NewGuid().ToString();
        // DisableAudioSource();


    }

    protected virtual void Update()
    {
        // Disable the audio source if the camera is too far away
        if (Vector2.Distance(transform.position, Camera.main.transform.position) > 100f)
        {
            DisableAudioSource();
        }
        else EnableAudioSource();

        if (_abilityHolder != null)
        {
            UseAbilities(TargetManager.CurrentTarget.transform); // Uses the ability if the cooldown is 0
            if (_abilitySound != null) _audioSource.PlayOneShot(_abilitySound);
        }
        if (TargetManager.CurrentTarget.transform != null)
        {

            float distanceToTarget = Vector2.Distance(transform.position, TargetManager.CurrentTarget.transform.position);
            if (distanceToTarget < _attackManager.AimRange)
            {

                // Check if the target is the one we should shoot at
                if (_attackManager.IsTargetInRange(TargetManager.CurrentTarget.transform) && _attackManager.ElapsedCooldown <= 0 && _attackManager.IsSilenced == false)
                {

                    Attack();
                    _attackManager.ElapsedCooldown = _attackManager.AttackCooldown;
                }
            }
        }
    }

    protected virtual void OnEnable()
    {
        EnableAudioSource();
        IncreaseStatsPerLevel();
        StartCoroutine(StartSpawnAnimationWithDelay());
        TargetManager.RegisterTarget(this);
        Debug.Log("Registered target: " + gameObject.name);

    }
    protected virtual void OnDisable()
    {
        TargetManager.UnregisterTarget(this);
        Debug.Log("Unregistered target: " + gameObject.name);
    }

    void EnableAudioSource() => _audioSource.enabled = true;
    void DisableAudioSource() => _audioSource.enabled = false;



    public virtual void UnBuffedState()
    {
        _buffedParticles.SetActive(false);
        Health.CurrentHealth /= 1.5f;
        Health.MaxHealth /= 1.5f;
        Kinematics.MaxSpeed /= 1.5f;
    }
    public virtual void BuffedState()
    {
        _buffedParticles.SetActive(true);
        Health.CurrentHealth *= 1.5f;
        Health.MaxHealth *= 1.5f;
        Kinematics.MaxSpeed *= 1.5f;

    }




    IEnumerator SpawnAnimation()
    {
        if (_spawnSound != null) _audioSource.PlayOneShot(_spawnSound);
        GameObject obj = ObjectPooler.Instance.SpawnFromPool(_spawnAnimation, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(2f);
        obj.SetActive(false);
    }
    IEnumerator StartSpawnAnimationWithDelay()
    {
        yield return new WaitForEndOfFrame();
        StartCoroutine(SpawnAnimation());
    }



    protected void UseAbilities(Transform target)
    {
        foreach (Ability ability in _abilityHolder.abilities)
        {
            // Check if the ability can be triggered
            if (ability.currentCooldown >= ability.cooldown && ability.isUnlocked)
            {
                // Trigger the ability
                ability.TriggerAbility(gameObject, target);
                Debug.Log("Triggered ability: " + ability.name);
                StartCoroutine(PlayAbilityParticles());
                Debug.Log("Played ability particles from UseAbilities");


            }
        }
    }

    public IEnumerator PlayAbilityParticles()
    {
        Debug.Log("Playing ability particles");
        foreach (ParticleSystem particle in _abilityParticles)
        {
            particle.gameObject.SetActive(true);
            particle.Play();
            Debug.Log("Played ability particles");
        }
        yield return new WaitForSeconds(isThraxBoss ? Mathf.Max(LevelManager.Instance.CurrentLevelIndex * 0.5f, 20f) : 0.1f);
        foreach (ParticleSystem particle in _abilityParticles)
        {
            particle.Stop();
            particle.gameObject.SetActive(false);
            Debug.Log("Stopped ability particles");
        }
    }

    public virtual void IncreaseStatsPerLevel()
    {
        _health.CurrentHealth += LevelManager.Instance.CurrentLevelIndex * 5f;
        _health.MaxHealth += LevelManager.Instance.CurrentLevelIndex * 5f;


        _health.CurrencyDrop += LevelManager.Instance.CurrentLevelIndex * 0.5f;

        _kinematics.MaxSpeed += LevelManager.Instance.CurrentLevelIndex * 0.05f;

    }
    public Vector3 GetPosition() => transform.position;
    public bool IsAlive() => _health != null && !_health.isDead;
    public FactionType GetFactionType() => _faction.factionType;


    /// <summary>
    /// Getters and Setters
    /// </summary>
    public Health Health { get => _health; }
    public Kinematics Kinematics { get => _kinematics; }
    public Faction Faction { get => _faction; }
    public AttackManager AttackManager { get => _attackManager; }
    public TargetManager TargetManager { get => _targetManager; }
    public AudioSource AudioSource { get => _audioSource; }
    public string EnemyID { get => _enemyID; set => _enemyID = value; }
    public int Priority { get => _priority; set => _priority = value; }


}
