using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CaptureCircle : MonoBehaviour
{
    public TextMeshProUGUI _progressText;
    public Image circleFill;
    public float progress;
    public float captureTime = 30f;
    public GameObject worldCanvas;

    void OnEnable()
    {

        progress = 0;
        circleFill.fillAmount = 0;
        _progressText.text = "0%";
        worldCanvas = GameObject.Find("WorldCanvas");
        transform.SetParent(worldCanvas.transform);
    }

    IEnumerator Capture()
    {
        while (progress < captureTime)
        {
            progress += Time.deltaTime;
            _progressText.text = Mathf.Round(progress / captureTime * 100).ToString() + "%";
            circleFill.fillAmount = progress / captureTime;
            yield return null;
        }
    }

    public bool IsCaptured()
    {
        return progress >= captureTime;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(Capture());
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StopAllCoroutines();
        }
    }
}
