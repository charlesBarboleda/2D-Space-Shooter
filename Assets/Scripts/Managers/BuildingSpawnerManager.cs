using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSpawnerManager : MonoBehaviour
{
    [SerializeField] GameObject skillTreeBuildingPrefab;
    [SerializeField] GameObject upgradeBuildingPrefab;
    [SerializeField] List<Transform> skillTreeBuildingLocations;
    [SerializeField] List<Transform> upgradeBuildingLocations;

    void Start()
    {
        GameObject skillTreeBuilding = Instantiate(skillTreeBuildingPrefab, skillTreeBuildingLocations[Random.Range(0, skillTreeBuildingLocations.Count)].position, Quaternion.identity);
        GameObject upgradeBuilding = Instantiate(upgradeBuildingPrefab, upgradeBuildingLocations[Random.Range(0, upgradeBuildingLocations.Count)].position, Quaternion.identity);
    }



}
