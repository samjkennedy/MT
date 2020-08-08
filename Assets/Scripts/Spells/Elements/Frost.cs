using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frost : Element
{

    private float baseDamage = 9f;

    public override ElementType GetElementType() {
        return ElementType.FROST;
    }

    public override float GetBaseDamage() {
        return baseDamage;
    }

}
