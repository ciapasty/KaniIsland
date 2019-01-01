using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaController : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player") {
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.up * 20;
        } else if (collision.tag == "Character") {
            collision.gameObject.GetComponent<CharacterController>().Drown();
        }
    }

}
