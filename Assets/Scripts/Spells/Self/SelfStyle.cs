using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SelfStyle : MonoBehaviour
{
    public abstract void Cast(Player player, Element element);
    public abstract float GetEffectTime();
}
