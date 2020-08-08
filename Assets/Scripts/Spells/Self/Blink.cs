using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blink : SelfStyle
{
    
    private float speedMultiplier = 4f;
    private float effectTime = 0.3f;

    public override float GetEffectTime() {
        return effectTime;
    }

    public override void Cast(Player player, Element element) {
        StartCoroutine(CastEffect(player));
    }

    IEnumerator CastEffect(Player player) {
        float originalVelocityX = player.velocity.x;
        player.velocity.x = originalVelocityX * speedMultiplier;

        yield return new WaitForSeconds(effectTime);
        
        player.velocity.x = originalVelocityX;
    }
}
