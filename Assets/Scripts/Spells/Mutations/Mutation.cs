using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Mutation : MonoBehaviour
{
    public virtual void PerformAfterEffect(Spell spell, string hitTag) {
        return;
    }

    public virtual void AlterPath(ref Vector3 velocity, float deltaTime) {
        return;
    }
}
