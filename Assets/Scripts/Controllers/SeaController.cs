using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaController : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Character") {
            collision.gameObject.GetComponent<CharacterController>().Drown();
        }
    }

}
