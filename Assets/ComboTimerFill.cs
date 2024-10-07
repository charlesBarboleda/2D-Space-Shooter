using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboTimerFill : MonoBehaviour
{
    [SerializeField] Image image;                   // The image that shows the fill

    void Update()
    {
        // If the combo timer is less than 3 seconds and the combo count is greater than 0
        if (ComboManager.Instance.comboTimer < 3f && ComboManager.Instance.comboCount > 0)
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
