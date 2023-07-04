using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooter : Enemy
{
    private ShootingBehaviour shootBehaviour;
    public override void Shoot()
    {
        if(!shootBehaviour.onCooldown)
        {
            Vector3 myLocation = transform.position;
            Vector3 targetLocation = PlayerController.Instance.transform.position;
            targetLocation.z = myLocation.z; // ensure there is no 3D rotation by aligning Z position

            // vector from this object towards the target location
            Vector3 vectorToTarget = targetLocation - myLocation;
            // rotate that vector by 90 degrees around the Z axis
            Vector3 rotatedVectorToTarget = Quaternion.Euler(0, 0, 90) * vectorToTarget;

            // get the rotation that points the Z axis forward, and the Y axis 90 degrees away from the target
            // (resulting in the X axis facing the target)
            Quaternion targetRotation = Quaternion.LookRotation(forward: Vector3.forward, upwards: rotatedVectorToTarget);
            Instantiate(shootBehaviour.projectile, gameObject.transform.position, targetRotation);
            shootBehaviour.Cooldown();
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        shootBehaviour = this.GetComponent<ShootingBehaviour>();
        this.maxHealth = 50;
        this.health = 50;
    }

    // Update is called once per frame
    void Update()
    {
        if (ai != null) //shooters can be stationary, like turrets. This will cause them to have no AI.
        {
            ai.Move(movementSpeed);
        }
    }
}
