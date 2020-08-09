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

    public float spellSpeedModifier;
    public string hitTag;

    Controller2D controller;

    void Start()
    {
        controller = GetComponent<Controller2D>();
    }

    void Update()
    {
        style.GetNextPos(ref velocity, Time.deltaTime);
        if (mutation != null) {
            mutation.AlterPath(ref velocity, Time.deltaTime);
        }
        controller.Move(velocity * Time.deltaTime);

        if (controller.collisions.any) {
            RaycastHit2D hit = controller.collisions.hit;
            if (hit.transform.tag == hitTag) {
                if (hitTag == "Enemy") {
                    Enemy enemy = hit.collider.GetComponent<Enemy>();
                    enemy.Hit(this);
                } else if (hitTag == "Player") {
                    Player player = hit.collider.GetComponent<Player>();
                    player.Hit(1);
                }
                if (mutation != null) {
                    mutation.PerformAfterEffect(this, hitTag);
                }
                style.PerformHitAction(this);
            } else if (hit.transform.tag == "Obstacles") {
                if (mutation != null) {
                    mutation.PerformAfterEffect(this, hitTag);
                }
                style.PerformHitAction(this);
            }
        }
    }

    public void Fire(Element elementPrefab, Style stylePrefab, Mutation mutationPrefab, Vector3 direction) {
        style = Instantiate(stylePrefab, transform);
        element = Instantiate(elementPrefab, transform);
        this.hitTag = hitTag;
        if (mutationPrefab != null) {
            mutation = Instantiate(mutationPrefab, transform);
        }

        Destroy(gameObject, element.GetLifetime() * style.GetRangeModifier());
        velocity = direction * element.GetSpeed() * style.GetSpeedModifier() * spellSpeedModifier;
    }

    void OnDisable() {
        element.transform.parent = null;
        style.PerformHitAction(this);
        if (mutation != null) {
            mutation.transform.parent = null;
            mutation.PerformAfterEffect(this, hitTag);
        }
        Destroy(element.gameObject, element.GetLifetime());
    }

    public float GetDamage() {
        return element.GetBaseDamage() * style.GetDamageModifier();
    }
    
}
