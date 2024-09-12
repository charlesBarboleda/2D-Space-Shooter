using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NukeEnemy : Enemy
{
    private bool isOnCoolDown;
    [SerializeField] float _nukeRadius;
    [SerializeField] float _nukeDamage;
    [SerializeField] float _nukeChargeTime;
    [SerializeField] string _nukeExplosion;
    [SerializeField] string _nukeTarget;


    [SerializeField] float _attackRange;
    [SerializeField] float _coolDownTime = 3f;
    Vector3 _initTargetPos;
    GameObject _nukeTargetPool;



    protected override void OnDisable()
    {
        StopAllCoroutines();
        _nukeTargetPool.SetActive(false);

    }
    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (Vector2.Distance(transform.position, CheckForTargets().position) < _attackRange)
        {
            Debug.Log("In range to attack");
            if (_coolDownTime <= 0)
            {
                Attack();
                _coolDownTime = 5f;
            }
            else _coolDownTime -= Time.deltaTime;

        }

    }

    IEnumerator ShootNuke()
    {
        LayerMask _targetLayer = LayerMask.GetMask("Player") | LayerMask.GetMask("CrimsonFleet") | LayerMask.GetMask("ThraxArmada");
        Collider2D[] _hitColliders = Physics2D.OverlapCircleAll(_initTargetPos, _nukeRadius, _targetLayer);
        foreach (Collider2D hit in _hitColliders)
        {
            if (hit.CompareTag("Player") || hit.CompareTag("CrimsonFleet") || hit.CompareTag("ThraxArmada") || hit.CompareTag("EnemyDestroyable"))
            {
                hit.GetComponent<IDamageable>().TakeDamage(_nukeDamage);
            }
        }
        GameObject exp = ObjectPooler.Instance.SpawnFromPool(_nukeExplosion, _initTargetPos, Quaternion.identity);
        yield return new WaitForSeconds(2f);
        exp.SetActive(false);
    }

    IEnumerator ChargeNuke()
    {
        _initTargetPos = CheckForTargets().position;
        _nukeTargetPool = ObjectPooler.Instance.SpawnFromPool(_nukeTarget, _initTargetPos, Quaternion.identity);
        Debug.Log("Target Aim Position: " + _initTargetPos);
        yield return new WaitForSeconds(_nukeChargeTime);
        _nukeTargetPool.SetActive(false);
        StartCoroutine(ShootNuke());
    }

    protected override void Attack()
    {
        StartCoroutine(ChargeNuke());

    }

    public override void BuffedState()
    {
        _attackRange = _attackRange * 1.5f;
        _coolDownTime = _coolDownTime / 1.5f;
    }

    public override void UnBuffedState()
    {
        _attackRange = _attackRange / 1.5f;
        _coolDownTime = _coolDownTime * 1.5f;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(CheckForTargets().position, _nukeRadius);
    }

}
