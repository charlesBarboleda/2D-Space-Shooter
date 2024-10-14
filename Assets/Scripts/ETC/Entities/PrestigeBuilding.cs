using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrestigeBuilding : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.tag == "Player")
        {

            UIManager.Instance.OpenPrestigePanel();

        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            UIManager.Instance.ClosePrestigePanel();
        }
    }
}
