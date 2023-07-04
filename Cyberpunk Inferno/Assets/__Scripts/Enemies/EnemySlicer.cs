using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySlicer : Enemy
{
    public AI AIAfterAlert;
    new public bool Alert //alert system, so that the enemy may patrol an area before being 'awoken' by the player. Enemies cannot go un-alert.
    {
        get
        {
            return _alert;
        }
        set
        {
            _alert = value;
            if(_alert)
            {
                ai = AIAfterAlert;
                movementSpeed *= 1.5f; //become faster when noticing player
            }
           
        }
    }
    private void Awake()
    {
        Alert = false;
        this.maxHealth = 50;
        this.health = 50;
    }

    private void Update()
    {
        if (!PlayerController.Pause)
        {
            ai.Move(movementSpeed);
        }
        else
        {
            GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        }

    }

    public override void onPlayerHit(Weapon wep)
    {
        health -= wep.Damage;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("Collided with " + other.name);
        if (other.tag == "Player")
        {
            PlayerController.Instance.hitPlayer(damage);
        }
    }

    public override void AlertEnemy()
    {
        Alert = true;
    }

    public override void Shoot()
    {
        throw new System.NotImplementedException();
    }
}
