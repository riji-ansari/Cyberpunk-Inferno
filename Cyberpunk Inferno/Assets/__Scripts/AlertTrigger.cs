using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertTrigger : MonoBehaviour
{
    public Enemy[] enemiesToAlert; //set in inspector
   
    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("Trigger " + gameObject.name + " collided with " + other.name + " with tag " + other.tag);
        if (other.tag == "Player")
        {
            foreach (Enemy e in enemiesToAlert) //alerts all enemies on the list when the player walks through the trigger
            {
                //Debug.Log("Enemy " + e.name + " alerted");
                e.AlertEnemy(); 
            }
        }
    }
}
