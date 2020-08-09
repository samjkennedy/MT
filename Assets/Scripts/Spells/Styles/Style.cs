using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Style : MonoBehaviour
{
    public abstract Vector3 GetNextPos(ref Vector3 velocity, float deltaTime);
    public abstract float GetSpeedModifier();
    public abstract float GetDamageModifier();
    public abstract float GetRangeModifier();
    public abstract float GetFireRateModifier();

    public abstract StyleType GetType();

    public abstract void PerformHitAction(Spell spell);

    //Unused
    public abstract void SetProjectileColour(Color colour);
}
