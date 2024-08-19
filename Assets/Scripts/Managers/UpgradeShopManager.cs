using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeShopManager : MonoBehaviour
{
    public static UpgradeShopManager Instance;
    [SerializeField] GameObject upgradeShopPanel;

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
    public void OpenUpgradeShop()
    {
        upgradeShopPanel.SetActive(true);
        Time.timeScale = 0;
    }


    public void ExitUpgradeShop()
    {
        upgradeShopPanel.SetActive(false);
        Debug.Log("Exit Upgrade Shop");
        Time.timeScale = 1;
    }
}
