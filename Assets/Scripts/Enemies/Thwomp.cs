using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thwomp : Enemy
{
    public float gravity;
    private int damage = 2;//a whole heart
    private float timeOnGround = 1f;

    private bool isFalling = false;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        gravity = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();

        Vector2 raycastOrigin = new Vector2(
            (GetBounds().min.x + GetBounds().max.x) / 2.0f, 
            GetBounds().min.y - 0.001f
        );

        RaycastHit2D hit = Physics2D.Raycast(raycastOrigin, Vector2.down, Mathf.Infinity, 1 << 8);
        
        if (!isFalling && hit && hit.transform == player.transform) {
            isFalling = true;
            gravity = -50f;
        }

        if (isFalling && controller.collisions.below) {
            RaycastHit2D attackRay = controller.collisions.hit;
            if (attackRay.transform == player.transform) {
                player.Hit(damage);
            }
        }

        if (controller.collisions.below) {
            StartCoroutine(ReturnToCeiling());
        }

        if (controller.collisions.above) {
            RaycastHit2D ceilingRay = controller.collisions.hit;
            if (ceilingRay.transform.tag == "Obstacles") {
                isFalling = false;
                gravity = 0f;
            }
        }
    }    

    private IEnumerator ReturnToCeiling() {
        yield return new WaitForSeconds(timeOnGround);

        gravity = 10f;
    }

    public override void Hit(Spell spell) {
        TakeDamage(0);
    }

    public override bool IsKillable() {
        return false;
    }

    public override float GetGravity() {
        return gravity;
    }
}
