using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private Slider slider;

    void Update()
    {

        SetProgress();
    }
    public void SetProgress()
    {
        slider.value = GameManager.Instance.points;


    }
}
