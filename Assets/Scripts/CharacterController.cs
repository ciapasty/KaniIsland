using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour {

    public float maxVelocity;

    private bool isDead = false;

    // Components
    Animator anim;
    Rigidbody2D body;
    BoxCollider2D boxCollider;
    SpriteRenderer render;

    // Movement components
    MovementFlock flock;
    MovementRunAway runAway;

    void Start() {
        anim = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        render = GetComponent<SpriteRenderer>();
        flock = GetComponent<MovementFlock>();
        runAway = GetComponent<MovementRunAway>();

        maxVelocity = Random.Range(1.5f, 3);
        GetComponent<MovementFlock>().perceptionRadius = Random.Range(1.5f, 2.5f);
        GetComponent<MovementRunAway>().maxForce = Random.Range(10, 45);
    }
    
    void Update() {
        if (!isDead) {
            // Walking animation
            if (Mathf.Abs(body.velocity.magnitude) > 0.2) {
                anim.SetBool("isWalking", true);
            } else {
                anim.SetBool("isWalking", false);
            }

            // Sprite flip
            if (body.velocity.x >= 0) {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            } else {
                transform.rotation = Quaternion.Euler(0, 180f, 0);
            }

            body.velocity = Vector2.ClampMagnitude(body.velocity, maxVelocity);
            render.sortingOrder = Mathf.RoundToInt(transform.position.y * 100f) * -1;
        }
    }

    public void Drown() {
        KillCharacter();
        anim.SetTrigger("Drown");
    }

    public void Destroy() {
        Destroy(this.gameObject);
    }

    public void Hit() {
        KillCharacter();
        GetComponent<ParticleSystem>().Play();
        anim.SetTrigger("Kill");
    }

    public void KillCharacter() {
        isDead = true;
        flock.enabled = false;
        runAway.enabled = false;
        boxCollider.isTrigger = true;
        gameObject.layer = 9;
    }

    public bool IsDead() {
        return isDead;
    }
}
