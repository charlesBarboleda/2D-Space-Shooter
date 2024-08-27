using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Unity.VisualScripting;

public class ObjectivesUIManager : MonoBehaviour
{
    public static ObjectivesUIManager Instance;
    [SerializeField] GameObject objectivesDescriptionPrefab;
    [SerializeField] Transform objectivesPanel;
    Dictionary<Objective, TextMeshProUGUI> objectiveUITexts = new Dictionary<Objective, TextMeshProUGUI>();

    // Update is called once per frame
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
    public void InitializeUI(List<Objective> objectives)
    {
        foreach (Objective objective in objectives)
        {
            AddObjectiveUI(objective);
        }
    }

    public void AddObjectiveUI(Objective newObjective)
    {
        GameObject objectiveItem = Instantiate(objectivesDescriptionPrefab, objectivesPanel);
        TextMeshProUGUI objectiveText = objectiveItem.GetComponent<TextMeshProUGUI>();
        objectiveUITexts.Add(newObjective, objectiveText);
        UpdateObjectiveUI(newObjective); // Set initial text
    }

    public void UpdateObjectiveUI(Objective objective)
    {
        if (objectiveUITexts.ContainsKey(objective))
        {
            TextMeshProUGUI objectiveText = objectiveUITexts[objective];
            objectiveText.text = objective.GetObjectiveDescription();
            if (objective.GetIsCompleted()) objectiveText.color = Color.green;
            else if (objective.GetIsFailed()) objectiveText.color = Color.red;
            else objectiveText.color = Color.white;
        }
    }

}
