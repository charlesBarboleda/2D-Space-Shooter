using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleetFormation : MonoBehaviour
{
    [SerializeField] List<Transform> _formationPositions = new List<Transform>();
    [SerializeField] List<Transform> _bossPositions = new List<Transform>();
    void Awake()
    {
        foreach (Transform child in transform)
        {
            // If the child is a boss spawn point, add it to the boss positions list otherwise add it to the formation positions list
            if (child.CompareTag("BossSpawnPoint")) _bossPositions.Add(child);
            else _formationPositions.Add(child);
        }
    }

    public int TotalPositions => _formationPositions.Count + _bossPositions.Count;

    public List<Transform> FormationPositions { get => _formationPositions; set => _formationPositions = value; }
    public List<Transform> BossPositions { get => _bossPositions; set => _bossPositions = value; }


}
