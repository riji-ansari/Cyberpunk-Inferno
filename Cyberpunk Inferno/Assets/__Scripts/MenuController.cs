using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    //implement Singleton pattern for MenuController --------------------------------------------------------------------------
    private static MenuController _instance;

    //call this method to access the Singleton instance of this class
    public static MenuController Instance { get { return _instance; } }


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

    private MenuState _state;
    public MenuState state
    {
        
        get {
            return _state;
        }
        set
        {
            _state = value;
            switch (_state)
            {
                case MenuState.HUD:
                    {
                        HUDController.Instance.setActive(true);
                        PlayerInventoryController.Instance.setActive(false);
                        ShopController.Instance.setActive(false);
                        SkillMenuController.Instance.setActive(false);

                        //unpause all entities when this menu is open
                        PlayerController.Pause = false;
                        Enemy.paused = false;

                        break;
                    }
                case MenuState.PlayerInventory:
                    {
                        HUDController.Instance.setActive(false);
                        PlayerInventoryController.Instance.setActive(true);
                        ShopController.Instance.setActive(false);
                        SkillMenuController.Instance.setActive(false);

                        //pause all entities when this menu is open
                        PlayerController.Pause = true;
                        Enemy.paused = true;

                        break;
                    }
                case MenuState.SkillMenu:
                    {
                        HUDController.Instance.setActive(false);
                        PlayerInventoryController.Instance.setActive(false);
                        ShopController.Instance.setActive(false);
                        SkillMenuController.Instance.setActive(true);

                        //pause all entities when this menu is open
                        PlayerController.Pause = true;
                        Enemy.paused = true;

                        break;
                    }
                case MenuState.Shop:
                    {
                        HUDController.Instance.setActive(false);
                        PlayerInventoryController.Instance.setActive(false);
                        ShopController.Instance.setActive(true);
                        SkillMenuController.Instance.setActive(false);

                        //pause all entities when this menu is open
                        PlayerController.Pause = true;
                        Enemy.paused = true;

                        break;
                    }

            }
        }
    }
    public enum MenuState
    {
        HUD,
        PlayerInventory,
        SkillMenu,
        Shop
    }
    private bool playerAtShop;

    // Start is called before the first frame update
    void Start()
    {
        //set initial states
        state = MenuState.HUD;
        playerAtShop = false;
    }

    // Update is called once per frame
    void Update()
    {
        //player presses "E" to toggle the inventory menu
        if (Input.GetKeyDown("e"))
        {
            switch (state)
            {
                case MenuState.HUD: //if HUD is active, "e" is to enter a menu
                    {
                        if (playerAtShop)
                        {
                            //transition to new state
                            state = MenuState.Shop;
                        }
                        else
                        { 
                            //transition to new state
                            state = MenuState.PlayerInventory;
                        }

                        break;
                    }
                default:
                    //if HUD is inactive, "e" is to exit a menu
                    {
                        //transition to new state
                        state = MenuState.HUD;

                        break;
                    }
            }//end of switch

        }//end of "e" keydown condition

    }//end of Update()

    public void activateShopMenu(bool state)
    {
        playerAtShop = state;
    }


}
