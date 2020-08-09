using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detonation : Mutation
{

    public ParticleController particleControllerPrefab;
    private ParticleController particleController;
    private float duration = 1.5f;
    private float radius = 1f;

    void Awake()
    {
        particleController = Instantiate(particleControllerPrefab, transform);
    }

    public override void PerformAfterEffect(Spell spell, string hitTag) {
        particleController.PlayExplosion(spell.Element.GetElementType());

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.tag == hitTag) {
                Enemy enemy = hitCollider.GetComponent<Enemy>();
                enemy.Hit(spell);
            }
        }

        Destroy(gameObject, duration);
    }
}
