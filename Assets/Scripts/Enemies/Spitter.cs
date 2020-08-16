using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spitter : Enemy
{
    //Combat
    public Element element;
    public Style style;
    public Mutation mutation;
    public Spell spellPrefab;
    private Spell spell;
    private float attackSpeed = 0.75f;
    private bool attackCooldown = false;

    public bool aimAtPlayer;
    public Vector3 attackDirection;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        StartCoroutine(ChargeUp());
    }

    void OnEnable() {
        StartCoroutine(ChargeUp());
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();

        if (attackCooldown) {
            return;
        }

        if (aimAtPlayer) {
            Vector3 vectorToPlayer = player.transform.position - transform.position;
            Vector3 directionToPlayer = vectorToPlayer.normalized;

            StartCoroutine(FireSpell(directionToPlayer));
        } else {
            StartCoroutine(FireSpell(attackDirection));
        }
    }

    IEnumerator ChargeUp() {
        attackCooldown = true;
        yield return new WaitForSeconds(attackSpeed);
        attackCooldown = false;
    }
    
    IEnumerator FireSpell(Vector3 direction) {
        attackCooldown = true;

        Spell spell = Instantiate(spellPrefab, transform.position, Quaternion.identity);
        spell.Fire(element, style, mutation, direction);
        yield return new WaitForSeconds(attackSpeed);

        attackCooldown = false;
    }

    public override void Hit(Spell spell) {
        TakeDamage(0);
    }

    public override bool IsKillable() {
        return false;
    }

    public override float GetGravity() {
        return 0f;
    }
}
