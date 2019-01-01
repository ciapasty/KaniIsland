using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float maxVelocity  = 10f;
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
        if (Input.GetKey(KeyCode.W)) {
            body.AddForce(Vector2.up.normalized * moveForce);
        }

        if (Input.GetKey(KeyCode.S)) {
            body.AddForce(Vector2.down.normalized * moveForce);
        }

        if (Input.GetKey(KeyCode.A)) {
            body.AddForce(Vector2.left.normalized * moveForce);
            transform.rotation = Quaternion.Euler(0, 180f, 0);
        }

        if (Input.GetKey(KeyCode.D)) {
            body.AddForce(Vector2.right.normalized * moveForce);
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        if (characterCarried == null) {
            // Body pickup
            if (Input.GetKeyDown(KeyCode.C)) {

            }   

            // Attack/Body pickup
            if (timeBtwAttack <= 0) {
                if (Input.GetKeyDown(KeyCode.Space)) {
                    timeBtwAttack = startTimeBtwAttack;
                    if (characterToPickup != null) {
                        characterCarried = characterToPickup;
                        characterToPickup = null;
                        return;
                    }

                    Collider2D[] enemiesToHit = Physics2D.OverlapCircleAll(attackPosition.position, attackRange, characterLayerMask);
                    if (enemiesToHit.Length > 0) {
                        enemiesToHit[0].GetComponent<CharacterController>().Hit();
                    }
                }
            } else {
                timeBtwAttack -= Time.deltaTime;
            }
        } else {
            // Move Character with Player
            characterCarried.transform.position = new Vector3(transform.position.x, transform.position.y+0.5f, 0);
        }

        render.sortingOrder = Mathf.RoundToInt(transform.position.y * 100f) * -1;
        body.velocity = Vector2.ClampMagnitude(body.velocity, maxVelocity);
    }

    public bool IsCarryingCharacter() {
        return characterCarried != null;
    }

    public void DropCharacter() {
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
