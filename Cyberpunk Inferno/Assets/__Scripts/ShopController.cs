using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ShopController : MonoBehaviour
{
    //implement Singleton pattern for PlayerInventory --------------------------------------------------------------------------
    private static ShopController _instance;

    //call this method to access the Singleton instance of this class
    public static ShopController Instance { get { return _instance; } }


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
    public GameObject shopInventory = null;
    public GameObject productsParent = null;
    public GameObject playerBalance = null;

    public int shopSize = 6;


    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void setActive(bool active)
    {

        //call method to disable/enable menu
        mainCanvas.SetActive(active);

        //setup the items for sale
        populateProducts();

        //update player's balance
        updateBalance();
    }

    public void updateBalance()
    {
        playerBalance.GetComponent<Text>().text = "x " + PlayerController.Instance.gold;
    }

    public void populateProducts()
    {
        //on initialization: populate the ProductSlots with items from the shopInventory

        for (int a = 0; a < shopSize; a++)
        {
            if (shopInventory.transform.childCount > 0) { 
            GameObject item = shopInventory.transform.GetChild(0).gameObject; //always use index 0, since number of items decreases, which messes with index numbers
            productsParent.transform.GetChild(a).gameObject.GetComponent<ProductSlot>().addItem(item);
            }
            else
            {
                productsParent.transform.GetChild(a).gameObject.GetComponent<ProductSlot>().UpdateProductInfo();
            }
        }
    }

}
