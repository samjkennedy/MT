using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : Element
{
 private float baseDamage = 10f;

    public override ElementType GetElementType() {
        return ElementType.WATER;
    }

    public override float GetBaseDamage() {
        return baseDamage;
    }
}
