using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnerEnemy : Enemy
{
    [SerializeField] float _spawnRadius = 10f;
    public List<String> ships = new List<string>();

    void SpawnRandomShips()
    {
        int randomShipIndex = Random.Range(0, ships.Count); // Randomly choose between easyShips and midShips
        // Calculate a random position on the circle
        float angle = Random.Range(0f, Mathf.PI * 2f);
        Vector3 spawnPosition = transform.position + new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * _spawnRadius;

        GameObject enemy = ObjectPooler.Instance.SpawnFromPool(ships[randomShipIndex], spawnPosition, transform.rotation);
        GameManager.Instance.AddEnemy(enemy);


    }
    protected override void OnEnable()
    {
        base.OnEnable();
        Attack();
        AttackCooldown -= GameManager.Instance.Level * 0.01f;
        if (AttackCooldown <= 0.1f)
        {
            AttackCooldown = 0.1f;
        }
    }

    public override void BuffedState()
    {
        AttackCooldown = AttackCooldown / 1.5f;
    }
    public override void UnBuffedState()
    {
        base.UnBuffedState();
        AttackCooldown = AttackCooldown * 1.5f;
    }

    protected override void OnDisable()
    {

        CancelInvoke("SpawnRandomShips");
    }

    protected override void Attack()
    {
        InvokeRepeating("SpawnRandomShips", 0, AttackCooldown);
    }

    public void SetSpawnRadius(float radius)
    {
        _spawnRadius = radius;
    }
    public float GetSpawnRadius()
    {
        return _spawnRadius;
    }

    public void SetShips(List<string> ships)
    {
        this.ships = ships;
    }


}
