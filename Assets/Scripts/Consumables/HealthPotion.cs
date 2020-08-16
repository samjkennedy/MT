using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : Consumable
{
    void Start()
    {
        base.Start();
        GameObject playerGameObj = GameObject.Find("Player");
        player = playerGameObj.GetComponent<Player>();
    }

    void Update()
    {
        base.Update();
        if (player == null) {
            GameObject playerGameObj = GameObject.Find("Player");
            player = playerGameObj.GetComponent<Player>();
            return;
        }

        if (HealthController.instance.AtFullHealth()) {
            return;
        }

        float distanceToPlayer = (player.transform.position - transform.position).magnitude;
        if (distanceToPlayer < 3f) {
            transform.position = Vector3.SmoothDamp(transform.position, player.transform.position, ref sdVelocity, 1f / speed);
        }
    }

    public override bool PickUp() {
        if (HealthController.instance.AtFullHealth()) {
            return false;
        }
        HealthController.instance.Increase(2);
        return true;
    }
}
