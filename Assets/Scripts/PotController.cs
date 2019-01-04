using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotController : MonoBehaviour {

    public string playerTag;
    public GameObject ipsGO;

    private IngredientsController ips;
    private List<GameObject> charactersInPot;

    void Awake() {
        ips = ipsGO.GetComponent<IngredientsController>();
        ips.pot = this;
    }

    private void Start() {
        charactersInPot = new List<GameObject>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == playerTag) {
            PlayerController pc = collision.GetComponent<PlayerController>();
            if (pc.IsCarryingCharacter()) {
                IngredientDelivered(pc.DropCharacterToPot());
            }
        }
    }
    
    private void IngredientDelivered(GameObject character) {
        // Set character GO in pot
        character.GetComponent<Animator>().SetTrigger("GoIntoPot");
        character.GetComponent<SpriteRenderer>().sortingOrder = 0;
        character.transform.position =
            new Vector3(transform.position.x + Random.Range(-0.3f, 0.3f), transform.position.y + 0.4f, 0);
        charactersInPot.Add(character);

        ips.IngredientDelivered(character);
    }

    public void RemoveCharactersFromPot() {
        if (charactersInPot == null || charactersInPot.Count == 0)
            return;

        for (int i = 0; i < charactersInPot.Count; i++) {
            Destroy(charactersInPot[i]);
        }
        charactersInPot = new List<GameObject>();
    }
}
