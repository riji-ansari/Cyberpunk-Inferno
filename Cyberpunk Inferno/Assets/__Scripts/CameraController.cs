using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class is attached to the Main Camera GameObject 
//Ensures the Main Camera is carried over between levels

public class CameraController : MonoBehaviour
{

    //implement Singleton pattern for CameraController --------------------------------------------------------------------------
    private static CameraController _instance;

    //call this method to access the Singleton instance of this class
    public static CameraController Instance { get { return _instance; } }


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

    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        moveCamera();
    }

    // Update is called once per frame (after everything else)
    void LateUpdate()
    {
        //update camera position at every frame (after everything else)
        moveCamera();
    }

    void moveCamera()
    {
        //get players current position
        Vector3 playerPos = getPlayerPos();
        //move the camera there
        transform.position=new Vector3(playerPos.x, playerPos.y, transform.position.z);
    }

    Vector3 getPlayerPos()
    {
        //get current postion of player as a Vector3 object
        return player.transform.position;
    }
}
