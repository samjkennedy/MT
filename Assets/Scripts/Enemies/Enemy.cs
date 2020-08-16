using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;

[RequireComponent (typeof (SpriteRenderer))]
public abstract class Enemy : PhysicsObject
{

    public GameObject floatingTextPrefab;
    public int health;

    public Vector3 spawnPosition;

    public Player player;

    private float knockBack = 1f;

    //Particle effects, might be a better way of handling this down the line?
    public ParticleController particleControllerPrefab;
    private ParticleController particleController;

    //Universal movement stuff
    public float movementSpeed = 2f;

    //statuses - add more as need be
    public bool onFire;
    private bool burning;
    public bool frozen;
    private bool slow;
    public bool isPoisoned;
    private bool poisoned;
    public bool wet;
    private bool isWet;

    //Components & component paraphernalia
    public SpriteRenderer spriteRenderer; 
    private Color originalColour;

    public override void Start()
    {
        base.Start();

        spriteRenderer = GetComponent<SpriteRenderer>();
        spawnPosition = transform.position;
        GameObject playerGameObj = GameObject.Find("Player");
        player = playerGameObj.GetComponent<Player>();
        particleController = Instantiate(particleControllerPrefab, transform);
        originalColour = spriteRenderer.color;
    }

    public override void Update()
    {
        base.Update();

        if (!burning && onFire) {
            StartCoroutine(Burn());
        }

        if (!slow && frozen) {
            StartCoroutine(Freeze());
        }

        if (!poisoned && isPoisoned) {
            StartCoroutine(Poison());
        }

        if (!isWet && wet) {
            StartCoroutine(Damp());
        }
    }

    public virtual ElementType[] GetImmunities() {
        return new ElementType[0];
    }

    public virtual void Hit(Spell spell) {
        if (ArrayUtility.Contains(GetImmunities(), spell.Element.GetElementType())) {
            TakeDamage(spell.GetDamage() / 2f);
            return;
        }
        TakeDamage(spell.GetDamage());
        
        Hit(spell.Element);

        //Knockback
        //TODO reenable when the spell style confers momentum too
        //AddVelocity(spell.Velocity.normalized * 3f + (Vector3.up)); //hard coding mass 
    }

    public virtual bool IsKillable() {
        return true;
    }

    //TODO different enemies will deal with statuses differently, maybe more flags? isImmuneToFire?
    public void Hit(Element element) {
        ElementType elementType = element.GetElementType();
        switch (elementType)
        {
            case ElementType.ARCANA:
                break;
            case ElementType.FIRE:
                if (!wet) {
                    onFire = true;
                }
                break;
            case ElementType.CORRUPTION:
                isPoisoned = true;
                break;
            case ElementType.FROST:
                if (!onFire) {
                    frozen = true;
                }
                break;
            case ElementType.WATER:
                onFire = false;
                wet = true;
                break;
            default:
                Debug.Log(elementType);
                break;
        }
    }

    public void TakeDamage(float damage) {
        int damageDealt = (int) Mathf.Ceil(damage);
        if (damageDealt <= 0) {
            return;
        }
        ShowDamagePopup(damageDealt);
        health -= damageDealt;
        if (health <= 0) {
            Die(); //TODO better deaths - animations?
        }
    }

    public virtual void Die() {
        //Drop loot
        if (Random.Range(0, 15) > 13) {
            HealthPotion healthPotion = Instantiate(LootController.instance.healthPotionPrefab, transform.position, Quaternion.identity, transform.parent);
            Vector3 potionVelocity = Random.insideUnitSphere;
            healthPotion.velocity = new Vector3(potionVelocity.x, Mathf.Abs(potionVelocity.y) + 1, 0) * 3f;
        }
        for (int i = 0; i < Random.Range(0, 15); i++)
        {
            Coin coin = Instantiate(LootController.instance.coinPrefab, transform.position, Quaternion.identity, transform.parent);
            Vector3 coinVelocity = Random.insideUnitSphere;
            coin.velocity = new Vector3(coinVelocity.x, Mathf.Abs(coinVelocity.y) + 1, 0) * 3f;
        }
        Destroy(gameObject); //TODO better deaths - animations?
    }

    private void ShowDamagePopup(int damage) {
        if (floatingTextPrefab == null) {
            return;
        }
        GameObject floatingText = Instantiate(floatingTextPrefab, transform.position, Quaternion.identity, transform);
        floatingText.GetComponent<TextMeshPro>().text = "-" + damage.ToString() + "HP";
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

    IEnumerator Damp() {
        burning = false;
        particleController.PlayEffect(ElementType.WATER);
        float originalMovementSpeed = movementSpeed;
        movementSpeed = movementSpeed/2f;
        //TODO: make configurable
        for (int i = 0; i < 10; i++) 
        {
            yield return new WaitForSeconds(0.5f);
            if (!wet) {
                break;
            }
        } 
        particleController.StopEffect(ElementType.WATER);
        wet = false;
        isWet = false;
        movementSpeed = originalMovementSpeed;
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
}
