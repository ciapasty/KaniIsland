using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour {

    public float maxVelocity;

    Animator anim;
    Rigidbody2D body;
    SpriteRenderer render;

    MovementFlock flock;
    MovementRunAway runAway;

    void Start() {
        anim = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
        render = GetComponent<SpriteRenderer>();
        flock = GetComponent<MovementFlock>();
        runAway = GetComponent<MovementRunAway>();
    }
    
    void Update() {
        body.velocity = Vector2.ClampMagnitude(body.velocity, maxVelocity);
    }

    public void Hit() {
        flock.enabled = false;
        runAway.enabled = false;
        gameObject.layer = 9;
    }
}
