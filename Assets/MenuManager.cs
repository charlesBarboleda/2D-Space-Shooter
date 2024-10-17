using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject _mainMenu;
    [SerializeField] GameObject _settingsMenu;
    [SerializeField] GameObject _instructionsMenu;

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
        _settingsMenu.SetActive(false);
        _instructionsMenu.SetActive(false);
        _mainMenu.SetActive(true);
    }

}
