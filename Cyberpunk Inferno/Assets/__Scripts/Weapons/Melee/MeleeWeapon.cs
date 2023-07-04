using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MeleeWeapon : Weapon
{
    /*public MeleeWeapon(float damage, float cooldown) : base(damage, cooldown)
    {
        this.weaponType = WeaponType.melee;
        this.damage = damage;
        this.cooldown = cooldown;
    }*/

    public override void Attack()
    {
        if(canAttack)
        {
            
            PlayerController.Instance.anim.SetBool("attacking", true);
            canAttack = false;
            Invoke("ActivateCooldown", 0.5f); // hardcoded melee cooldown to match animation for now
            Invoke("StopAnimation", 0.5f);
        }
    }

    public override void MeleeHit(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            other.GetComponent<Enemy>().onPlayerHit(this);
        }
    }

    private void StopAnimation()
    {
        PlayerController.Instance.anim.SetBool("attacking", false);
    }

    /*private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            other.GetComponent<Enemy>().onPlayerHit(this);
            Debug.Log("HIT");
        }
    }*/
}
