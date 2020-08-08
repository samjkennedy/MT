using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Earth : Element
{
    private float baseDamage = 13f;

    public override ElementType GetElementType() {
        return ElementType.EARTH;
    }

    public override float GetBaseDamage() {
        return baseDamage;
    }
}
