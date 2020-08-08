using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : Element
{
    
    private float baseDamage = 15f;

    public override ElementType GetElementType() {
        return ElementType.FIRE;
    }

    public override float GetBaseDamage() {
        return baseDamage;
    }
}
