using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public int gold;
    public GameObject closed=null;
    public GameObject open=null;
    private bool proximity;

    void Start()
    {
        //change sprite
        closed.SetActive(true);
        open.SetActive(false);

        proximity = false;
    }

    // Update is called once per frame
    void Update() //move to MenuController?
    {
        //player presses "E" to toggle the inventory menu
        if (Input.GetKeyDown("e") && proximity)
        {
            //change sprite
            closed.SetActive(false);
            open.SetActive(true);

            //award gold to player
            PlayerController.Instance.gold += this.gold;

            //disable collider to prevent future activation
            GetComponent<BoxCollider2D>().enabled = false;
            //deactivate current activation
            //setActive(false);

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            setActive(true);
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            setActive(false);
        }
    }

    private void setActive(bool active)
    {
        proximity = active;
    }
}
