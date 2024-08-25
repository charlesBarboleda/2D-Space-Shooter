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
        coolDownTime -= GameManager.Instance.level * 0.0001f;
    }
    // Update is called once per frame
    private void FixedUpdate()
    {
        if (playerTarget == null) return;
        if (Vector2.Distance(transform.position, playerTarget.position) < attackRange)
        {
            Attack();
        }
        if (shouldRotate) Aim(player.transform);
        Movement(player.transform);
    }

    private void ShootNuke()
    {
        if (!isOnCoolDown)
        {
            GameObject nuke = Instantiate(nukePrefab, player.transform.position, Quaternion.identity);
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


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerBullet"))
        {
            TakeDamage(other.GetComponent<Bullet>().BulletDamage);
        }
    }
}
