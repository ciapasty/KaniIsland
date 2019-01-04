using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotController : MonoBehaviour {

    public string playerTag;

    private int charactersInPot = 0;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == playerTag) {
            PlayerController pc = collision.GetComponent<PlayerController>();
            if (pc.IsCarryingCharacter()) {
                pc.DropCharacterToPot();
                charactersInPot++;
                Debug.Log("Charcters in "+ playerTag +" pot: " + charactersInPot);
            }
        }
    }
}
