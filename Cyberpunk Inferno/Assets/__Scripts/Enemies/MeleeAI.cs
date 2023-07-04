using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAI : AI
{
    private Vector3 playerPosition;

    private void Awake()
    {
        _rb = gameObject.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        playerPosition = PlayerController.Instance.gameObject.transform.position; //gets player position to run towards them
    }

    public override void Move(float speed)
    {
        if (!PlayerController.Pause)
        {
            base.moveTowardsTarget(speed, playerPosition);
        }
    }
}
