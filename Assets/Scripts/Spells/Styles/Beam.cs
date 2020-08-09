using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beam : Style
{
    private SpriteRenderer sr;

    private float speedModifier = 1f;
    private float damageModifier = 0.2f;
    private float rangeModifier = 0.5f;
    private float fireRateModifier = 0.1f;

    void Awake() {
        sr = GetComponent<SpriteRenderer>();
    }
    
    public override StyleType GetType() {
        return StyleType.BEAM;
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

    public override float GetFireRateModifier() {
        return fireRateModifier;
    }

    public override void SetProjectileColour(Color colour) {
        sr.color = colour;
    }

    public override void PerformHitAction(Spell spell) {
        return;
    }
}
