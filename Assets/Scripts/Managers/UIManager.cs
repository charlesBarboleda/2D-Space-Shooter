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
        if (player.laser.laserStock > 0) laserText.text = $": x{player.laser.laserStock}";
        else laserText.text = ": x0";

        if (player.playerShield.shieldTimer > 0) shieldText.text = $": {player.playerShield.shieldTimer}s";
        else shieldText.text = ": 0s";

        if (player.turret.turretCountDownTime > 0) turretText.text = $": {player.turret.turretCountDownTime}s";
        else turretText.text = ": 0s";

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
