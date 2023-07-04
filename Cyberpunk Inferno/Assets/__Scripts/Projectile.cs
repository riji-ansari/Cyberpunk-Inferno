using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    private Rigidbody2D _rb;
    public Weapon weapon;
    // Start is called before the first frame update

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        Invoke("Kill", 50/speed); //kills the projectile after a lifetime decided by the speed, to ensure no framerate issues from too many projectiles.
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!PlayerController.Pause)
        {
            _rb.velocity = transform.right * speed * SkillMenuController.getMult(SkillMenuController.SkillMult.BulletVelocity);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        //Debug.Log("Collided w/ " + other.name);

        if (other.tag == "Enemy")
        {
           
            if(weapon == null) //Throw an exception if a projectile has no weapon
            {
                throw new System.Exception("Projectile " + gameObject.name + " not assigned a weapon.");
            }
            other.GetComponent<Enemy>().onPlayerHit(weapon);
        }

        if (other.tag == "Enemy" || other.tag == "Wall") // allows to shoot over lava
        {
            Kill();
        }
    }

    private void Kill()
    {
        Destroy(gameObject);
    }
}
