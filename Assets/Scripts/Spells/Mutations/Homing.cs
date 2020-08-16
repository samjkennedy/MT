using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Homing : Mutation
{
    
    public float range;
    public float strength;

    Vector3 directionToClosestEnemy;

    public override void AlterPath(ref Vector3 velocity, float deltaTime) {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, range);
        if (hitColliders.Length == 0) {
            return;
        }
        Enemy closestEnemy = null;
        directionToClosestEnemy = Vector3.positiveInfinity;
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.tag == "Enemy") {
                Enemy enemy = hitCollider.GetComponent<Enemy>();
                if (!enemy.IsKillable()) {
                    continue;
                }
                Vector3 vectorToEnemy = (enemy.transform.position - transform.position);

                if (vectorToEnemy.magnitude < directionToClosestEnemy.magnitude) {
                    closestEnemy = enemy;
                    directionToClosestEnemy = vectorToEnemy;
                }
            }
        }
        if (closestEnemy == null) {
            return;
        }
        
        velocity += (directionToClosestEnemy.normalized * strength);
    }

}
