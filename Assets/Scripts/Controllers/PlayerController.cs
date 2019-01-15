using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : PausableBehaviour {

    public string player = "P1";

    public float maxVelocity = 10f;
    public float moveForce = 2f;

    private float timeBtwAttack;
    public float startTimeBtwAttack;

    private float timeStunned;
    public float startTimeStunned;

    public Transform attackPosition;
    public float attackRange;
    public LayerMask characterLayerMask;

    public AudioClip attackSound;
    public AudioClip pickupSound;

    private GameObject characterToPickup;
    private GameObject characterCarried;

    private Animator anim;
    private Rigidbody2D body;
    private SpriteRenderer render;
    private AudioSource audioSource;
    
    void Start() {
        anim = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
        render = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update() {
        if (!isGamePaused) {
            if (timeStunned <= 0) {

                anim.SetBool("isStunned", false);

                //  Walking animation
                if (body.velocity.magnitude > 0.3f) {
                    anim.SetBool("isWalking", true);
                } else {
                    anim.SetBool("isWalking", false);
                }

                // Movement
                if (Input.GetButton(player + "_Up")) {
                    body.AddForce(Vector2.up.normalized * moveForce);
                }

                if (Input.GetButton(player + "_Down")) {
                    body.AddForce(Vector2.down.normalized * moveForce);
                }
                if (Input.GetButton(player + "_Left")) {
                    body.AddForce(Vector2.left.normalized * moveForce);
                    transform.rotation = Quaternion.Euler(0, 180f, 0);
                }

                if (Input.GetButton(player + "_Right")) {
                    body.AddForce(Vector2.right.normalized * moveForce);
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                }


                // Attack/Body pickup
                if (timeBtwAttack <= 0) {
                    if (Input.GetButtonDown(player + "_Action")) {
                        timeBtwAttack = startTimeBtwAttack;
                        if (characterCarried == null) {
                            if (characterToPickup != null) {
                                PickupCharacter();
                                return;
                            }

                            anim.SetTrigger("attack");
                        } else {
                            DropCharacter();
                        }
                    }
                } else {
                    timeBtwAttack -= Time.deltaTime;
                }
            } else {
                timeStunned -= Time.deltaTime;
            }
        }

        if (characterCarried != null) {
            // Move Character with Player
            characterCarried.transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, 0);
        }
    }

    void FixedUpdate() {
        body.velocity = Vector2.ClampMagnitude(body.velocity, maxVelocity);
    }

    public bool IsCarryingCharacter() {
        return characterCarried != null;
    }

    public void PickupCharacter() {
        characterCarried = characterToPickup;
        characterCarried.GetComponent<NpcController>().isPickable = false;
        characterToPickup = null;
        audioSource.PlayOneShot(pickupSound);
    }

    public void DropCharacter() {
        if (characterCarried != null) {
            characterCarried.transform.position = transform.position;
            characterCarried.GetComponent<NpcController>().isPickable = true;
            characterToPickup = characterCarried;
            characterCarried = null;
        }
    }

    public GameObject DropCharacterToPot() {
        if (characterCarried == null) {
            Debug.LogError("DropCharacter called without carried character");
            return null;
        }
        GameObject c = characterCarried;
        characterCarried = null;
        return c;
    }

    public void Hit() {
        timeStunned = startTimeStunned;
        DropCharacter();
        anim.SetBool("isStunned", true);
    }

    public void OnAttack() {
        audioSource.PlayOneShot(attackSound);
        Collider2D[] enemiesToHit = Physics2D.OverlapCircleAll(attackPosition.position, attackRange);
        if (enemiesToHit.Length > 0) {
            for (int i = 0; i < enemiesToHit.Length; i++) {
                if (enemiesToHit[i].gameObject != this.gameObject) {
                    NpcController cc = enemiesToHit[i].GetComponent<NpcController>();
                    PlayerController pc = enemiesToHit[i].GetComponent<PlayerController>();
                    if (cc != null) {
                        cc.Hit();
                        break;
                    } else if (pc != null) {
                        pc.Hit();
                        break;
                    }
                }
            }
        }
    }

    // Circle Collider - Picking up characters

    private void OnTriggerEnter2D(Collider2D collision) {
        NpcController cController = collision.GetComponent<NpcController>();
        if (characterToPickup == null && cController != null &&
                cController.IsDead() && cController.isPickable) {
            characterToPickup = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject == characterToPickup) {
            characterToPickup = null;
        }
    }

    // Gizmos

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPosition.position, attackRange);
    }
}
