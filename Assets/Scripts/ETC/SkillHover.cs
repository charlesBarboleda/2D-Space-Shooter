using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class SkillHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Skill skill;
    public GameObject descriptionPanel;
    [SerializeField] TextMeshProUGUI skillNameText, skillCostText, skillDescriptionText, skillMaxLevelText;
    [SerializeField] GameObject defaultIcon;
    bool isHovering = false;

    void Start()
    {
        descriptionPanel.SetActive(false);
        UpdateSkillInfo();
    }

    void Update()
    {
        if (isHovering)
        {
            UpdateSkillInfo(); // Update the skill info continuously if the panel is active
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
        descriptionPanel.SetActive(true);
        UpdateSkillInfo();

        Vector3 offset = new Vector3(0, 120f, 0f); // Offset to position the box
        descriptionPanel.transform.position = Input.mousePosition + offset;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
        descriptionPanel.SetActive(false);
    }

    void UpdateSkillInfo()
    {
        skillNameText.text = skill.skillName;
        skillDescriptionText.text = skill.description;
        skillCostText.text = skill.cost.ToString();
        skillMaxLevelText.text = "MAX LV." + skill.maxSkillLevel.ToString();
        defaultIcon.GetComponent<Image>().sprite = skill.skillIcon;
    }
}
