using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Image image;
    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        SetHealth();
    }

    public void SetHealth()
    {
        image.fillAmount = PlayerManager.GetPlayer().playerHealth / PlayerManager.GetPlayer().maxHealth;


    }
}
