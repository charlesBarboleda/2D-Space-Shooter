using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboTimerFill : MonoBehaviour
{
    [SerializeField] Image image;                   // The image that shows the fill
    [SerializeField] GameObject _basicSparkPrefab;  // Reference to the spark prefab (with a RectTransform)
    [SerializeField] RectTransform _basicSpark;     // The RectTransform of the instantiated spark object
    private RectTransform _imgRectTransform;        // The RectTransform of the image

    void Start()
    {
        // Get the RectTransform of the Image (UI element)
        _imgRectTransform = image.GetComponent<RectTransform>();
    }

    void Update()
    {
        // If the combo timer is less than 3 seconds and the combo count is greater than 0
        if (ComboManager.Instance.comboTimer < 3f && ComboManager.Instance.comboCount > 0)
        {
            image.enabled = true;
            image.fillAmount = ComboManager.Instance.comboTimer / 3f;

            // Make sure the spark is active
            _basicSpark.gameObject.SetActive(true);

            // Update the spark's position to follow the fill amount
            UpdateSparkPosition(image.fillAmount);
        }
        else
        {
            image.enabled = false;

            // Disable the spark if the combo timer is inactive
            if (_basicSpark != null)
            {
                _basicSpark.gameObject.SetActive(false);
            }
        }
    }

    // Updates the position of the basic spark based on the fill amount
    void UpdateSparkPosition(float fillAmount)
    {
        if (!_basicSpark.gameObject.activeInHierarchy)
        {
            _basicSpark.gameObject.SetActive(true);
        }

        // Calculate the width of the fill portion based on the fill amount
        float fillWidth = _imgRectTransform.rect.width * fillAmount;

        // Set the spark's anchored position relative to the image's width
        Vector2 anchoredPosition = new Vector2(fillWidth - (_imgRectTransform.rect.width / 2f), _imgRectTransform.anchoredPosition.y);
        _basicSpark.anchoredPosition = anchoredPosition;
    }
}