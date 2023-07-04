using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//This class is attached to the GameObject that holds ALL Player Inventory Menu GameObjects
//Ensures the Inventory menu is carried over between levels

public class PlayerInventoryController : MonoBehaviour
{

    //implement Singleton pattern for PlayerInventory --------------------------------------------------------------------------
    private static PlayerInventoryController _instance;

    //call this method to access the Singleton instance of this class
    public static PlayerInventoryController Instance { get { return _instance; } }


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }
    //end of Singleton setup ---------------------------------------------------------------------------------------------------

    //gameobjects from the UI that must be referenced in the script
    [Header("Active Elements (DON'T LEAVE NULL)")]
    public GameObject mainCanvas = null;
    public GameObject inventoryParent = null;
    public GameObject equipmentParent = null;
    public GameObject goldText = null;
    public GameObject filledXP = null;
    public GameObject playerLvlText = null;
    //end of referenced UI elements

    [Header("Parameters")]
    //stores data on items in player's inventory
    public int inventorySize = 16;
    private GameObject[] invSlot;
    //stores data on items in player's equipment
    public int equipSize = 5;
    private GameObject[] equSlot;

    // Start is called before the first frame update
    void Start()
    {
        //set player menu as disabled
        setActive(false);

        //instantiate inventory array
        invSlot = new GameObject[inventorySize];

        //assign references to each inventory slot
        for(int i=0; i<inventorySize; i++)
        {
            invSlot[i] = inventoryParent.transform.GetChild(i).gameObject;
        }

        //instantiate equipment array
        equSlot = new GameObject[equipSize];

        //assign references to each equipment slot
        for (int i = 0; i < equipSize; i++)
        {
            equSlot[i] = equipmentParent.transform.GetChild(i).gameObject;
        }
    }

    public void reset()
    {

        for (int i = 0; i < inventorySize; i++)
        {
            inventoryParent.transform.GetChild(i).GetComponent<Slot>().removeItem();
        }
        for (int a = 0; a < equipSize; a++)
        {
            equipmentParent.transform.GetChild(a).GetComponent<Slot>().removeItem();
        }
    }

    public void setActive(bool active) 
    {

        //call method to disable/enable menu
        mainCanvas.SetActive(active);

        //update amount of gold displayed
        goldText.GetComponent<Text>().text ="x "+PlayerController.Instance.gold;

        //update size of the XP bar
        filledXP.GetComponent<Image>().fillAmount=xpFillAmount();

        //update player level displayed
        playerLvlText.GetComponent<Text>().text =" "+PlayerController.Instance.Level;
    }

    private float xpFillAmount()
    {
        float parentHeight = filledXP.transform.parent.GetComponent<RectTransform>().rect.height;
        float parentWidth = filledXP.transform.parent.GetComponent<RectTransform>().rect.width;

        float xp =PlayerController.Instance.XP;
        int level = PlayerController.Instance.Level;
        float nextMilestone = PlayerController.Instance.levelMilestones[level-1];


        float progress;
        if(level == 1)
        {
            progress = xp / nextMilestone;
        }
        else if(level== PlayerController.Instance.levelMilestones.Length && xp>=nextMilestone)
        {
            progress = 1;
        }
        else
        {
            float prevMilestone = PlayerController.Instance.levelMilestones[level - 2];
            progress = (xp - prevMilestone) / nextMilestone;
        }

        return progress;
    }

    public void pickupObject(GameObject newItem)
    {
        GameObject openSlot = findEmptySlot(invSlot, newItem);
        if (openSlot != null)
        {
            openSlot.GetComponent<Slot>().addItem(newItem);
        }
    }

    /**
     * @param: looks for empty slot in inventory or equipment
     */
    private GameObject findEmptySlot(GameObject[] section, GameObject newItem)
    {
        for(int i=0; i<section.Length; i++)
        {
            if (section[i].GetComponent<Slot>().isAvailable(newItem))
            {
                return section[i];
            }
        }
        return null;
    }

    public void transferItem(GameObject fromSlot,GameObject newItem)
    {
        GameObject[] section;
        GameObject toSlot;

        //Debug.Log("Moving item " + newItem.name);

        switch (fromSlot.GetComponent<Slot>().slotSection)
        {
            case SlotSection.Inventory:
                {
                    //Debug.Log("Equipping..");
                    section = equSlot;
                    break;
                }
            case SlotSection.Equipment:
                {
                    section = invSlot;
                    break;
                }
            case SlotSection.Shop:
                {
                    section = invSlot;
                    break;
                }
            default:
                {
                    section = invSlot;
                    break;
                }
        }

        //insert the item in the first available slot
        toSlot = findEmptySlot(section,newItem);

        //don't perform the transfer if no slots are available
        if (toSlot != null)
        {
            //remove item from the clicked slot and add it to the new slot
            toSlot.GetComponent<Slot>().addItem(fromSlot.GetComponent<Slot>().removeItem());

            if(newItem.GetComponent<Item>() != null) //makes sure an Item component exists
            {
            
                if(newItem.GetComponent<Item>().weapon != null) //if the item is a weapon,..
                {
                    if (section == invSlot) //if it's being moved out of the equipment, we must remove it.
                    {
                        PlayerController.Instance.weapon = null;
                        PlayerController.Instance.weaponSprite.sprite = null;
                    }
                    else if (section == equSlot)
                    {
                        PlayerController.Instance.weapon = newItem.GetComponent<Item>().weapon; //equip the weapon if it's being moved to equipment
                        PlayerController.Instance.weaponSprite.sprite = newItem.GetComponent<Item>().getSprite();
                    }
                    
                }

                if(newItem.GetComponent<Item>().relic != null) //if the item is a relic,...
                {
                    Relic relic = newItem.GetComponent<Item>().relic;
                    if(section == equSlot)
                    {
                        Debug.Log("Relic added");
                        PlayerController.Instance.addRelic(relic);
                    }
                    else if (section == invSlot)
                    {
                        PlayerController.Instance.removeRelic(relic);
                        Debug.Log("Relic removed");
                    }

                }
            }
        }
    }

}
