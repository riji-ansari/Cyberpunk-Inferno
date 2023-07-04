using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{

    /**
     * MAKE SURE THE SCENES ARE IN BUILD SETTINGS IN UNITY
     * File > Build Settings
     * 
     * Currently, a seperate instnace of LevelManager is required for each Scene/Level
     * To make it into a singleton would require a pretty radical restructuring of the game's code
     */


    public GameObject player = null;

    public GameObject levelExit = null;
    public string nextLevel;

    //player starts at these coordinates when they first enter the level
    public int initialX = 0;
    public int initialY = 0;

    /**
     * Player is a Singleton, but LevelManager is not
     * Therefore Player must be assigned to each new LevelManager
     */
    private void Awake()
    {
        player = GameObject.Find("Player"); //make sure the Player Singleton is named "Player" in the editor
    }

    // Start is called before the first frame update
    void Start()
    {
        player.transform.position = new Vector3(initialX,initialY);

    }

    // Update is called once per frame
    void Update()
    {

        //if player reaches transition point
        if (player.GetComponent<PlayerController>().atExit())
        {
            switchLevel();
            player.transform.position = Vector3.zero;
        }
    }

    void switchLevel()
    {
        HUDController.Instance.updateLevelDisplay(nextLevel);
        SceneManager.LoadScene(nextLevel);
    }

}
