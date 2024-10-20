using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrestigeBuilding : MonoBehaviour
{
    bool isShopOpen = false;
    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.tag == "Player")
        {
            if (isShopOpen)
            {
                return;
            }
            isShopOpen = true;
            UIManager.Instance.OpenPrestigePanel();

        }
    }

    public void TurnPrestigePanelOff() => isShopOpen = false;

}
