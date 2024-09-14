using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityUtils;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Faction))]
public abstract class Enemy : MonoBehaviour
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

    [SerializeField] GameObject _buffedParticles;
    [SerializeField] AudioClip _abilitySound;
    [SerializeField] AudioClip _spawnSound;
    [SerializeField] string _spawnAnimation;


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


    }

    protected virtual void Update()
    {

        if (_abilityHolder != null)
        {
            UseAbilities(TargetManager.CurrentTarget); // Uses the ability if the cooldown is 0
            if (_abilitySound != null) _audioSource.PlayOneShot(_abilitySound);
        }
        if (TargetManager.CurrentTarget != null)
        {

            float distanceToTarget = Vector2.Distance(transform.position, TargetManager.CurrentTarget.position);
            if (distanceToTarget < _attackManager.AimRange)
            {

                // Check if the target is the one we should shoot at
                if (_attackManager.IsTargetInRange(TargetManager.CurrentTarget) && _attackManager.ElapsedCooldown <= 0)
                {

                    Attack();
                    _attackManager.ElapsedCooldown = _attackManager.AttackCooldown;
                }
            }
        }
    }

    protected virtual void OnEnable()
    {
        IncreaseStatsPerLevel();
        StartCoroutine(StartSpawnAnimationWithDelay());

    }

    public virtual void UnBuffedState()
    {
        _buffedParticles.SetActive(false);
        Health.CurrentHealth /= 1.5f;
        Health.MaxHealth /= 1.5f;
        Kinematics.Speed /= 1.5f;
    }
    public virtual void BuffedState()
    {
        _buffedParticles.SetActive(true);
        Health.CurrentHealth *= 1.5f;
        Health.MaxHealth *= 1.5f;
        Kinematics.Speed *= 1.5f;

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
            if (ability.currentCooldown >= ability.cooldown) ability.TriggerAbility(gameObject, target);

        }
    }

    public virtual void IncreaseStatsPerLevel()
    {
        _health.CurrentHealth += GameManager.Instance.Level * 5f;
        _health.MaxHealth += GameManager.Instance.Level * 5f;


        _health.CurrencyDrop += GameManager.Instance.Level * 0.5f;

        _kinematics.Speed += GameManager.Instance.Level * 0.05f;

    }

    /// <summary>
    /// Getters and Setters
    /// </summary>
    public Health Health { get => _health; }
    public Kinematics Kinematics { get => _kinematics; }
    public Faction Faction { get => _faction; }
    public AttackManager AttackManager { get => _attackManager; }
    public TargetManager TargetManager { get => _targetManager; }
    public AudioSource AudioSource { get => _audioSource; }


}
