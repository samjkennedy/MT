using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Controller2D))]
public abstract class PhysicsObject : MonoBehaviour
{
    public Controller2D controller;
    
    public Vector3 velocity;
    
    public virtual void Start()
    {
        controller = GetComponent<Controller2D>();
        velocity = Vector3.zero;
    }

    public virtual void Update()
    {
        //Prevent gravity accumulation
        if (controller.collisions.above || controller.collisions.below) {
            velocity.y = 0;
        }
        if (controller.collisions.left || controller.collisions.right) {
            velocity.x = 0;
        }
        velocity.y += GetGravity() * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime + (0.5f * Vector3.up * GetGravity() * Mathf.Pow(Time.deltaTime, 2)));
    }

    public void Move(Vector3 motion) {
        controller.Move(motion * Time.deltaTime + (0.5f * Vector3.up * GetGravity() * Mathf.Pow(Time.deltaTime, 2)));
    }

    public void SetVelocity(Vector3 velocity) {
        this.velocity = velocity;
    }

    public void AddVelocity(Vector3 velocity) {
        this.velocity += velocity;
    } 

    public abstract float GetGravity();
}
