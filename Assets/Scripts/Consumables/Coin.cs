﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : Consumable
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
        
        if (ConsumableController.instance.AtMaxCoins()) {
            return;
        }

        float distanceToPlayer = (player.transform.position - transform.position).magnitude;
        if (distanceToPlayer < 3f) {
            transform.position = Vector3.SmoothDamp(transform.position, player.transform.position, ref sdVelocity, 1f / speed);
        }
    }

    public override bool PickUp() {
        if (ConsumableController.instance.AtMaxCoins()) {
            return false;
        }
        ConsumableController.instance.AddCoins(1);
        return true;
    }
    
}
