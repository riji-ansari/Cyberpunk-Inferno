using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RangedWeapon : Weapon
{
    public GameObject projectile;
    /*public RangedWeapon(float damage, float cooldown, GameObject projectile) : base(damage, cooldown)
    {
        weaponType = WeaponType.ranged;
        this.damage = damage;
        this.cooldown = cooldown;
        if (projectile.GetComponent<Projectile>() != null)
        {
            this.projectile = projectile;
        }
    }*/


    protected void useDefaultProjectile()
    {
        projectile = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/_Prefabs/Projectile.prefab", typeof(GameObject)); //get the base projectile if it has not been assigned
        projectile.GetComponent<Projectile>().weapon = this;
    }

    public override void MeleeHit(Collider2D other)
    {
        throw new System.NotImplementedException();
    }

    public override void Attack()
    {
        
        if (projectile == null)
        {
            useDefaultProjectile();
        }
        if (canAttack)
        {
            Instantiate(projectile, PlayerController.Instance.rangedAttackPoint.position, PlayerController.Instance.rangedAttackPoint.rotation);
            canAttack = false;
            Invoke("ActivateCooldown", cooldown * Relic.calculateTimeBetweenShotsMult(PlayerController.Instance.relics));
        }
    }
}
