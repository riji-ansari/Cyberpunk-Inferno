using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProductSlot : MonoBehaviour
{
    public GameObject itemSlot = null;
    public GameObject itemName = null;
    public GameObject itemType = null;
    public GameObject itemCost = null;

    public void UpdateProductInfo()
    {

        if (itemSlot.GetComponent<Slot>().isEmpty())
        {
            itemName.GetComponent<Text>().text = "SOLD OUT";
            itemType.GetComponent<Text>().text = "";
            itemCost.GetComponent<Text>().text = "-";
        }
        else
        {
            //print("updating");
            Item item = itemSlot.GetComponent<Slot>().storedItem.GetComponent<Item>();

            itemName.GetComponent<Text>().text = item.itemName;
            //print(item.itemName);
            itemType.GetComponent<Text>().text = ""+item.type;
            //print("" + item.type);
            itemCost.GetComponent<Text>().text = "x "+item.cost;
            //print("" + item.cost);
        }
    }

    public void addItem(GameObject newItem)
    {
        itemSlot.GetComponent<Slot>().addItem(newItem);
        UpdateProductInfo();
    }

    public void removeItem()
    {
        itemSlot.GetComponent<Slot>().removeItem();
        UpdateProductInfo();
    }

}
