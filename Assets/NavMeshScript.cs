using System.Collections;
using System.Collections.Generic;
using NavMeshPlus.Components;
using UnityEngine;

public class NavMeshScript : MonoBehaviour
{
    public static NavMeshScript Instance { get; private set; }
    public NavMeshSurface navMeshSurface;

    void Start()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);

        }
        else
        {
            Instance = this;
        }

        navMeshSurface.BuildNavMeshAsync();
    }

    // Call this method to update the NavMesh
    public void UpdateNavMesh()
    {
        navMeshSurface.UpdateNavMesh(navMeshSurface.navMeshData);
    }


}
