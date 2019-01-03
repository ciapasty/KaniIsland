using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public string player = "P1";

    public float maxVelocity = 10f;
    public float moveForce = 2f;

    private float timeBtwAttack;
    public float startTimeBtwAttack;

    public Transform attackPosition;
    public float attackRange;
    public LayerMask characterLayerMask;

    private GameObject characterToPickup;
    private GameObject characterCarried;

    Animator anim;
    Rigidbody2D body;
    SpriteRenderer render;

    void Start() {
        anim = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
        render = GetComponent<SpriteRenderer>();
    }

    void Update() {
        // Animation
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
                        characterCarried = characterToPickup;
                        characterToPickup = null;
                        return;
                    }

                    Collider2D[] enemiesToHit = Physics2D.OverlapCircleAll(attackPosition.position, attackRange, characterLayerMask);
                    if (enemiesToHit.Length > 0) {
                        enemiesToHit[0].GetComponent<CharacterController>().Hit();
                    }
                } else {
                    characterCarried.transform.position = transform.position;
                    characterToPickup = characterCarried;
                    characterCarried = null;
                }
            }
        } else {
            timeBtwAttack -= Time.deltaTime;
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

    public void DropCharacterToPot() {
        if (characterCarried == null) {
            Debug.LogError("DropCharacter called without carried character");
            return;
        }

        Destroy(characterCarried);
        characterCarried = null;
    }

    // Circle Collider - Picking up characters

    private void OnTriggerEnter2D(Collider2D collision) {
        CharacterController cController = collision.GetComponent<CharacterController>();
        if (characterToPickup == null && cController != null && cController.IsDead()) {
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
