using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillValueNumber : MonoBehaviour
{
    public Image TargetImage;
    // Update is called once per frame
    void Update()
    {
        float amount = PlayerManager.Instance.GetComponent<PlayerHealthBehaviour>().currentHealth;
        gameObject.GetComponent<Text>().text = amount.ToString("F0");
    }
}
