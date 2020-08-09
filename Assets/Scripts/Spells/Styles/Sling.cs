using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sling : Style
{

    private static float fireHeight = 1f;
    private static float timeToApex = 0.4f;

    private static float gravity = -(2 * fireHeight) / Mathf.Pow(timeToApex, 2);
    public static float Gravity => gravity;

    private SpriteRenderer sr;

    private float speedModifier = 1f;
    private float damageModifier = 1.5f;
    private float rangeModifier = 2f;
    private float fireRateModifier = 3f;

    public Detonation detonationPrefab;
    private Detonation detonation;

    void Awake() {
        sr = GetComponent<SpriteRenderer>();
    }
    
    public override StyleType GetType() {
        return StyleType.SLING;
    }

    public override Vector3 GetNextPos(ref Vector3 velocity, float deltaTime) {
        velocity.y += gravity * deltaTime;
        return velocity * deltaTime + (0.5f * Vector3.up * gravity * Mathf.Pow(deltaTime, 2));
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
        detonation = Instantiate(detonationPrefab, transform.position, Quaternion.identity);
        detonation.PerformAfterEffect(spell, spell.hitTag);
        Destroy(spell);
    }
}
