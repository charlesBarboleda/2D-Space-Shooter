using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTreeBuilding : MonoBehaviour
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
            UIManager.Instance.OpenSkillTree();
            Debug.Log("Inside Skill Tree building");
        }

    }

    public void TurnSkillTreeOff() => isShopOpen = false;
}
