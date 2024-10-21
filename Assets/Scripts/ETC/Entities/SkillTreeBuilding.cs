using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTreeBuilding : MonoBehaviour
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
            UIManager.Instance.OpenBuildingConfirmationPanel("SkillTree");
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
