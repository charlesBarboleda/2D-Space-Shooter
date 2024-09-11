using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NukeEnemy : Enemy
{
    [SerializeField] GameObject _nukePrefab;
    private bool isOnCoolDown;
    [SerializeField] float _attackRange;
    [SerializeField] float _coolDownTime = 1f;


    protected override void OnEnable()
    {
        base.OnEnable();
        _coolDownTime -= GameManager.Instance.Level * 0.0001f;
    }
    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (Vector2.Distance(transform.position, CheckForTargets().position) < _attackRange)
        {
            Debug.Log("In range to attack");
            Attack();
        }

    }

    private void ShootNuke()
    {
        Debug.Log("Checking if on cooldown");
        if (!isOnCoolDown)
        {
            Debug.Log("Shooting Nuke");
            GameObject nuke = Instantiate(_nukePrefab, CheckForTargets().position, Quaternion.identity);
            Destroy(nuke, 6f);
            isOnCoolDown = true;
            StartCoroutine(Cooldown());
        }
    }


    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(_coolDownTime);
        isOnCoolDown = false;
    }

    protected override void Attack()
    {
        Debug.Log("Attacking");
        ShootNuke();

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



}
