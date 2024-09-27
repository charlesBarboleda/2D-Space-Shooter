using System.Collections;
using System.Collections.Generic;
using AssetUsageDetectorNamespace;
using Mkey;
using Unity.VisualScripting;
using UnityEngine;

public class BossPhaseController : MonoBehaviour
{
    Health _health;
    Kinematics _kinematics;
    AttackManager _attackManager;
    [SerializeField] List<GameObject> _exhausts = new List<GameObject>();
    bool hasPhased = false;
    Dissolve _dissolve;
    [SerializeField] float phaseThreshold = 1f;


    void Awake()
    {
        _health = GetComponent<Health>();
        _kinematics = GetComponent<Kinematics>();
        _dissolve = GetComponent<Dissolve>();
        _attackManager = GetComponent<AttackManager>();
    }

    void Update()
    {
        if (_health.CurrentHealth <= _health.MaxHealth / phaseThreshold && !hasPhased)
        {
            hasPhased = true;
            Debug.Log("Health is below threshold");
            StartCoroutine(Phase2());
            Debug.Log("Starting phase 2");
        }
    }

    IEnumerator Phase2()
    {
        foreach (GameObject exhaust in _exhausts)
        {
            exhaust.SetActive(false);
        }
        _kinematics.ShouldRotate = false;
        _kinematics.ShouldMove = false;
        _attackManager.IsSilenced = true;
        _health.isDead = true;
        yield return StartCoroutine(_dissolve.DissolveOut());
        transform.position = new Vector3(0, 0, 0);
        CameraFollowBehaviour.Instance.ActivateTargetCamera(transform);
        yield return new WaitForSeconds(3f);
        yield return StartCoroutine(_dissolve.DissolveIn());
        yield return StartCoroutine(CameraFollowBehaviour.Instance.ChangeOrthographicSize(200, 1f));
        foreach (GameObject exhaust in _exhausts)
        {
            exhaust.SetActive(true);
        }
        _attackManager.IsSilenced = false;
        _health.isDead = false;
        _kinematics.ShouldRotate = true;

    }


    public float PhaseThreshold { get => phaseThreshold; set => phaseThreshold = value; }

}
