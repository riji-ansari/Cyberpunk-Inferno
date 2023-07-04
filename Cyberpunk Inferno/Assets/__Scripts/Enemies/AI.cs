using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AI : MonoBehaviour
{
    public abstract void Move(float speed);

    protected Rigidbody2D _rb;

    protected void moveTowardsTarget(float speed, Vector3 target)
    {
        //Moves 'speed' units towards the target.
        if (!PlayerController.Pause)
        {
            Vector3 movement = target - gameObject.transform.position;
            movement.z = 0;
            movement.Normalize(); //normalize after removing z component
            movement *= speed * Time.deltaTime * Relic.calculateEnemySpeedMult(PlayerController.Instance.relics);
            _rb.velocity = movement;
        }
    }
}
