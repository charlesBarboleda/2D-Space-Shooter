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
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {

        if (other.gameObject.CompareTag("Player"))
        {
            UIManager.Instance.ExitSkillTree();
        }
    }
}
