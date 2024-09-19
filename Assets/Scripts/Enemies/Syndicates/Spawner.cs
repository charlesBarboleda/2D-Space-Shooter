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
    [SerializeField] int _shipsPerSpawn;
    public List<String> ships = new List<string>();


    void SpawnRandomShips(int numberOfShipsPerSpawn = 1)
    {
        for (int i = 0; i < numberOfShipsPerSpawn; i++)
        {
            int randomShipIndex = Random.Range(0, ships.Count);
            float angle = Random.Range(0f, Mathf.PI * 2f);
            Vector3 spawnPosition = transform.position + new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * _spawnRadius;

            GameObject enemy = ObjectPooler.Instance.SpawnFromPool(ships[randomShipIndex], spawnPosition, transform.rotation);
            SpawnerManager.Instance.AddEnemy(enemy);
        }


    }
    protected override void OnEnable()
    {
        base.OnEnable();
        AttackManager.AttackCooldown -= LevelManager.Instance.CurrentLevelIndex * 0.01f;
        if (AttackManager.AttackCooldown <= 0.1f)
        {
            AttackManager.AttackCooldown = 0.1f;
        }
    }

    public override void BuffedState()
    {
        AttackManager.AttackCooldown = AttackManager.AttackCooldown / 1.5f;
    }
    public override void UnBuffedState()
    {
        base.UnBuffedState();
        AttackManager.AttackCooldown = AttackManager.AttackCooldown * 1.5f;
    }

    protected void OnDisable()
    {

        CancelInvoke("SpawnRandomShips");
    }

    protected override void Attack()
    {
        SpawnRandomShips(_shipsPerSpawn);
    }

    public float SpawnRadius { get => _spawnRadius; set => _spawnRadius = value; }
    public int ShipsPerSpawn { get => _shipsPerSpawn; set => _shipsPerSpawn = value; }
    public List<string> Ships { get => ships; set => ships = value; }


}
