using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : Enemy
{
    //Combat
    public Element element;
    public Style style;
    public Mutation mutation;
    public Spell spellPrefab;
    private Spell spell;
    private float attackSpeed = 1.5f;
    private bool attackCooldown = false;

    //Movement
    private float jumpHeight = 1.0f;
    private float timeToJumpApex = 0.55f;
    private float jumpFrequency = 2.5f;
    private float gravity;
    private float jumpVelocity;
    private bool canJump = true;

    void OnEnable() {
        StartCoroutine(ChargeAttack());
    }
 
    void Start()
    {
        base.Start();

        attackCooldown = true;
        StartCoroutine(ChargeAttack());

        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
    }

    void Update()
    {
        base.Update();
        Vector3 vectorToPlayer = player.transform.position - transform.position;
        Vector3 directionToPlayer = vectorToPlayer.normalized;

        if (!attackCooldown) {
            StartCoroutine(FireSpell(directionToPlayer));
        }

        //Move towards player
        bool tooClose = vectorToPlayer.magnitude <= 5f;

        //velocity.x = tooClose ? 0f : directionToPlayer.x * movementSpeed;

        if (velocity.x > 0 && spriteRenderer.flipX) {
            spriteRenderer.flipX = false;
        }
        if (velocity.x < 0 && !spriteRenderer.flipX) {
            spriteRenderer.flipX = true;
        }

        RaycastHit2D rc2d = Physics2D.Raycast(transform.position, Vector2.down + (Vector2.right * Mathf.Sign(directionToPlayer.x)), 2f);
        if (!rc2d) {
            velocity.x = 0;
        }

        Move(velocity * Time.deltaTime + (0.5f * Vector3.up * gravity * Mathf.Pow(Time.deltaTime, 2)));
    }

    public override ElementType[] GetImmunities() {
        return new ElementType[]{element.GetElementType()};
    }

    IEnumerator Jump() {
        canJump = false;

        velocity.y = jumpVelocity;
        Vector3 vectorToPlayer = player.transform.position - transform.position;
        Vector3 directionToPlayer = vectorToPlayer.normalized;
        float distanceToPlayer = vectorToPlayer.magnitude;
        velocity.x = distanceToPlayer < 2f ? directionToPlayer.x * -movementSpeed : directionToPlayer.x * movementSpeed;

        yield return new WaitForSeconds(jumpFrequency);
        canJump = true;
    }

    IEnumerator ChargeAttack() {
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

    public override float GetGravity() {
        return gravity;
    }
}
