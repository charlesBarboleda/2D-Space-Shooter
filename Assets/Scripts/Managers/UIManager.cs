using System;
using System.Collections;
using System.Collections.Generic;
using Mkey;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    [Header("Ultimate Animations")]
    [SerializeField] Material _revealMaterial;
    [SerializeField] Image _cameraCrack;
    [SerializeField] Image _whiteFlash;
    [SerializeField] ParticleSystem _shatterParticles;


    [Header("Combo System")]
    [SerializeField] TextMeshProUGUI _comboText;
    [SerializeField] GameObject _comboHotkey;
    int _lastComboCount = 0;


    [Header("Objectives")]
    [SerializeField] GameObject objectiveDescriptionText;
    [SerializeField] GameObject objectivePanel;

    // A list to keep track of instantiated objective UI elements
    Dictionary<ObjectiveBase, TextMeshProUGUI> _objectiveUIElements = new Dictionary<ObjectiveBase, TextMeshProUGUI>();

    [Header("Upgrade Shop")]
    [SerializeField] GameObject upgradeShopPanel;
    [SerializeField] TextMeshProUGUI healthUpgradeText, damageUpgradeText, fireRateUpgradeText, bulletSpeedUpgradeText, extraBulletUpgradeText, speedUpgradeText, pickUpUpgradeText;
    [SerializeField] TextMeshProUGUI healthCost, damageCost, fireRateCost, bulletSpeedCost, extraBulletCost, speedCost, pickUpCost;

    [Header("Game UI Elements")]
    public Canvas worldCanvas;
    public TextMeshProUGUI midScreenText;
    public TextMeshProUGUI countdownText;
    [SerializeField] TextMeshProUGUI roundText;
    [SerializeField] TextMeshProUGUI highscoreText;

    [SerializeField] Image currencyIcon;
    [SerializeField] TextMeshProUGUI currencyText;
    [SerializeField] GameObject _pauseMenu;

    [Header("Player Abilities UI")]
    [SerializeField] Image laserIconFill, shieldIconFill, teleportIconFill, turretIconFill;
    [SerializeField] Image laserUltIconFill, shieldUltIconFill, teleportUltIconFill, turretUltIconFill;

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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ActivateCrackAndShatter();
        }
        PulseAbilityIcon();
        UpdateCountdownText();
        UpdateRoundText();
        UpdateComboText();
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
            if (ability.isUltimateUnlocked)
            {
                if (ability is AbilityLaser)
                {
                    laserUltIconFill.fillAmount = ability.currentUltimateCooldown / ability.ultimateCooldown;
                }
                if (ability is AbilityShield)
                {
                    shieldUltIconFill.fillAmount = ability.currentUltimateCooldown / ability.ultimateCooldown;
                }
                if (ability is AbilityTeleport)
                {
                    teleportUltIconFill.fillAmount = ability.currentUltimateCooldown / ability.ultimateCooldown;
                }
                if (ability is AbilityTurrets)
                {
                    turretUltIconFill.fillAmount = ability.currentUltimateCooldown / ability.ultimateCooldown;
                }
            }
        }

        // Set the currency icon position based on the currency text width
        currencyIcon.rectTransform.anchoredPosition = new Vector2(currencyText.preferredWidth + 130, currencyIcon.rectTransform.anchoredPosition.y);
    }
    void PulseAbilityIcon()
    {
        foreach (Ability ability in _abilityHolder.abilities)
        {
            if (ability.isUltimateReady)
            {
                StartCoroutine(PulseIcon(ability));
            }
        }
    }

    IEnumerator PulseIcon(Ability ability)
    {
        float t = 0;
        while (ability.isUltimateReady)
        {
            t += Time.deltaTime;
            if (ability is AbilityLaser)
            {
                laserUltIconFill.transform.localScale = new Vector3(1 + Mathf.PingPong(Time.time, 0.1f), 1 + Mathf.PingPong(Time.time, 0.1f), 1);
            }
            else
            if (ability is AbilityShield)
            {
                shieldUltIconFill.transform.localScale = new Vector3(1 + Mathf.PingPong(Time.time, 0.1f), 1 + Mathf.PingPong(Time.time, 0.1f), 1);
            }
            else
            if (ability is AbilityTeleport)
            {
                teleportUltIconFill.transform.localScale = new Vector3(1 + Mathf.PingPong(Time.time, 0.1f), 1 + Mathf.PingPong(Time.time, 0.1f), 1);
            }
            else
            if (ability is AbilityTurrets)
            {
                turretUltIconFill.transform.localScale = new Vector3(1 + Mathf.PingPong(Time.time, 0.1f), 1 + Mathf.PingPong(Time.time, 0.1f), 1);
            }
            yield return null;
        }
    }

    public void ActivateCrackAndShatter()
    {
        _cameraCrack.gameObject.SetActive(true);
        StartCoroutine(RevealAndShatter());
    }

    IEnumerator RevealAndShatter()
    {
        _revealMaterial.SetFloat("_Cutoff", 0f);
        CameraFollowBehaviour.Instance.ShakePlayerCamera(10f, 5f, 1.3f);
        StartCoroutine(AnimateReveal(0.5f));
        yield return new WaitForSeconds(1f);
        CameraFollowBehaviour.Instance.IncreasePlayerOrthographicSize(75f, 6f);
        StartCoroutine(Shatter());
    }

    IEnumerator AnimateReveal(float revealDuration)
    {

        float t = 0f;
        while (t < revealDuration)
        {
            t += Time.deltaTime;
            float _cutoffValue = Mathf.Lerp(0f, 1f, t / revealDuration);  // Lerp from 0 to 1
            _revealMaterial.SetFloat("_Cutoff", _cutoffValue);  // Set the new cutoff value
            yield return null;
        }
    }

    IEnumerator Shatter()
    {
        StartCoroutine(FlashWhite(0.1f));
        yield return new WaitForSeconds(0.2f);
        _shatterParticles.Play();
        _cameraCrack.gameObject.SetActive(false);
    }

    IEnumerator FlashWhite(float duration)
    {
        _whiteFlash.gameObject.SetActive(true);
        yield return new WaitForSeconds(duration);
        float t = 0;
        while (t < 0.5f)
        {
            t += Time.deltaTime;
            float fade = Mathf.Lerp(1, 0, t / 0.5f);
            _whiteFlash.color = new Color(1, 1, 1, fade);
            yield return null;
        }
        _whiteFlash.gameObject.SetActive(false);
    }



    public void ActivateComboKey()
    {
        _comboHotkey.SetActive(true);
    }

    public void DeactivateComboKey()
    {
        _comboHotkey.SetActive(false);
    }

    void UpdateComboText()
    {
        int currentComboCount = ComboManager.Instance.comboCount;

        // Update combo text only if combo count has changed
        if (currentComboCount != _lastComboCount)
        {
            if (currentComboCount == 0)
            {
                _comboText.gameObject.SetActive(false);
            }
            else
            {
                _comboText.gameObject.SetActive(true);
                _comboText.text = $"{currentComboCount}";

                // Start the pop animation when the combo count changes
                StartCoroutine(PopTextAnimation(_comboText));
            }

            _lastComboCount = currentComboCount;  // Update the last combo count
        }
    }

    // Coroutine to animate the pop effect on the text
    IEnumerator PopTextAnimation(TextMeshProUGUI text)
    {
        Vector3 originalScale = Vector3.one; // Reset to (1, 1, 1) scale
        Vector3 popScale = originalScale * 1.5f;  // Scale up by 50%
        float animationDuration = 0.2f;  // Duration of pop animation

        // Animate scaling up
        float elapsedTime = 0f;
        while (elapsedTime < animationDuration)
        {
            text.transform.localScale = Vector3.Lerp(originalScale, popScale, elapsedTime / animationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        text.transform.localScale = popScale; // Ensure it's fully scaled up

        // Animate scaling back down
        elapsedTime = 0f;
        while (elapsedTime < animationDuration)
        {
            text.transform.localScale = Vector3.Lerp(popScale, originalScale, elapsedTime / animationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Reset back to original scale after the animation
        text.transform.localScale = originalScale;
    }


    // --------------- Objective System Integration ---------------

    /// <summary>
    /// Adds an objective to the UI panel.
    /// </summary>
    /// <param name="objective">The objective to display in the UI.</param>
    public void AddObjectiveToUI(ObjectiveBase objective)
    {
        // Create a new UI element for the objective
        GameObject objectiveDescription = Instantiate(objectiveDescriptionText, objectivePanel.transform);
        TextMeshProUGUI textComponent = objectiveDescription.GetComponent<TextMeshProUGUI>();

        // Set the initial text
        textComponent.text = $"{objective.objectiveDescription}";

        // Map the objective to its UI element
        _objectiveUIElements[objective] = textComponent;

        Debug.Log("Instantiated objective description UI for " + objective.objectiveName);
    }

    /// <summary>
    /// Updates the description of a specific objective in the UI.
    /// </summary>
    /// <param name="objective">The objective to update.</param>
    public void UpdateObjectiveUI(ObjectiveBase objective)
    {
        Debug.Log("Update Objective UI called from UIManager");

        // Check if the objective is already mapped to a UI element
        if (_objectiveUIElements.TryGetValue(objective, out TextMeshProUGUI textComponent))
        {
            // Update the text to the latest description
            textComponent.text = $"{objective.objectiveDescription}";
        }
        else
        {
            Debug.LogError("Failed to find the UI element for the objective: " + objective.objectiveName);
        }
    }

    /// <summary>
    /// Marks an objective as complete in the UI.
    /// </summary>
    /// <param name="objective">The completed objective.</param>
    public void MarkObjectiveAsComplete(ObjectiveBase objective)
    {
        if (_objectiveUIElements.TryGetValue(objective, out TextMeshProUGUI textComponent))
        {
            textComponent.text += " [COMPLETED]";
            textComponent.color = Color.green;  // Change the color to indicate completion
            Debug.Log("Objective marked as complete in UI");
        }
    }

    public void MarkObjectiveAsFailed(ObjectiveBase objective)
    {
        if (_objectiveUIElements.TryGetValue(objective, out TextMeshProUGUI textComponent))
        {
            textComponent.text += " [FAILED]";
            textComponent.color = Color.red;  // Change the color to indicate failure
            Debug.Log("Objective marked as failed in UI");
        }
    }

    /// <summary>
    /// Clears all objective UI elements from the screen.
    /// </summary>
    public void ClearObjectivesFromUI()
    {
        foreach (var uiElement in _objectiveUIElements.Values)
        {
            Destroy(uiElement.gameObject); // Destroy the UI GameObjects
        }
        _objectiveUIElements.Clear(); // Clear the dictionary
        Debug.Log("Cleared all objective UI elements.");
    }

    void UpdateHighScoreUI()
    {
        highscoreText.text = $"Highscore: Level {PlayerPrefs.GetFloat("HighScore")}";
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
        skillTreePanel.SetActive(false);
        upgradeShopPanel.SetActive(false);
        playerHealthBar.SetActive(false);
        objectivesPanel.SetActive(false);
        powerUpsPanel.SetActive(false);
        currencyPanel.SetActive(false);
        roundNumber.SetActive(false);
        bossHealthBar.SetActive(false);
        miniMapContainer.SetActive(false);
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

    void UpdateAllUpgradeText()
    {
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
            float fade = Mathf.PingPong(Time.time * 3, 1); // Fading effect
            midScreenText.color = new Color(1, 1, 1, fade);

            t += Time.deltaTime;
            yield return null;
        }

        midScreenText.gameObject.SetActive(false);
    }

    public IEnumerator OnHitDamageText(string text, Vector3 position)
    {
        // Convert the world position of the damage to screen space
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(position);

        // Check if the object is within the camera's view
        if (screenPosition.z > 0 && IsWithinCameraView(position))
        {
            GameObject damageText = ObjectPooler.Instance.SpawnFromPool("DamageText", transform.position, Quaternion.identity);
            damageText.transform.SetParent(worldCanvas.transform, false);

            RectTransform canvasRect = worldCanvas.GetComponent<RectTransform>();
            RectTransform damageTextRect = damageText.GetComponent<RectTransform>();

            Vector2 localPosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPosition, Camera.main, out localPosition);
            damageTextRect.localPosition = localPosition;

            damageText.GetComponent<TextMeshProUGUI>().text = text;
            yield return StartCoroutine(MoveUpAndFadeOut(damageText, 1f));
            damageText.SetActive(false);
        }
        else
        {
            yield break; // Exit the coroutine if not in camera view
        }
    }

    // Helper method to check if the position is within the camera's view
    bool IsWithinCameraView(Vector3 worldPosition)
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
        Bounds bounds = new Bounds(worldPosition, Vector3.one); // Check with a small bounding box

        return GeometryUtility.TestPlanesAABB(planes, bounds);
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
        if (GameManager.Instance.CurrentGameState == GameManager.GameState.LevelIn)
            roundText.text = $"{LevelManager.Instance.CurrentLevelIndex}";
    }

    void UpdateCountdownText()
    {
        countdownText.text = $"{Mathf.Round(GameManager.Instance.RoundCountdown)}";
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
