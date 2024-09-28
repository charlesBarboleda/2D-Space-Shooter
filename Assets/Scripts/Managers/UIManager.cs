using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public Canvas worldCanvas;
    public TextMeshProUGUI midScreenText;

    [Header("Upgrade Shop")]
    [SerializeField] GameObject upgradeShopPanel;
    [SerializeField] TextMeshProUGUI healthUpgradeText, damageUpgradeText, fireRateUpgradeText, bulletSpeedUpgradeText, extraBulletUpgradeText, speedUpgradeText, pickUpUpgradeText;
    [SerializeField] TextMeshProUGUI healthCost, damageCost, fireRateCost, bulletSpeedCost, extraBulletCost, speedCost, pickUpCost;

    [Header("Game UI Elements")]
    [SerializeField] TextMeshProUGUI roundText;
    [SerializeField] TextMeshProUGUI highscoreText;

    [SerializeField] Image currencyIcon;
    [SerializeField] TextMeshProUGUI currencyText;
    [SerializeField] GameObject _pauseMenu;

    [Header("Player Abilities UI")]
    [SerializeField] Image laserIconFill, shieldIconFill, teleportIconFill, turretIconFill;
    public GameObject laserPanel, shieldPanel, teleportPanel, turretPanel;



    AbilityHolder _abilityHolder;

    [Header("PowerUps Panel")]
    public GameObject DamagePowerUp;
    public GameObject FireRatePowerUp;
    public GameObject SpeedPowerUp;
    public GameObject HealthPowerUp;
    public GameObject PickUpRadiusPowerUp;

    [Header("UI Panels")]
    public GameObject skillTreePanel;
    public GameObject bossHealthBar;
    public GameObject gameOverPanel;
    public GameObject miniMapContainer;
    public GameObject playerHealthBar;
    public GameObject objectivesPanel;
    public GameObject powerUpsPanel;
    public GameObject currencyPanel;
    public GameObject roundNumber;


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

        _abilityHolder = PlayerManager.GetInstance().AbilityHolder();

    }
    void OnEnable()

    {
        EventManager.OnCurrencyChange += UpdateCurrencyText;
        EventManager.OnNextLevel += UpdateRoundText;
        EventManager.OnGameOver += GameOver;
        EventManager.OnGameOver += UpdateHighScoreUI;

    }

    void OnDisable()
    {
        EventManager.OnCurrencyChange -= UpdateCurrencyText;
        EventManager.OnNextLevel -= UpdateRoundText;
        EventManager.OnGameOver -= GameOver;
        EventManager.OnGameOver -= UpdateHighScoreUI;
    }

    void UpdateHighScoreUI()
    {
        highscoreText.text = $"Highscore: Level {PlayerPrefs.GetFloat("HighScore")}";
    }

    void Update()
    {
        UpdateRoundText();
        PauseMenu();
        if (upgradeShopPanel.activeSelf)
        {
            UpdateAllUpgradeText();
        }



        // Update the ability icons fill amount based on the cooldown
        foreach (Ability ability in _abilityHolder.abilities)
        {
            if (ability.isUnlocked)
            {
                if (ability is AbilityLaser)
                {
                    laserIconFill.fillAmount = ability.currentCooldown / ability.cooldown;
                }
                if (ability is AbilityShield)
                {
                    shieldIconFill.fillAmount = ability.currentCooldown / ability.cooldown;
                }
                if (ability is AbilityTeleport)
                {
                    teleportIconFill.fillAmount = ability.currentCooldown / ability.cooldown;
                }
                if (ability is AbilityTurrets)
                {
                    turretIconFill.fillAmount = ability.currentCooldown / ability.cooldown;
                }
            }
        }

        // Set the currency icon position based on the currency text width
        currencyIcon.rectTransform.anchoredPosition = new Vector2(currencyText.preferredWidth + 130, currencyIcon.rectTransform.anchoredPosition.y);
    }

    void PauseMenu()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_pauseMenu.activeSelf)
            {
                _pauseMenu.SetActive(false);
                Time.timeScale = 1;
            }

            else
            {
                Time.timeScale = 0;
                _pauseMenu.SetActive(true);
            }

        }
    }
    public void DeactivateAllUIPanels()
    {
        Debug.Log("Deactivating all UI panels");
        skillTreePanel.SetActive(false);
        upgradeShopPanel.SetActive(false);
        playerHealthBar.SetActive(false);
        objectivesPanel.SetActive(false);
        powerUpsPanel.SetActive(false);
        currencyPanel.SetActive(false);
        roundNumber.SetActive(false);
        bossHealthBar.SetActive(false);
        miniMapContainer.SetActive(false);
        Debug.Log("Deactivated all UI panels");
    }

    public void ActivateAllUIPanels()
    {

        playerHealthBar.SetActive(true);
        objectivesPanel.SetActive(true);
        powerUpsPanel.SetActive(true);
        currencyPanel.SetActive(true);
        roundNumber.SetActive(true);
        miniMapContainer.SetActive(true);
    }

    public void UnPauseButton()
    {
        Time.timeScale = 1;
        _pauseMenu.SetActive(false);
    }

    // Methods for the Upgrade
    void UpdateAllUpgradeText()
    {
        // Updates the description text and cost text for each upgrade

        SetDescriptionText(healthUpgradeText, UpgradeShopManager.healthUpgrade);
        SetDescriptionText(damageUpgradeText, UpgradeShopManager.bulletDamageUpgrade);
        SetDescriptionText(fireRateUpgradeText, UpgradeShopManager.fireRateUpgrade);
        SetDescriptionText(bulletSpeedUpgradeText, UpgradeShopManager.bulletSpeedUpgrade);
        SetDescriptionText(extraBulletUpgradeText, UpgradeShopManager.extraBulletUpgrade);
        SetDescriptionText(speedUpgradeText, UpgradeShopManager.shipSpeedUpgrade);
        SetDescriptionText(pickUpUpgradeText, UpgradeShopManager.pickUpUpgrade);



        SetCostText(healthCost, UpgradeShopManager.healthUpgrade);
        SetCostText(damageCost, UpgradeShopManager.bulletDamageUpgrade);
        SetCostText(fireRateCost, UpgradeShopManager.fireRateUpgrade);
        SetCostText(bulletSpeedCost, UpgradeShopManager.bulletSpeedUpgrade);
        SetCostText(extraBulletCost, UpgradeShopManager.extraBulletUpgrade);
        SetCostText(speedCost, UpgradeShopManager.shipSpeedUpgrade);
        SetCostText(pickUpCost, UpgradeShopManager.pickUpUpgrade);
    }

    public void MidScreenWarningText(string text, float duration)
    {
        StartCoroutine(OnMidScreenText(text, duration));
    }
    IEnumerator OnMidScreenText(string text, float duration)
    {
        midScreenText.text = text;
        midScreenText.gameObject.SetActive(true);

        float t = 0;

        while (t < duration)
        {
            // Use PingPong to create a continuous fade-in and fade-out effect
            float fade = Mathf.PingPong(Time.time * 3, 1); // Adjust the multiplier (2) for faster or slower fading
            midScreenText.color = new Color(1, 1, 1, fade);

            t += Time.deltaTime;
            yield return null;
        }

        midScreenText.gameObject.SetActive(false);
    }


    public IEnumerator OnHitDamageText(string text, Vector3 position)
    {
        // Get screen position from world position
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(position);

        // Spawn damage text and convert its position to canvas space
        GameObject damageText = ObjectPooler.Instance.SpawnFromPool("DamageText", transform.position, Quaternion.identity);

        // Set the parent to the world canvas and convert the screen point to the canvas's world position
        damageText.transform.SetParent(worldCanvas.transform, false);

        // Convert screen position to local position in the canvas
        RectTransform canvasRect = worldCanvas.GetComponent<RectTransform>();
        RectTransform damageTextRect = damageText.GetComponent<RectTransform>();

        Vector2 localPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPosition, Camera.main, out localPosition);
        damageTextRect.localPosition = localPosition;


        // Set damage text content
        damageText.GetComponent<TextMeshProUGUI>().text = text;
        yield return StartCoroutine(MoveUpAndFadeOut(damageText, 1f));
        damageText.SetActive(false);
    }

    public void CreateOnHitDamageText(string text, Vector3 position)
    {
        StartCoroutine(OnHitDamageText(text, position));
    }
    public IEnumerator MoveUpAndFadeOut(GameObject damageTextObject, float duration)
    {
        float time = 0;
        while (time < duration)
        {
            time += Time.deltaTime;
            damageTextObject.transform.position += Vector3.up * Time.deltaTime * 5f;
            damageTextObject.GetComponent<TextMeshProUGUI>().color = new Color(1, 1, 1, 1 - time / duration);
            yield return null;
        }
        damageTextObject.SetActive(false);
    }


    public void DisableAllUIPanels()
    {
        upgradeShopPanel.SetActive(false);
        skillTreePanel.SetActive(false);
    }

    private void UpdateCurrencyText(float newCurrency)
    {
        currencyText.text = $"{newCurrency}";
    }

    private void UpdateRoundText()
    {
        if (GameManager.Instance.CurrentGameState == GameManager.GameState.LevelIn) roundText.text = $"{LevelManager.Instance.CurrentLevelIndex}";
        if (GameManager.Instance.CurrentGameState == GameManager.GameState.Countdown) roundText.text = $"{Math.Round(GameManager.Instance.RoundCountdown, 0)}";
    }

    private void SetDescriptionText(TextMeshProUGUI text, Upgrade upgrade)
    {
        text.text = upgrade.upgradeDescription;
    }

    private void SetCostText(TextMeshProUGUI text, Upgrade upgrade)
    {
        text.text = $"{upgrade.upgradeCost}";
    }


    public void GameOver()
    {
        gameOverPanel.SetActive(true);
    }

    public void ExitSkillTree()
    {
        skillTreePanel.SetActive(false);
    }

    public void OpenSkillTree()
    {
        skillTreePanel.SetActive(true);
    }
}
