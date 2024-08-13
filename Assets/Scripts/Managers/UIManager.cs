using System;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private PlayerManager player;
    [SerializeField] private TextMeshProUGUI roundText;
    [SerializeField] private TextMeshProUGUI laserText;
    [SerializeField] private TextMeshProUGUI shieldText;
    [SerializeField] private TextMeshProUGUI turretText;
    [SerializeField] private TextMeshProUGUI highscore;
    [SerializeField] private GameObject gameOverPanel;

    void Start()
    {
        player = GameManager.Instance.GetPlayer();
        GameManager.OnNextRound += UpdateRoundText;
        GameManager.OnGameOver += GameOver;
        GameManager.OnBossRound += UpdateBossRoundText;
        GameManager.OnPowerUpRound += UpdatePowerUpRoundText;
        GameManager.OnPowerUpChosen += UpdateRoundText;
    }

    void OnDestroy()
    {
        GameManager.OnNextRound -= UpdateRoundText;
        GameManager.OnGameOver -= GameOver;
        GameManager.OnBossRound -= UpdateBossRoundText;
        GameManager.OnPowerUpRound -= UpdatePowerUpRoundText;
        GameManager.OnPowerUpChosen -= UpdateRoundText;



    }

    void Update()
    {

    }

    private void UpdateRoundText()
    {

        roundText.text = $"Round: {GameManager.Instance.level}";
    }
    private void UpdatePowerUpRoundText()
    {
        roundText.text = "Power Up Round";
    }

    private void UpdateBossRoundText()
    {
        roundText.text = "Boss Round";
    }


    public void GameOver()
    {
        gameOverPanel.SetActive(true);

    }
}
