using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnerEnemy : Enemy
{
    public float spawnRate = 3f;
    public List<String> ships = new List<string>();
    public float spawnRadius = 10f;

    void Start()
    {
        Attack();
    }

    private void FixedUpdate()
    {
        Movement(target);
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
    public override void OnEnable()
    {
        base.OnEnable();
        spawnRate -= GameManager.Instance.level * 0.01f;
        if (spawnRate <= 0.3f)
        {
            spawnRate = 0.3f;
        }
    }

    void OnDisable()
    {
        CancelInvoke();
    }

    public override void Attack()
    {
        InvokeRepeating("SpawnRandomShips", 0, spawnRate);
    }


}
