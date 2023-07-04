using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour , IPointerClickHandler
{
    public GameObject storedItem=null;
    public GameObject iconDisplay;

    public ItemType itemType;
    public SlotSection slotSection;

    private void Start()
    {
        updateSlot();
    }

    //Detect if a click occurs
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        //Use this to tell when the user right-clicks
        if (pointerEventData.button == PointerEventData.InputButton.Right)
        {
            if (!isEmpty())
            {
                if (slotSection == SlotSection.Inventory || slotSection == SlotSection.Equipment)
                {
                    PlayerInventoryController.Instance.transferItem(gameObject, storedItem);
                }
                else if (slotSection == SlotSection.Shop) //if item is being transferred out of the shop
                {
                    int cost = storedItem.GetComponent<Item>().cost;

                    //dont perform the transfer unless player can afford it
                    if(PlayerController.Instance.gold >= cost){
                        //perform the transfer
                        PlayerInventoryController.Instance.transferItem(gameObject, storedItem);
                        //deduct currency as appropriate
                        PlayerController.Instance.gold -=cost;
                        //user is most definitely in shop menu, so update the displayed balance
                        ShopController.Instance.updateBalance();
                    }


                }
            }
 
        }
    }

    public bool isEmpty()
    {
        return (storedItem == null);
    }

    /**
     * This method is called from outside
     * Determines if this slot object is able to hold the parameter item object
     */
    public bool isAvailable(GameObject newItem)
    {
        return (isEmpty() && compatibleItem(newItem));
    }

    public bool compatibleItem(GameObject targetItem)
    {

        ItemType item = targetItem.GetComponent<Item>().type;

        //switch statement depends on the slot's section (Inventory or Equipment)
        switch (slotSection)
        {
            //Inventory slots can hold any item type
            case SlotSection.Inventory:
                {
                    return true;
                }
            //Equipment slots can only hold a specific type (Armor, Weapon, Relic), defined by each instance
            case SlotSection.Equipment:
                {
                    return (item == itemType);
                }
            //default is false, since more errors can be caused by items in incompatible slots
            default:
                {
                    return false;
                }

        }
    }

    //If item slot is empty: disable the gameobject that displays the item sprite
    //If the item slot is filled: enable the item sprite display and set it to the correct sprite
    public void updateSlot()
    {
        if (itemType == ItemType.Null)//ignore all this if slot is a placeholder (ItemType==null)
        {
        }
        else if (isEmpty())
        {
            iconDisplay.SetActive(false);
        }
        else
        {
            iconDisplay.SetActive(true);
            iconDisplay.GetComponent<Image>().sprite = storedItem.GetComponent<Item>().getSprite();
        }
        

    }

    public void addItem(GameObject newItem)
    {

        //existing items will be overwritten
        if (!isEmpty())
        {
            removeItem();
        }

        storedItem = newItem;
        storedItem.SetActive(false);
        itemType = storedItem.GetComponent<Item>().type;

        storedItem.transform.parent = transform;
        storedItem.transform.position = new Vector3(0, 0, 0);

        updateSlot();
    }

    public GameObject removeItem()
    {
        GameObject item = storedItem;
        storedItem = null;
        updateSlot();

        return item;
    }

}
