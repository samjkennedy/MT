using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Consumable : PhysicsObject
{
    public Player player;

    public Vector3 sdVelocity;
    public float speed = 10f;
    public float gravity = -25;

    void Start()
    {
        base.Start();
    }

    void Update()
    {
        base.Update();
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            if (PickUp()) {
                Destroy(gameObject);
            }
        }
    }

    public override float GetGravity() {
        return gravity;
    }

    public abstract bool PickUp();
}
