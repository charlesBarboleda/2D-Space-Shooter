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
    void Start()
    {
        playerTarget = GameObject.Find("Player").transform;
    }


    // Update is called once per frame
    private void FixedUpdate()
    {
        if (playerTarget == null) return;
        if (Vector2.Distance(transform.position, playerTarget.position) < attackRange)
        {
            Attack();
        }
        if (shouldRotate) Aim();
        Movement();
    }

    private void ShootNuke()
    {
        if (!isOnCoolDown)
        {
            GameObject nuke = Instantiate(nukePrefab, playerTarget.position, Quaternion.identity);
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

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("PlayerLaser"))
        {
            TakeDamage(LaserDamageByLevel());
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerBullet"))
        {
            TakeDamage(other.GetComponent<Bullet>().BulletDamage);
            other.gameObject.SetActive(false);
        }
    }
}
