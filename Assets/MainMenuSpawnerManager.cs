using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MainMenuSpawnerManager : MonoBehaviour
{
    public List<Transform> offenceSpawns = new List<Transform>();
    public List<Transform> defenceSpawns = new List<Transform>();
    public List<GameObject> offenceShips = new List<GameObject>();
    public List<GameObject> defenceShips = new List<GameObject>();
    GameObject ship;


    void Start()
    {
        StartCoroutine(SpawnEnemiesOverTime());
    }
    void Update()
    {
        ship.GetComponent<Health>().CurrentHealth = 1;
        ship.GetComponent<AttackManager>().AimRange = 20;
        ship.GetComponent<NavMeshAgent>().stoppingDistance = 20;
        ship.GetComponent<NavMeshAgent>().speed = 5;
    }

    IEnumerator SpawnEnemiesOverTime()
    {
        float t = 0;
        while (t < Mathf.Infinity)
        {
            SpawnOffenceEnemy();
            SpawnDefenceEnemy();
            yield return new WaitForSeconds(3f);
        }

    }

    void SpawnOffenceEnemy()
    {
        ship = Instantiate(offenceShips[Random.Range(0, offenceShips.Count)],
                    offenceSpawns[Random.Range(0, offenceSpawns.Count)].position,
                    Quaternion.identity);

    }
    void SpawnDefenceEnemy()
    {
        ship = Instantiate(defenceShips[Random.Range(0, defenceShips.Count)],
                    defenceSpawns[Random.Range(0, defenceSpawns.Count)].position,
                    Quaternion.identity);


        ship.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

    }
}
