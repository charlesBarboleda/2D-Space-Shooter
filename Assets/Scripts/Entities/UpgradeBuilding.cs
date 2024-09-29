using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeBuilding : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            UpgradeShopManager.Instance.OpenUpgradeShop();
            Debug.Log("Inside Upgrade building");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {

        if (other.gameObject.CompareTag("Player"))
        {
            UpgradeShopManager.Instance.ExitUpgradeShop();
            Debug.Log("Exit Upgrade building");
        }
    }
}
