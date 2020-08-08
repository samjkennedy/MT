using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arcana : Element
{

    private float baseDamage = 10f;

    public override ElementType GetElementType() {
        return ElementType.ARCANA;
    }

    public override float GetBaseDamage() {
        return baseDamage;
    }

}
