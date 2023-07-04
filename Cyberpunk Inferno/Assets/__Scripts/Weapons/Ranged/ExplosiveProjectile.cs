using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ExplosiveProjectile : Projectile
{
    private Rigidbody2D _rb;
    private GameObject projectile;

    // Start is called before the first frame update

    void Start()
    {
        speed = 2.5f;
        useDefaultProjectile();
        _rb = GetComponent<Rigidbody2D>();
        // add equipped weapon stats (need to integrate with inventory system)
        // temporary constant
        Invoke("Kill", 50 / speed); //kills the projectile after a lifetime decided by the speed, to ensure no framerate issues from too many projectiles.
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _rb.velocity = transform.right * speed;
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        Debug.Log("Co0llided w/ " + other.name);

        if (other.tag == "Enemy")
        {

            if (weapon == null) //Throw an exception if a projectile has no weapon
            {
                throw new System.Exception("Projectile " + gameObject.name + " not assigned a weapon.");
            }
            other.GetComponent<Enemy>().onPlayerHit(weapon);
        }
        Kill();
    }

    protected void useDefaultProjectile()
    {
        projectile = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/_Prefabs/Projectile.prefab", typeof(GameObject)); //get the base projectile if it has not been assigned
        projectile.GetComponent<Projectile>().weapon = weapon;
    }

    private void Kill()
    {
        Vector3 angle = gameObject.transform.rotation.eulerAngles;
        for (int a=0; a<=360; a += 36)
        {
            print("sdv");
            Instantiate(projectile, gameObject.transform.position,Quaternion.Euler(angle.x,angle.y,angle.z+a));
        }
        Destroy(gameObject);
    }

}
