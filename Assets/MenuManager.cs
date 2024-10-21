using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


public class MenuManager : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Texture2D customCursor;  // Reference to your custom cursor image
    Vector2 _hotSpot = Vector2.zero;  // Hotspot of the cursor (usually the pointy tip)
    CursorMode _cursorMode = CursorMode.Auto;
    [SerializeField] GameObject _mainMenu;
    [SerializeField] GameObject _settingsMenu;
    [SerializeField] GameObject _instructionsMenu;
    [SerializeField] GameObject _playMenu;
    [SerializeField] GameObject _upgradesMenu;

    [SerializeField] GameObject _upgradesButtons;
    Resolution[] resolutions;
    public TMP_Dropdown resolutionDropdown;
    public GameObject dropdownsGameObject;

    public GameObject playButton, instructionsButton, settingsButton, quitButton;

    void Start()
    {

        Cursor.SetCursor(customCursor, _hotSpot, _cursorMode);
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
        StartCoroutine(FadeInMainMenu());
    }

    public void BackButton()
    {
        _settingsMenu.SetActive(false);
        dropdownsGameObject.SetActive(false);
        _mainMenu.SetActive(true);
    }
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }


    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    IEnumerator FadeInMainMenu()
    {
        yield return new WaitForSeconds(0.5f);

        playButton.GetComponent<CanvasGroup>().DOFade(1, 2f);
        yield return new WaitForSeconds(0.25f);
        instructionsButton.GetComponent<CanvasGroup>().DOFade(1, 2f);
        yield return new WaitForSeconds(0.25f);

        settingsButton.GetComponent<CanvasGroup>().DOFade(1, 2f);
        yield return new WaitForSeconds(0.25f);

        quitButton.GetComponent<CanvasGroup>().DOFade(1, 2f);
    }

    public void EnableUpgradesMenu()
    {

        _playMenu.SetActive(false);
        _upgradesButtons.SetActive(true);
        _upgradesMenu.SetActive(true);
    }
    public void EnableSettingsMenu()
    {
        _mainMenu.SetActive(false);
        _settingsMenu.SetActive(true);
        dropdownsGameObject.SetActive(true);
    }
    public void EnableInstructionsMenu()
    {
        _mainMenu.SetActive(false);
        _instructionsMenu.SetActive(true);
    }

    public void EnableMainMenu()
    {
        _playMenu.SetActive(false);
        _settingsMenu.SetActive(false);
        _instructionsMenu.SetActive(false);
        _mainMenu.SetActive(true);
    }
    public void EnablePlayMenu()
    {
        _mainMenu.SetActive(false);
        _playMenu.SetActive(true);
    }


    public void QuitGame()
    {
        Application.Quit();
    }

    public void PressStartGame()
    {
        _playMenu.SetActive(false);
    }

}
