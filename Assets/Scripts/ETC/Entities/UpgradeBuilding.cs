using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeBuilding : MonoBehaviour
{
    bool isConfirmBoxOpen = false;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (isConfirmBoxOpen)
            {
                return;
            }
            isConfirmBoxOpen = true;
            UIManager.Instance.OpenBuildingConfirmationPanel("Upgrade");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isConfirmBoxOpen = false;
            UIManager.Instance.CloseBuildingConfirmationPanel();
        }
    }
}
