using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    //implement Singleton pattern for HUD --------------------------------------------------------------------------
    private static HUDController _instance;

    //call this method to access the Singleton instance of this class
    public static HUDController Instance { get { return _instance; } }


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
    public GameObject levelDisplay = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void setActive(bool active)
    {

        //call method to disable/enable menu
        mainCanvas.SetActive(active);
    }

    public void updateLevelDisplay(string level)
    {
        levelDisplay.GetComponent<Text>().text =level;
    }

}
