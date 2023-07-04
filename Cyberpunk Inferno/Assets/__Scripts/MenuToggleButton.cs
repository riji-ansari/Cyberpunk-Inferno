using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuToggleButton : MonoBehaviour, IPointerClickHandler
{

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        //Use this to tell when the user right-clicks
        if (pointerEventData.button == PointerEventData.InputButton.Left)
        {
            //print("clicked");
            toggleMenu();

        }
    }

    private void toggleMenu()
    {
        if (MenuController.Instance.state == MenuController.MenuState.SkillMenu)
        {
            MenuController.Instance.state = MenuController.MenuState.PlayerInventory;
        }
        else if (MenuController.Instance.state == MenuController.MenuState.PlayerInventory)
        {
            MenuController.Instance.state = MenuController.MenuState.SkillMenu;
        }
    }

}
