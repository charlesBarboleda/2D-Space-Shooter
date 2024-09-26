using System.Collections;
using System.Collections.Generic;
using AssetUsageDetectorNamespace;
using Unity.VisualScripting;
using UnityEngine;

public class BossPhaseController : MonoBehaviour
{
    Health _health;
    Kinematics _kinematics;
    bool hasPhased = false;
    GameObject portal;
    [SerializeField] float phaseThreshold = 3f;


    void Awake()
    {
        _health = GetComponent<Health>();
        _kinematics = GetComponent<Kinematics>();
    }


    public float PhaseThreshold { get => phaseThreshold; set => phaseThreshold = value; }

}
