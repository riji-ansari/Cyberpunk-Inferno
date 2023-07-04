using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string ID;
    public string itemName="No Name";
    public string description;
    public int cost=0;
    public Weapon weapon;
    public Relic relic;
    //Uncomment when relics and armour are added
    //public Armour armour;

    public ItemType type;

    private Sprite sprite;

    private void Start()
    {
        sprite = getSprite();
    }

    public Sprite getSprite()
    {
        return gameObject.GetComponent<SpriteRenderer>().sprite;
    }
}
