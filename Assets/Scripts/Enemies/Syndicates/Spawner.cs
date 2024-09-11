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
    [SerializeField] float _spawnRate = 3f;
    public List<String> ships = new List<string>();

    protected override void Start()
    {
        base.Start();
        Attack();
    }
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
        _spawnRate -= GameManager.Instance.Level * 0.01f;
        if (_spawnRate <= 0.1f)
        {
            _spawnRate = 0.1f;
        }
    }

    public override void BuffedState()
    {
        _spawnRate = _spawnRate / 1.5f;
    }
    public override void UnBuffedState()
    {
        base.UnBuffedState();
        _spawnRate = _spawnRate * 1.5f;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        CancelInvoke("SpawnRandomShips");
    }

    protected override void Attack()
    {
        InvokeRepeating("SpawnRandomShips", 0, _spawnRate);
    }

    public void SetSpawnRadius(float radius)
    {
        _spawnRadius = radius;
    }
    public void SetSpawnRate(float rate)
    {
        _spawnRate = rate;
    }
    public float GetSpawnRate()
    {
        return _spawnRate;
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
