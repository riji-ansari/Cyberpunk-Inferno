using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float speed = 10f;
    public float damage = 10f;

    private Rigidbody2D _rb;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        Invoke("Kill", 50 / speed); //kills the projectile after a lifetime decided by the speed, to ensure no framerate issues from too many projectiles.   
    }

    void FixedUpdate()
    {
        if (!PlayerController.Pause)
        {
            _rb.velocity = transform.right * speed;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Player")
        {
            PlayerController.Instance.hitPlayer(damage);
        }
        if (other.tag == "Wall" || other.tag == "Player") // allows to shoot over lava
        {
            Kill();
        }
    }

    private void Kill()
    {
        Destroy(gameObject);
    }
}
