using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : Enemy
{
    
    //Combat
    private int damage = 1;//half a heart
    private float attackSpeed = 1f; //once per second
    private bool attackCooldown = false;

    //Movement
    private float jumpHeight = 1.0f;
    private float timeToJumpApex = 0.55f;
    private float jumpFrequency = 2.5f;
    private float gravity;
    private float jumpVelocity;
    private bool canJump = true;

    void Start()
    {
        base.Start();

        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
    }

    void Update()
    {
        base.Update();

        if (!attackCooldown && controller.collisions.any) {
            RaycastHit2D hit = controller.collisions.hit;
            if (hit.transform == player.transform) {
                StartCoroutine(HitPlayer());
            }
        }

        //Move towards player
        Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
        velocity.x = directionToPlayer.x * movementSpeed;

        if (canJump && Random.Range(0f, 1f) > 0.975f) {
            StartCoroutine(Jump());
        }

        Move(velocity * Time.deltaTime + (0.5f * Vector3.up * gravity * Mathf.Pow(Time.deltaTime, 2)));
    }

    IEnumerator Jump() {
        canJump = false;

        velocity.y = jumpVelocity;
        Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
        velocity.x = directionToPlayer.x * movementSpeed;

        yield return new WaitForSeconds(jumpFrequency);
        canJump = true;
    }

    private IEnumerator HitPlayer() {
        player.Hit(damage);
        attackCooldown = true;
        yield return new WaitForSeconds(attackSpeed);
        attackCooldown = false;
    }

    public override float GetGravity() {
        return gravity;
    }
}
