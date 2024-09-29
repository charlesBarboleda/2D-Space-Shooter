using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossPhaseController : MonoBehaviour
{
    Health _health;
    Kinematics _kinematics;
    AttackManager _attackManager;
    AbilityHolder _abilityHolder;
    [SerializeField] List<GameObject> _exhausts = new List<GameObject>();
    bool hasPhased = false;
    bool hasPhased2 = false;
    Dissolve _dissolve;
    [SerializeField] float phase1Threshold = 1.5f;
    [SerializeField] float phase2Threshold = 10f;


    void Awake()
    {
        _abilityHolder = GetComponent<AbilityHolder>();
        _health = GetComponent<Health>();
        _kinematics = GetComponent<Kinematics>();
        _dissolve = GetComponent<Dissolve>();
        _attackManager = GetComponent<AttackManager>();
    }

    void Update()
    {
        if (_health.CurrentHealth <= _health.MaxHealth / phase1Threshold && !hasPhased)
        {
            hasPhased = true;
            Debug.Log("Health is below phase 1 threshold");
            StartCoroutine(Phase1());
            Debug.Log("Starting phase 1");
        }
        if (_health.CurrentHealth <= _health.MaxHealth / phase2Threshold && !hasPhased2)
        {
            hasPhased2 = true;
            Debug.Log("Health is below phase 2threshold");
            StartCoroutine(Phase2());
            Debug.Log("Starting phase 2");
        }

    }

    IEnumerator Phase1()
    {
        _abilityHolder.abilities[0].isUnlocked = false;
        // Disable the exhausts
        foreach (GameObject exhaust in _exhausts)
        {
            exhaust.SetActive(false);
        }
        // Disable boss movement
        _kinematics.ShouldRotate = false;
        _kinematics.ShouldMove = false;
        // Silence the boss
        _attackManager.IsSilenced = true;
        // Mark the boss dead to prevent further damage
        _health.isDead = true;
        // Dissolve the boss
        yield return StartCoroutine(_dissolve.DissolveOut());
        // Teleport the boss to the middle
        transform.position = new Vector3(0, 0, 0);
        // Pan the camera to the boss
        CameraFollowBehaviour.Instance.ActivateTargetCamera(transform);
        // Wait for 6 seconds to finish animations
        yield return new WaitForSeconds(6f);
        // Dissolve the boss back in
        yield return StartCoroutine(_dissolve.DissolveIn());
        // Enable the exhausts
        foreach (GameObject exhaust in _exhausts)
        {
            exhaust.SetActive(true);
        }
        // Increase camera ortho size
        yield return StartCoroutine(CameraFollowBehaviour.Instance.ChangeOrthographicSize(210, 1f));
        // Enable boss damageable
        _health.isDead = false;
        // Enable boss rotation
        _kinematics.ShouldRotate = true;

        // Get a random full map ability
        int randomAbility = Random.Range(1, 5);
        // Activate boss' second ability and make sure it's not on cooldown
        _abilityHolder.abilities[randomAbility].isUnlocked = true;
        _abilityHolder.abilities[randomAbility].currentCooldown = _abilityHolder.abilities[1].cooldown;
        // Wait for 7 seconds to finish animations
        yield return new WaitForSeconds(8f);
        // Activate the boss' first ability and make sure it's not on cooldown
        _abilityHolder.abilities[0].isUnlocked = true;
        _abilityHolder.abilities[0].duration = 30f;
        _abilityHolder.abilities[0].currentCooldown = _abilityHolder.abilities[0].cooldown;
        yield return new WaitForSeconds(26f);

        // Enable the boss movement
        _kinematics.ShouldMove = true;

        // Enable the boss attacks
        _attackManager.AttackCooldown = 0.75f;
        _attackManager.IsSilenced = false;


        // Reset the boss' ability 

        _abilityHolder.abilities[0].currentCooldown = 0f;
        _abilityHolder.abilities[randomAbility].isUnlocked = false;
        // Activate the player camera
        CameraFollowBehaviour.Instance.ActivatePlayerCamera();
    }


    IEnumerator Phase2()
    {
        // Disable the exhausts to get ready for the dissolve
        foreach (GameObject exhaust in _exhausts)
        {
            exhaust.SetActive(false);
        }
        // Disable boss movement
        _kinematics.ShouldRotate = false;
        _kinematics.ShouldMove = false;
        // Silence the boss
        _attackManager.IsSilenced = true;
        // Mark the boss dead to prevent further damage
        _health.isDead = true;
        // Dissolve the boss
        yield return StartCoroutine(_dissolve.DissolveOut());
        // Teleport the boss to the middle
        transform.position = new Vector3(0, 0, 0);
        // Pan the camera to the boss
        CameraFollowBehaviour.Instance.ActivateTargetCamera(transform);
        // Wait for 6 seconds to finish animations
        yield return new WaitForSeconds(6f);
        // Dissolve the boss back in
        yield return StartCoroutine(_dissolve.DissolveIn());
        // Enable the exhausts
        foreach (GameObject exhaust in _exhausts)
        {
            exhaust.SetActive(true);
        }
        // Increase camera ortho size
        yield return StartCoroutine(CameraFollowBehaviour.Instance.ChangeOrthographicSize(210, 1f));
        // Enable boss damageable
        _health.isDead = false;
        // Enable boss attacks with buffed attack rate
        _kinematics.ShouldRotate = true;

        int randomAbility = Random.Range(5, 7);

        // Activate boss' second ability and make sure it's not on cooldown
        _abilityHolder.abilities[randomAbility].isUnlocked = true;
        _abilityHolder.abilities[randomAbility].currentCooldown = _abilityHolder.abilities[1].cooldown;
        // Wait for 7 seconds to finish animations
        yield return new WaitForSeconds(8f);
        // Activate the boss' first ability and make sure it's not on cooldown
        _abilityHolder.abilities[0].isUnlocked = true;
        _abilityHolder.abilities[0].duration = 30f;
        _abilityHolder.abilities[0].currentCooldown = _abilityHolder.abilities[0].cooldown;
        yield return new WaitForSeconds(26f);

        // Enable the boss movement
        _kinematics.ShouldMove = true;

        // Activate the player camera
        CameraFollowBehaviour.Instance.ActivatePlayerCamera();


    }


    public float Phase1Threshold { get => phase1Threshold; set => phase1Threshold = value; }
    public float Phase2Threshold { get => phase2Threshold; set => phase2Threshold = value; }

}
