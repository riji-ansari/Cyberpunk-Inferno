using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// attach script to MeleeWeapon on Player to provide a really hacky workaround to implement melee temporarily
public class MeleeHitbox : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Weapon weapon = PlayerController.Instance.weapon;
        if (weapon != null)
        {
            weapon.MeleeHit(other);
        }

    }
}
