using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashRoller : MonoBehaviour
{
    private Rigidbody2D _rb;
    private float dashTime;
    public float dashSpeed;
    public float startDashTime;
    private int direction;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        direction = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (direction == 0)
        {
            //print("0");
            if (Input.GetKeyDown(KeyCode.LeftArrow) && Input.GetKeyDown(KeyCode.Space))
            {
                //print("1");
                direction = 1;
                startDash();
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) && Input.GetKeyDown(KeyCode.Space))
            {
                //print("2");
                direction = 2;
                startDash();
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow) && Input.GetKeyDown(KeyCode.Space))
            {
                //print("3");
                direction = 3;
                startDash();
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) && Input.GetKeyDown(KeyCode.Space))
            {
                //print("4");
                direction = 4;
                startDash();
            }

        }
        else
        {
            if (dashTime <= 0)
            {
                endDash();
            }
            else
            {
                dashTime -= Time.deltaTime;
                
                if (direction == 1)
                {
                    _rb.velocity = Vector2.left * dashSpeed;
                }
                else if (direction == 2)
                {
                    _rb.velocity = Vector2.right * dashSpeed;
                }
                else if (direction == 3)
                {
                    _rb.velocity = Vector2.up * dashSpeed;
                }
                else if (direction == 4)
                {
                    _rb.velocity = Vector2.down * dashSpeed;
                }
            }
        }
    }

    public void endDash()
    {
        direction = 0;
        dashTime = startDashTime;
        _rb.velocity = Vector2.zero;
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    public void startDash()
    {
        GetComponent<SpriteRenderer>().color = new Color(0, 0, 0.5f, 0.5f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        endDash();
    }
}
