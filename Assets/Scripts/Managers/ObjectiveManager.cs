using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    public List<Objective> objectives;

    private void Update()
    {
        foreach (Objective objective in objectives)
        {
            if (!objective.isCompleted)
            {
                objective.CheckCompletion();
            }
        }
    }
}
