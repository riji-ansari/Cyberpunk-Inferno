using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ShootingBehaviour : MonoBehaviour
{
    public GameObject projectile; //make sure to use an ENEMY PROJECTILE prefab!
    public float velocity = 10f;
    public float cooldown = 1f;
    public bool triggerAlert = false;
    public bool triggerShoot = true;
    public Enemy enemy;
    

    public bool onCooldown = false;
    public bool canSeePlayer = false;

    private void Awake()
    {
        projectile = (GameObject) AssetDatabase.LoadAssetAtPath("Assets/_Prefabs/EnemyProjectile.prefab", typeof(GameObject));
        projectile.GetComponent<EnemyProjectile>().speed = velocity;
    }

    private void Update()
    {
        projectile.GetComponent<EnemyProjectile>().damage = enemy.damage;
        // Bit shift the index of the layer (0 and 19) to get a bit mask
        LayerMask playerMask = LayerMask.GetMask("Player");
        LayerMask wallmask = LayerMask.GetMask("Default");
        LayerMask mask = playerMask + wallmask;

        Vector2 direction = PlayerController.Instance.transform.position - this.transform.position; //direction we will cast the ray in
        direction.Normalize();

        RaycastHit2D hit;

        hit = Physics2D.Raycast(transform.position, direction, Mathf.Infinity, mask);
        
        if (hit.collider != null) //uncomment these to see what the ray is hitting
        {
            //Debug.DrawRay(transform.position, direction * hit.distance, Color.yellow, 1f, false);
            if (hit.collider.name == "Player")
            {
                canSeePlayer = true;
                if (triggerAlert)
                {
                    enemy.AlertEnemy();
                }
                if (triggerShoot)
                {
                    enemy.Shoot();
                }
            }
            else
            {
                canSeePlayer = false;
            }
            
        }
        else
        {
           // Debug.DrawRay(transform.position, direction * hit.distance, Color.white, 1f, false);
            Debug.Log("Did not Hit");
        }
    }

    public void Cooldown()
    {
        onCooldown = true;
        Invoke("offCooldown", cooldown);
    }

    private void offCooldown()
    {
        onCooldown = false;
    }
}
