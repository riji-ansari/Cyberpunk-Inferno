using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : RangedWeapon
{

    public override void Attack()
    {
        if (projectile == null)
        {
            useDefaultProjectile();
        }
        if (canAttack)
        {
            Vector3 angle = PlayerController.Instance.rangedAttackPoint.rotation.eulerAngles;
            for(int a=-2; a<=2; a++)
            {
                Instantiate(projectile, PlayerController.Instance.rangedAttackPoint.position, Quaternion.Euler(angle.x,angle.y, angle.z+a*5));
            }

            canAttack = false;
            Invoke("ActivateCooldown", cooldown);
        }
    }

    private void Awake()
    {
        cooldown = 1.5f;
        this.damage = 10f;
    }
}
