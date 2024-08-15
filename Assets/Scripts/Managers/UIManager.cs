using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI roundText;
    [SerializeField] private TextMeshProUGUI laserText;
    [SerializeField] private TextMeshProUGUI shieldText;
    [SerializeField] private TextMeshProUGUI turretText;
    [SerializeField] private TextMeshProUGUI highscoreText;
    [SerializeField] private TextMeshProUGUI currencyText;

    [SerializeField] private GameObject gameOverPanel;

    void Start()
    {
        GameManager.OnNextRound += UpdateRoundText;
        GameManager.OnGameOver += GameOver;
        PlayerManager.OnCurrencyChange += UpdateCurrencyText;
    }

    void OnDestroy()
    {
        GameManager.OnNextRound -= UpdateRoundText;
        GameManager.OnGameOver -= GameOver;
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
