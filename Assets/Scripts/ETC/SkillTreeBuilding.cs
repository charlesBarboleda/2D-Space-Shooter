using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTreeBuilding : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            UIManager.Instance.OpenSkillTree();
        }
    }
}
