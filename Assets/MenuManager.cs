using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject _mainMenu;
    [SerializeField] GameObject _settingsMenu;
    [SerializeField] GameObject _instructionsMenu;
    [SerializeField] GameObject _playMenu;
    [SerializeField] GameObject _upgradesMenu;
    public void EnableUpgradesMenu()
    {
        _playMenu.SetActive(false);
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
