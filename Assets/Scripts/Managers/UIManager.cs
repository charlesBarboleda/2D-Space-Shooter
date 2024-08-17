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

}
