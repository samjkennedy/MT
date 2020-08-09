using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Controller2D))]
public class Player : MonoBehaviour
{

    //Movement
    private float jumpHeight = 4.0f;
    private float lowJumpHeight = 2.0f;
    private float timeToJumpApex = 0.4f;

    private float accelerationTimeAirborne = 0.1f;
    private float accelerationTimeGrounded = 0.05f;
    private float movementSpeed = 10f;

    //Could be useful later for respawning after falling off the map?
    //Place player back at doorway?
    private Vector3 spawnPos;

    private float gravity;
    private float jumpVelocity;

    private int extraJumps = 1;
    private int remainingExtraJumps = 1;

    public Vector3 velocity;
    private float velocityXSmoothing;

    //Wall jumping
    private float wallSlideSpeedMax = 1.0f;
    private Vector2 wallJumpClimb;
    private Vector2 wallJumpOff;
    private Vector2 wallLeap;

    //Resource management
    public HealthController health; //Set in editor
    private float damageFlashTime = 1f; //Time invincible after being hit
    private float damageFlashInterval = 0.2f;
    private bool invincible;

    //Components
    Controller2D controller;
    SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        //Dirty hack, maybe fix me
        DontDestroyOnLoad(gameObject);

        controller = GetComponent<Controller2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        spawnPos = transform.position;

        wallJumpClimb = new Vector2(7.5f, 16f);
        wallJumpOff = new Vector2(8.5f, 7f);
        wallLeap = new Vector2(10f, 10f);

        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
    }

    // Update is called once per frame
    void Update()
    {
        float inputX = Input.GetAxis("Horizontal");
        Vector3 aimDirection = new Vector3(Input.GetAxis("Mouse Y"), -Input.GetAxis("Mouse X"), 0).normalized;
        if (aimDirection.magnitude > 0) {  
            if (aimDirection.x > 0 && spriteRenderer.flipX) {
                spriteRenderer.flipX = false;
            }
            if (aimDirection.x < 0 && !spriteRenderer.flipX) {
                spriteRenderer.flipX = true;
            }
        } else {
            if (inputX > 0 && spriteRenderer.flipX) {
                spriteRenderer.flipX = false;
            }
            if (inputX < 0 && !spriteRenderer.flipX) {
                spriteRenderer.flipX = true;
            }
        }
        float inputY = Input.GetAxis("Vertical");
        int wallDirX = controller.collisions.left ? -1 : 1; //Left = -1, Right = 1

        bool holdHeld = (Input.GetButton("Hold") || Input.GetAxis("Hold") != 0f);

        bool holding = holdHeld && (controller.collisions.right || controller.collisions.left) && !controller.collisions.below;
        bool hanging = holdHeld && controller.collisions.above;

        //Wall sliding
        bool wallSliding = false;
        if (holding) {
            wallSliding = true;
            remainingExtraJumps = extraJumps;
            //Limit how fast we can slide downwards
            if (velocity.y < -wallSlideSpeedMax) {
                velocity.y = -wallSlideSpeedMax;
            }
            //Stop us sliding upwards, if we grab we can't slip upwards
            if (velocity.y > 0) {
                velocity.y = 0;
            }
        }

        //Prevent gravity accumulation
        if (controller.collisions.above || controller.collisions.below) {
            velocity.y = 0;
        }

        //Reset extra jump counter
        if (controller.collisions.below) {
            remainingExtraJumps = extraJumps;
        }

        //Prevents us from acting on lateral input while "stuck" to a wall
        if (!holding) {
            float speedMultiplier = holdHeld && controller.collisions.below ? 0.6f : 1f;
            float targetXVelocity = inputX * movementSpeed * speedMultiplier;
            velocity.x = Mathf.SmoothDamp(velocity.x, targetXVelocity, ref velocityXSmoothing, controller.collisions.below ? accelerationTimeGrounded : accelerationTimeAirborne);
        }

        //Jumping
        if (Input.GetButtonDown("Jump")) {
            if (wallSliding) {
                //Change gravity
                gravity = -(2 * lowJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
                jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
                if (wallDirX == inputX) {
                    velocity.x = -wallDirX * wallJumpClimb.x;
                    velocity.y = wallJumpClimb.y;
                } else if (inputX == 0 && inputY == 0) {
                    velocity.x = -wallDirX * wallJumpOff.x;
                    velocity.y = wallJumpOff.y;
                } else {
                    velocity.x = (inputX * wallLeap.x) + 1f; //Control leap power laterally
                    velocity.y = wallLeap.y * inputY + 5f; //Aim leap Up/Down
                }
            } else if (hanging) {
                //Change gravity
                gravity = -(2 * lowJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
                jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
                velocity.y = -1f;
            } else if (controller.collisions.below) {
                //Change gravity
                gravity = -(2 * lowJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
                jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
                velocity.y = jumpVelocity;
            } else if (remainingExtraJumps > 0) {
                //Change gravity
                gravity = -(2 * lowJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
                jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
                velocity.y = jumpVelocity;
                remainingExtraJumps--;
            }
        }

        if (Input.GetButtonUp("Jump")) {
            //Reset gravity
            gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
            jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        }

        if (Input.GetKeyDown(KeyCode.H)) {
            Heal(1);
        }

        //Ceiling hanging (Doesn't fucking work yet, why??)
        if (hanging) {
            //velocity = new Vector2(0, 0);
        } else {
            //Gravity
            velocity.y += gravity * Time.deltaTime;
        }
        //Actually move based on input + gravity
        controller.Move(velocity * Time.deltaTime + (0.5f * Vector3.up * gravity * Mathf.Pow(Time.deltaTime, 2)));
    }

    public void Hit(int damage) {//Todo take in spell type, direction etc
        if (invincible) {
            return;
        }
        StartCoroutine(InvincibleFlicker());
        health.Decrease(damage);
        if (health.GetCurrentHealth() <= 0) {
            Destroy(gameObject); //TODO: better death
        }
    }

    public void Heal(int amount) {
        health.Increase(amount);
    }

    IEnumerator InvincibleFlicker() {
        invincible = true;
        Color invisible = new Color(1f, 1f, 1f, 0f);
        Color normal = new Color(1f, 1f, 1f, 1f);
        for (int i = 0; i < (damageFlashTime/damageFlashInterval); i++)
        {
            spriteRenderer.color = invisible;
            yield return new WaitForSeconds(damageFlashInterval);

            spriteRenderer.color = normal;
            yield return new WaitForSeconds(damageFlashInterval);
        }
        invincible = false;
    }
}
