using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Element : MonoBehaviour
{
    public float lifeTime;
    public float speed;
    public float fireRate;

    public abstract ElementType GetElementType();

    public abstract float GetBaseDamage();

    public float GetLifetime() {
        return lifeTime;
    }

    public float GetSpeed() {
        return speed;
    }

    public float GetFireRate() {
        return fireRate;
    }

    //TODO: Put particle effects on each element instead of on the spell
}
