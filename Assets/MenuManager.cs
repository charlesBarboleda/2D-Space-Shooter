using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject _mainMenu;
    [SerializeField] GameObject _settingsMenu;
    [SerializeField] GameObject _instructionsMenu;
    [SerializeField] GameObject _playMenu;
    [SerializeField] GameObject _upgradesMenu;
    [SerializeField] GameObject _upgradesButtons;

    public GameObject playButton, instructionsButton, settingsButton, quitButton;

    void Start()
    {
        StartCoroutine(FadeInMainMenu());

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
