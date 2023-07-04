using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SkillSlot : MonoBehaviour, IPointerClickHandler
{
    public Skill storedSkill;
    public GameObject iconDisplay;
    public GameObject emptySprite;
    public GameObject skillName;
    public GameObject description;
    public GameObject skillCost;
    public GameObject purchaseIndicator;
    public skillSection section;

    public enum skillSection
    {
        available,
        equipped
    }

    public void updateSkillInfo()
    {
        string name = skillName.GetComponent<Text>().text;
        string cost = skillCost.GetComponent<Text>().text;

        if (isEmpty())
        {
            skillName.GetComponent<Text>().text = "________";
            description.GetComponent<Text>().text = "";
            skillCost.GetComponent<Text>().text = "-";

            purchaseIndicator.SetActive(false);
        }
        else
        {
            skillName.GetComponent<Text>().text = storedSkill._name;
            description.GetComponent<Text>().text = storedSkill._effect;
            skillCost.GetComponent<Text>().text = storedSkill._reqPoints+" ";

            if (storedSkill._enabled)
            {
                skillCost.GetComponent<Text>().text += "(PURCHASED)";
            }
            purchaseIndicator.SetActive(storedSkill._enabled);
        }

    }

    public void addSkill(Skill newSkill)
    {
        storedSkill = newSkill;
        iconDisplay.GetComponent<Image>().sprite = newSkill.sprite;
        updateSkillInfo();
    }

    public void removeSkill()
    {
        Skill[] equipped = PlayerController.Instance.equippedSkills;
        for (int a = 0; a < equipped.Length; a++)
        {
            if (equipped[a] == storedSkill)
            {
                equipped[a] = null;
            }
        }

        storedSkill = null;
        iconDisplay.GetComponent<Image>().sprite = emptySprite.GetComponent<SpriteRenderer>().sprite;
        updateSkillInfo();
    }

    public bool isEmpty()
    {
        return (storedSkill == null);
    }

    //Detect if a click occurs
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        //Use this to tell when the user right-clicks
        if (pointerEventData.button == PointerEventData.InputButton.Right)
        {
            if (!isEmpty())
            {
                if (section == SkillSlot.skillSection.available )
                {
                    //only carry out the transfer if player has already purchased the skill, or if a purchase executed now is successful
                    if (storedSkill._enabled || storedSkill.unlock() )
                    {
                        transferSkill();
                        updateSkillInfo();
                    }
                    
                }
                else if (section == SkillSlot.skillSection.equipped)
                {
                    removeSkill();
                }
            }
        }
    }

    private void transferSkill()
    {
        bool success = SkillMenuController.Instance.transferSkill(this);
    }

}
