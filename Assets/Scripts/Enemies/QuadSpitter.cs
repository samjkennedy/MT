using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadSpitter : Enemy
{
    //Combat
    public Element element;
    public Style style;
    public Mutation mutation;
    public Spell spellPrefab;
    private Spell spell;
    private float attackSpeed = 1.5f;
    private bool attackCooldown = false;

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

        if (!attackCooldown) {
            StartCoroutine(FireSpell());
        }
    }

    IEnumerator ChargeUp() {
        attackCooldown = true;
        yield return new WaitForSeconds(attackSpeed);
        attackCooldown = false;
    }
    
    IEnumerator FireSpell() {
        attackCooldown = true;

        Spell spell = Instantiate(spellPrefab, transform.position, Quaternion.identity);
        spell.Fire(element, style, mutation, Vector3.up);
        spell = Instantiate(spellPrefab, transform.position, Quaternion.identity);
        spell.Fire(element, style, mutation, Vector3.right);
        spell = Instantiate(spellPrefab, transform.position, Quaternion.identity);
        spell.Fire(element, style, mutation, Vector3.down);
        spell = Instantiate(spellPrefab, transform.position, Quaternion.identity);
        spell.Fire(element, style, mutation, Vector3.left);
        yield return new WaitForSeconds(attackSpeed);
        spell = Instantiate(spellPrefab, transform.position, Quaternion.identity);
        spell.Fire(element, style, mutation, Vector3.up + Vector3.right);
        spell = Instantiate(spellPrefab, transform.position, Quaternion.identity);
        spell.Fire(element, style, mutation, Vector3.right + Vector3.down);
        spell = Instantiate(spellPrefab, transform.position, Quaternion.identity);
        spell.Fire(element, style, mutation, Vector3.down + Vector3.left);
        spell = Instantiate(spellPrefab, transform.position, Quaternion.identity);
        spell.Fire(element, style, mutation, Vector3.left + Vector3.up);
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
