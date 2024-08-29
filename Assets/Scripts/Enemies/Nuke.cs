using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NukeEnemy : Enemy
{
    [SerializeField] private GameObject nukePrefab;
    private Transform playerTarget;
    private bool isOnCoolDown;
    [SerializeField] private float attackRange;
    [SerializeField] private float coolDownTime = 1f;


    public override void OnEnable()
    {
        base.OnEnable();
        coolDownTime -= GameManager.Instance.Level() * 0.0001f;
    }
    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        if (playerTarget == null) return;
        if (Vector2.Distance(transform.position, playerTarget.position) < attackRange)
        {
            Attack();
        }

    }

    private void ShootNuke()
    {
        if (!isOnCoolDown)
        {
            GameObject nuke = Instantiate(nukePrefab, CheckForTargets().position, Quaternion.identity);
            Destroy(nuke, 6f);
            isOnCoolDown = true;
            StartCoroutine(Cooldown());
        }
    }


    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(coolDownTime);
        isOnCoolDown = false;
    }

    public override void Attack()
    {

        ShootNuke();

    }



}
