using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ObjectivesUIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI objectivesText;
    [SerializeField] TextMeshProUGUI objectivesPanel;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        foreach (Objective objective in ObjectivesManager.Instance.objectives)
        {
            if (objective.GetIsActive())
            {
                if (objective is DestroyShipsTimed)
                {
                    DestroyShipsTimed destroyShipsTimed = (DestroyShipsTimed)objective;
                    destroyShipsTimed.SetObjectiveDescription("Destroy " + destroyShipsTimed.GetRequiredKills() + " ships in " + Math.Round(destroyShipsTimed.GetElapsedTime(), 0) + " seconds");
                    objectivesText.text = objective.GetObjectiveDescription();
                }
            }
            else if (objective.GetIsCompleted())
            {
                objectivesText.text = "Objective Completed";
            }
            else if (objective.GetIsFailed())
            {
                objectivesText.text = "Objective Failed";
            }
        }

    }
}
