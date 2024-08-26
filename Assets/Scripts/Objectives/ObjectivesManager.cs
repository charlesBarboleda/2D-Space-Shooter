using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectivesManager : MonoBehaviour
{
    public static ObjectivesManager Instance;
    public List<Objective> objectives = new List<Objective>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        foreach (Objective objective in objectives)
        {
            objective.InitObjective();
        }
    }

    void Update()
    {
        foreach (Objective objective in objectives)
        {
            if (objective.GetIsActive())
            {
                objective.UpdateObjective();
            }
        }
    }

    public void DestroyShip()
    {
        foreach (Objective objective in objectives)
        {
            if (objective is DestroyShipsTimed)
            {
                DestroyShipsTimed destroyShipsTimed = (DestroyShipsTimed)objective;
                destroyShipsTimed.SetCurrentKills(destroyShipsTimed.GetCurrentKills() + 1);
            }
        }
    }
}
