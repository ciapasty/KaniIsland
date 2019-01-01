using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementFlock : MonoBehaviour {

    public float perceptionRadius = 2;
    public float maxForce = 50;

    public LayerMask characterLayerMask;

    Rigidbody2D body;


    void Start() {
        body = GetComponent<Rigidbody2D>();
    }
    
    void Update() {
        Vector2 acceleration = new Vector2();
        Vector2 alignment = new Vector2();
        Vector2 cohesion = new Vector2();
        Vector2 separation = new Vector2();

        int total = 0;

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(gameObject.transform.position, perceptionRadius, characterLayerMask);
        foreach (var coll in hitColliders) {
            if (coll.gameObject == this.gameObject)
                continue;

            if (coll.gameObject.tag == "Character") {
                Vector2 collVelocity = coll.GetComponent<Rigidbody2D>().velocity;
                float d = Vector2.Distance(gameObject.transform.position, coll.gameObject.transform.position);

                alignment += collVelocity;
                cohesion += new Vector2(coll.gameObject.transform.position.x, coll.gameObject.transform.position.y);

                // Separation
                Vector2 diff = gameObject.transform.position - coll.gameObject.transform.position;
                diff /= (d * d * d);
                separation += diff;

                total++;
            }
        }

        if (total > 0) {
            alignment /= total;
            alignment *= maxForce;
            alignment -= body.velocity;
            alignment = Vector2.ClampMagnitude(alignment, maxForce);

            cohesion /= total;
            cohesion -= new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
            cohesion *= maxForce;
            cohesion -= body.velocity;
            cohesion = Vector2.ClampMagnitude(cohesion, maxForce);

            separation /= total;
            separation *= maxForce;
            separation -= body.velocity;
            separation = Vector2.ClampMagnitude(separation, maxForce);
        }

        acceleration += alignment;
        acceleration += cohesion;
        acceleration += separation * 1.25f;
        acceleration = Vector2.ClampMagnitude(acceleration, maxForce);

        body.AddForce(acceleration);
    }

    // Gizmos

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, perceptionRadius);
    }

}