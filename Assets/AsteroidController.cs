using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidController : MonoBehaviour
{
    void OnDisable()
    {
        NavMeshScript.Instance.UpdateNavMesh();
    }
}
