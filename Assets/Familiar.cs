using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Familiar : MonoBehaviour
{

    private Player player;

    public Element element;
    public Style style;
    public Spell spellPrefab;

    private Spell spell;
    private float attackSpeed = 1.5f;
    private bool attackCooldown = false;

    public float radius = 10f;

    private Vector3 sdVelocity;

    void Start()
    {
        GameObject playerGameObj = GameObject.Find("Player");
        player = playerGameObj.GetComponent<Player>();

        attackCooldown = true;
        StartCoroutine(ChargeAttack());
    }

    void Update()
    {
        if (player == null) {
            return;
        }

        Vector3 wiggle = Random.insideUnitCircle * 0.3f;
        transform.position = Vector3.SmoothDamp(transform.position, (player.transform.position + Vector3.up + player.lookDirection) + new Vector3(wiggle.x, wiggle.y, 0) , ref sdVelocity, 1f / 3f);

        Enemy nearestEnemy = null;
        Vector3 vectorToNearestEnemy = Vector3.positiveInfinity;

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.tag == "Enemy") {
                Enemy enemy = hitCollider.GetComponent<Enemy>();
                if (enemy.IsKillable() && (enemy.transform.position - transform.position).magnitude < vectorToNearestEnemy.magnitude) {
                    nearestEnemy = enemy;
                    vectorToNearestEnemy = enemy.transform.position - transform.position;
                }
            }
        }

        if (!attackCooldown && nearestEnemy != null) {
            StartCoroutine(FireSpell(vectorToNearestEnemy.normalized));
        }
    }

    IEnumerator ChargeAttack() {
        yield return new WaitForSeconds(attackSpeed);
        attackCooldown = false;
    }

    IEnumerator FireSpell(Vector3 direction) {
        attackCooldown = true;

        Spell spell = Instantiate(spellPrefab, transform.position, Quaternion.identity);
        spell.Fire(element, style, null, direction);
        yield return new WaitForSeconds(attackSpeed);

        attackCooldown = false;
    }
}
