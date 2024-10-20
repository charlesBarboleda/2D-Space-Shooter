using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeBuilding : MonoBehaviour
{
    bool isShopOpen = false;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (isShopOpen)
            {
                return;
            }
            isShopOpen = true;
            UIManager.Instance.OpenUpgradeShop();
            Debug.Log("Inside Upgrade building");
        }
    }
    public void TurnUpgradeShopOff() => isShopOpen = false;
}
