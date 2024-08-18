using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] TextMeshProUGUI roundText;
    [SerializeField] TextMeshProUGUI highscoreText;
    [SerializeField] TextMeshProUGUI currencyText;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] GameObject objectiveTemplate;
    [SerializeField] Transform objectivesContainer;

    private Dictionary<Objective, GameObject> objectiveUIElements = new Dictionary<Objective, GameObject>();

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

    void Start()
    {
        EventManager.OnNextRound += UpdateRoundText;
        EventManager.OnGameOver += GameOver;
        PlayerManager.OnCurrencyChange += UpdateCurrencyText;
        UpdateRoundText();
        InitializeObjectiveUI();
    }

    void OnDestroy()
    {
        EventManager.OnNextRound -= UpdateRoundText;
        EventManager.OnGameOver -= GameOver;
        PlayerManager.OnCurrencyChange -= UpdateCurrencyText;
    }

    void Update()
    {
        UpdateObjectiveStatus();

    }

    private void UpdateCurrencyText()
    {
        currencyText.text = $"{GameManager.Instance.GetPlayer().currency}";
    }

    private void UpdateRoundText()
    {
        roundText.text = $"{GameManager.Instance.level}";
    }

    public void GameOver()
    {
        gameOverPanel.SetActive(true);
    }

    private void InitializeObjectiveUI()
    {

        objectiveUIElements.Clear();

        foreach (Objective objective in GameManager.Instance.GetCurrentObjectives())
        {
            GameObject objectiveGO = Instantiate(objectiveTemplate, objectivesContainer);
            objectiveUIElements[objective] = objectiveGO;
        }
    }

    private void UpdateObjectiveStatus()
    {
        foreach (var objectivePair in objectiveUIElements)
        {
            Objective objective = objectivePair.Key;
            GameObject objectiveGO = objectivePair.Value;
            TextMeshProUGUI objectiveText = objectiveGO.GetComponentInChildren<TextMeshProUGUI>();
            if (objective is DestroyShipsObjective destroyShipsObjective)
            {
                objectiveText.text = $"Destroy {destroyShipsObjective.GetRemainingShips()} ships";
            }
            else if (objective is DestroyShipsTimerObjective destroyShipsTimerObjective)
            {
                objectiveText.text = $"Destroy {destroyShipsTimerObjective.GetRemainingShips()} ships in {Math.Round(destroyShipsTimerObjective.GetRemainingTime(), 0)} s";
            }

            if (objective.IsCompleted)
            {
                objectiveText.color = Color.green;  // Change color to green when completed
                objectiveText.text = $"✔ {objective.Description}";
            }
        }
    }
}
