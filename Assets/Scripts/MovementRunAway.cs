﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementRunAway : MonoBehaviour {

    public float perceptionRadius = 2;
    public float maxForce = 50;

    public LayerMask dangerLayerMask;

    Rigidbody2D body;
    
    void Start() {
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update() {
        Vector2 runAway = new Vector2();

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(gameObject.transform.position, perceptionRadius, dangerLayerMask);
        foreach (var coll in hitColliders) {
            float d = coll.Distance(this.gameObject.GetComponent<BoxCollider2D>()).distance; //Vector2.Distance(gameObject.transform.position, coll.gameObject.transform.position);
            if (d > 0) {
                Vector2 diff = gameObject.transform.position - coll.gameObject.transform.position;
                diff /= (d * d * d);
                runAway += diff;
            }
        }

        body.AddForce(Vector2.ClampMagnitude(runAway * maxForce, maxForce));
    }

    // Gizmos

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, perceptionRadius);
    }

}
