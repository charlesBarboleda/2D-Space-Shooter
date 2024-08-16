using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    [SerializeField] TextMeshProUGUI roundText; [SerializeField] TextMeshProUGUI highscoreText;
    [SerializeField] TextMeshProUGUI currencyText;

    [SerializeField] GameObject gameOverPanel;
    [SerializeField] GameObject objectiveTemplate;
    [SerializeField] Transform objectivesContainer;


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
    }



    void OnDestroy()
    {
        EventManager.OnNextRound -= UpdateRoundText;
        EventManager.OnGameOver -= GameOver;
        PlayerManager.OnCurrencyChange -= UpdateCurrencyText;

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
    public void UpdateObjectivesUI(List<Objective> objectives)
    {
        foreach (Transform child in objectivesContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (Objective objective in objectives)
        {
            GameObject objUI = Instantiate(objectiveTemplate, objectivesContainer);
            TextMeshProUGUI descriptionText = objUI.GetComponentInChildren<TextMeshProUGUI>();
            Toggle checkMark = objUI.GetComponentInChildren<Toggle>();

            if (objective is DestroyShipsObjective destroyObjective)
            {
                descriptionText.text = $"Destroy {destroyObjective.targetAmount - destroyObjective.currentAmount} ships";
            }
            else
            {
                descriptionText.text = objective.description; // Or any other fallback description
            }
            checkMark.isOn = objective.isCompleted;

            checkMark.onValueChanged.AddListener((isOn) =>
            {
                objective.isCompleted = isOn;
            });
        }
    }
}
