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

        // Attack
        if (timeBtwAttack <= 0) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                timeBtwAttack = startTimeBtwAttack;
                Collider2D[] enemiesToHit = Physics2D.OverlapCircleAll(attackPosition.position, attackRange, characterLayerMask);
                for (int i = 0; i < enemiesToHit.Length; i++) {
                    enemiesToHit[i].GetComponent<CharacterController>().Hit();
                }
            }
        } else {
            timeBtwAttack -= Time.deltaTime;
        }

        body.velocity = Vector2.ClampMagnitude(body.velocity, maxVelocity);
    }

    // Gizmos

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPosition.position, attackRange);
    }
}
