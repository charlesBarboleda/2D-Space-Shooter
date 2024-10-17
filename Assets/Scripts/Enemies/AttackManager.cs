using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    [SerializeField] float _aimRange = 20f;
    [SerializeField] float _attackCooldown = 1f;
    [SerializeField] bool _isSilenced;
    Kinematics _kinematics;
    TargetManager _targetManager;
    float _elapsedCooldown;


    void Start()
    {
        _kinematics = GetComponent<Kinematics>();
        _targetManager = GetComponent<TargetManager>();
    }

    void OnEnable()
    {
        _aimRange = _kinematics.StopDistance + 5f;
    }


    // Update is called once per frame
    void Update()
    {
        HandleCooldown();
    }

    void HandleCooldown()
    {
        if (_elapsedCooldown > 0)
        {
            _elapsedCooldown -= Time.deltaTime;
        }
    }


    public bool IsTargetInRange()
    {
        CompositeCollider2D targetCollider = _targetManager.CurrentTarget.GetComponent<CompositeCollider2D>();
        float targetSize = targetCollider != null ? targetCollider.bounds.extents.magnitude : 0;

        // Add targetSize to account for bigger targets
        return Vector3.Distance(transform.position, _targetManager.TargetPosition) <= _aimRange + targetSize;
    }


    public float AimRange { get => _aimRange; set => _aimRange = value; }
    public float AttackCooldown { get => _attackCooldown; set => _attackCooldown = value; }
    public float ElapsedCooldown { get => _elapsedCooldown; set => _elapsedCooldown = value; }
    public bool IsSilenced { get => _isSilenced; set => _isSilenced = value; }
}
