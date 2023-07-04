using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolAI : AI
{
    public GameObject[] Nodes;
    public bool looping = true;
    private GameObject nextNode;
    private bool forward = true;
    private int currentNode = 0;

    private void Start()
    {
        nextNode = Nodes[0]; //set the first destination
    }

    public override void Move(float speed)
    {
        if (!PlayerController.Pause) //prevents the AI from moving when paused
        {
            base.moveTowardsTarget(speed, nextNode.transform.position);
        }
    }

    private void Awake()
    {
        _rb = gameObject.GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

       
        if (other.transform == nextNode.transform) //after hitting the first node..
        {
            if(nextNode == Nodes[Nodes.Length - 1]) //if this was the last node..
            {
                if (looping)
                {
                    currentNode = -1; //set the next node to the first (this will be incremented to zero first)
                }
                else
                {
                    forward = false; //reverse direction
                }
            }
            else if (nextNode == Nodes[0]) //if this was the frst node..
            {
                forward = true; //go forward (incase it was currently returning to first)
            }
            currentNode += forward ? 1 : -1; //increment/decrement the node tracker
            nextNode = Nodes[currentNode]; //and set the next node accordingly

            //note that Forward is only used if this is a non-looping patrol path
        }
    }
}
