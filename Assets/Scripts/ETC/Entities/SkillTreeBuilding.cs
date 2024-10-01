using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTreeBuilding : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            UIManager.Instance.OpenSkillTree();
            Debug.Log("Inside Skill Tree building");
        }

    }

    void OnTriggerExit2D(Collider2D other)
    {

        if (other.gameObject.CompareTag("Player"))
        {
            UIManager.Instance.ExitSkillTree();
            Debug.Log("Exit Skill Tree building");
        }
    }
}
