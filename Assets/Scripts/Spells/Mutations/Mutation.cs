using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Mutation : MonoBehaviour
{
    public abstract void PerformAfterEffect(Spell spell);
}
