using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnerEnemy : Enemy
{
    public float spawnRate = 1f;
    public List<String> ships = new List<string>();
    public float spawnRadius = 10f;

    void Start()
    {
        Attack();
    }

    private void FixedUpdate()
    {
        Movement(player.transform);
    }

    private void SpawnRandomShips()
    {
        int randomShipIndex = Random.Range(0, ships.Count); // Randomly choose between easyShips and midShips
        // Calculate a random position on the circle
        float angle = Random.Range(0f, Mathf.PI * 2f);
        Vector3 spawnPosition = transform.position + new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * spawnRadius;

        GameObject enemy = ObjectPooler.Instance.SpawnFromPool(ships[randomShipIndex], spawnPosition, transform.rotation);
        GameManager.Instance.enemies.Add(enemy);

    }

    void OnDisable()
    {
        CancelInvoke();
    }

    public override void Attack()
    {
        InvokeRepeating("SpawnRandomShips", 0, spawnRate);
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
