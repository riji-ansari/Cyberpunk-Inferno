using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An AI that chases the player like a missile, meaning, a set velocity which it always moves, and a max turn radius.
/// </summary>

public class MissileAI : AI
{
    public float maxTurnAngle = 1f;
    public GameObject player;
    public Vector3 direction
    {
        get { return direction; }
        set
        {
            value.z = 0; //make sure the set direction has no z coordinates, then normalize it.
            direction = value;
            direction.Normalize();
        }
    }
    public override void Move(float speed)
    {
        Vector3 difference = player.transform.position - gameObject.transform.position;
        difference.z = 0;
        difference.Normalize();
        float angle = Vector3.Angle(direction, difference);
        if (angle < maxTurnAngle) 
        {
            direction = difference;
            base.moveTowardsTarget(speed, player.transform.position);
        }
        else //turning less than the actual angle, then moving towards target.
        {
            //first we need to figure out which way the angle is changing
            
        }
    }
}
