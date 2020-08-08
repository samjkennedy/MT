using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (SpriteRenderer))]
public class Enemy : PhysicsObject
{

    public int health;

    private Color originalColour;
    private SpriteRenderer sr; 

    private Player player;

    //Combat
    private int damage = 1;//half a heart
    private float attackSpeed = 1f; //once per second
    private bool attackCooldown = false;

    //TODO: Move all the movement code into concrete enemies
    private float movementSpeed = 2f;
    private float jumpHeight = 1.0f;
    private float timeToJumpApex = 0.55f;
    private float jumpFrequency = 2.5f;
    private float gravity;
    private float jumpVelocity;
    private bool canJump = true;

    private float knockBack = 1f;

    //Particle effects, might be a better way of handling this down the line?
    public ParticleController particleControllerPrefab;
    private ParticleController particleController;
    //statuses - add more as need be
    public bool onFire;
    private bool burning;
    public bool frozen;
    private bool slow;
    public bool isPoisoned;
    private bool poisoned;

    public override void Start()
    {
        base.Start();

        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;

        sr = GetComponent<SpriteRenderer>();
        player = FindObjectOfType<Player>();
        particleController = Instantiate(particleControllerPrefab, transform);
        originalColour = sr.color;
    }

    public override void Update()
    {
        base.Update();
        if (canJump && controller.collisions.below) {
            StartCoroutine(Jump());
        }

        if (!burning && onFire) {
            StartCoroutine(Burn());
        }

        if (!slow && frozen) {
            StartCoroutine(Freeze());
        }

        if (!poisoned && isPoisoned) {
            StartCoroutine(Poison());
        }

        if (!attackCooldown && controller.collisions.any) {
            RaycastHit2D hit = controller.collisions.hit;
            if (hit.transform == player.transform) {
                StartCoroutine(HitPlayer());
            }
        }

        Move(velocity * Time.deltaTime + (0.5f * Vector3.up * gravity * Mathf.Pow(Time.deltaTime, 2)));
    }

    public void Hit(Spell spell) {
        TakeDamage(spell.GetDamage());
        
        Hit(spell.Element);

        //Knockback
        AddVelocity(spell.Velocity.normalized * 3f + (Vector3.up)); //hard coding mass 
    }

    public void Hit(Element element) {
        ElementType elementType = element.GetElementType();
        switch (elementType)
        {
            case ElementType.ARCANA:
                Debug.Log("Nothing!");
                break;
            case ElementType.FIRE:
                Debug.Log("Burning!");
                onFire = true;
                break;
            case ElementType.CORRUPTION:
                isPoisoned = true;
                Debug.Log("Poison!");
                break;
            case ElementType.FROST:
                Debug.Log("Frost!");
                if (!onFire) {
                    frozen = true;
                }
                break;
            default:
                Debug.Log(elementType);
                break;
        }
    }

    void TakeDamage(float damage) {
        health -= (int) Mathf.Ceil(damage);
        if (health <= 0) {
            Destroy(gameObject);
        }
    }

    IEnumerator Burn() {
        burning = true;
        frozen = false;
        particleController.PlayEffect(ElementType.FIRE);
        //TODO: make configurable
        for (int i = 0; i < 10; i++) 
        {
            TakeDamage(1f);
            yield return new WaitForSeconds(0.5f);
            if (!onFire) {
                break;
            }
        } 
        particleController.StopEffect(ElementType.FIRE);
        onFire = false;
        burning = false;
    }

    IEnumerator Freeze() {
        slow = true;
        particleController.PlayEffect(ElementType.FROST);
        float originalMovementSpeed = movementSpeed;
        movementSpeed = movementSpeed/2f;
        //TODO: make configurable
        for (int i = 0; i < 25; i++) 
        {
            yield return new WaitForSeconds(0.2f);
            if (!frozen) {
                break;
            }
        } 
        particleController.StopEffect(ElementType.FROST);
        frozen = false;
        slow = false;
        movementSpeed = originalMovementSpeed;
    }

    IEnumerator Poison() {
        poisoned = true;
        particleController.PlayEffect(ElementType.CORRUPTION);
        //TODO: make configurable
        for (int i = 0; i < 10; i++) 
        {
            TakeDamage(2f);
            yield return new WaitForSeconds(0.5f);
            if (!isPoisoned) {
                break;
            }
        } 
        particleController.StopEffect(ElementType.CORRUPTION);
        poisoned = false;
        isPoisoned = false;
    }

    public override float GetGravity() {
        return gravity;
    }

    IEnumerator Jump() {
        canJump = false;

        velocity.y = jumpVelocity;
        Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
        velocity.x = directionToPlayer.x * movementSpeed;

        yield return new WaitForSeconds(jumpFrequency);
        canJump = true;
    }

    private IEnumerator HitPlayer() {
        player.Hit(damage);
        attackCooldown = true;
        yield return new WaitForSeconds(attackSpeed);
        attackCooldown = false;
    }
}
