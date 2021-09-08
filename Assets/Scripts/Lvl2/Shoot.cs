using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : Enemy_behaviour
{
    public List<Weapon> weapons;
    public bool shakeCamera;


    /*public void OnUpdate()
    {
        foreach(var weapon in weapons)
        {
            var projectile = Object.Instantiate(weapon.projectilePrefab, weapon.weaponTransform.position, Quaternion.identity);
            projectile.Shooter = gameObject;

            var force = new Vector2(weapon.horizontalForce * transform.localScale.x, weapon.verticalForce);
            projectile.setForce(force);

            if (shakeCamera)
            {
                CinemachineShake.Instance.ShakeCamera(5f, .1f);
            }
        }
    }*/
}
