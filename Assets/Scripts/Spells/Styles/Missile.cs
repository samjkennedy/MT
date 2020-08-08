using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : Style
{

    private SpriteRenderer sr;

    private float speedModifier = 2f;
    private float damageModifier = 1f;
    private float rangeModifier = 1f;

    void Awake() {
        sr = GetComponent<SpriteRenderer>();
    }
    
    public override StyleType GetType() {
        return StyleType.MISSILE;
    }

    public override Vector3 GetNextPos(ref Vector3 velocity, float deltaTime) {
        return velocity * Time.deltaTime * speedModifier;
    }

    public override float GetSpeedModifier() {
        return speedModifier;
    }

    public override float GetDamageModifier() {
        return damageModifier;
    }

    public override float GetRangeModifier() {
        return rangeModifier;
    }

    public override void SetProjectileColour(Color colour) {
        sr.color = colour;
    }

    public override void PerformHitAction(Spell spell) {
        return;
    }
}
