using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Controller2D))]
public class Spell : MonoBehaviour
{
    private Element element;
    public Element Element => element;
    private Style style;
    public Style Style => style;
    private Mutation mutation;
    public Mutation Mutation => mutation;
    private Vector3 velocity;
    public Vector3 Velocity => velocity;

    Controller2D controller;

    void Start()
    {
        controller = GetComponent<Controller2D>();
    }

    void Update()
    {
        controller.Move(style.GetNextPos(ref velocity, Time.deltaTime));

        if (controller.collisions.any) {
            RaycastHit2D hit = controller.collisions.hit;
            if (hit.transform.tag == "Enemy") {
                Enemy enemy = hit.collider.GetComponent<Enemy>();
                enemy.Hit(this);
            }
            style.PerformHitAction(this);
            if (mutation != null) {
                mutation.PerformAfterEffect(this);
            }
            Destroy(gameObject);
        }
    }

    public void Fire(Element elementPrefab, Style stylePrefab, Mutation mutationPrefab, Vector3 direction) {
        style = Instantiate(stylePrefab, transform);
        element = Instantiate(elementPrefab, transform);
        if (mutationPrefab != null) {
            mutation = Instantiate(mutationPrefab, transform);
        }

        Destroy(gameObject, element.GetLifetime() * style.GetRangeModifier());
        velocity = direction * element.GetSpeed() * style.GetSpeedModifier();
    }

    void OnDisable() {
        element.transform.parent = null;
        style.PerformHitAction(this);
        if (mutation != null) {
            mutation.transform.parent = null;
            mutation.PerformAfterEffect(this);
        }
        Destroy(element.gameObject, element.GetLifetime());
    }

    public float GetDamage() {
        return element.GetBaseDamage() * style.GetDamageModifier();
    }
    
}
