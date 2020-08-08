using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corruption : Element
{
    private float baseDamage = 7f;

    public override ElementType GetElementType() {
        return ElementType.CORRUPTION;
    }

    public override float GetBaseDamage() {
        return baseDamage;
    }
}
