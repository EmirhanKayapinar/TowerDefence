using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetLocator : MonoBehaviour
{
    [SerializeField] Transform weapon;
    [SerializeField] ParticleSystem projectileParticle;
    [SerializeField] float range =15f;
    Transform target;



    
    void Update()
    {

        FindClosestTarget();

        AimWeapon();
    }

    private void FindClosestTarget()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        Transform closestTarget = null;
        float maxDistance = Mathf.Infinity;

        foreach (Enemy item in enemies)
        {
            float targetDistance = Vector3.Distance(transform.position, item.transform.position);

            if (targetDistance< maxDistance)
            {
                closestTarget = item.transform;
                maxDistance = targetDistance;
            }

            target = closestTarget;

        }
    }

    private void AimWeapon()
    {
        float targetDistance = Vector3.Distance(transform.position, target.position);

        weapon.LookAt(target);

        if (targetDistance<range)
        {
            Attack(true);
        }

        else
        {
            Attack(false);
        }
    }

    void Attack(bool isActive)
    {
        var emissinModule = projectileParticle.emission;
        emissinModule.enabled = isActive;
    }
}
