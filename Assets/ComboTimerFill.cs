using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboTimerFill : MonoBehaviour
{
    [SerializeField] Image image;


    void Update()
    {
        if (ComboManager.Instance.comboTimer < 3f)
        {
            image.enabled = true;
            image.fillAmount = ComboManager.Instance.comboTimer / 3f;
        }
        else
        {
            image.enabled = false;
        }
    }
}
