using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VendorController : MonoBehaviour
{
    public GameObject alertBox;
    private bool _shopActive;
    private bool shopActive
    {
        get
        {
            return _shopActive;
        }
        set
        {
            _shopActive = value;
            alertBox.SetActive(_shopActive);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        shopActive=false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            shopActive = true;
            MenuController.Instance.activateShopMenu(true);
        }
 
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            shopActive = false;
            MenuController.Instance.activateShopMenu(false);
        }
    }
}
