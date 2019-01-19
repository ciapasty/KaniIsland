using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotController : PausableBehaviour {

    public GameObject ipsGO;
    public SoupData soupData;

    private ParticleSystem[] particleSystems;
    private List<GameObject> charactersInPot;

    private void Start() {
        particleSystems = GetComponentsInChildren<ParticleSystem>();
        charactersInPot = new List<GameObject>();

        soupData.GetNewIngredients();
    }

    public void RemoveCharactersFromPot() {
        if (charactersInPot == null || charactersInPot.Count == 0)
            return;

        for (int i = 0; i < charactersInPot.Count; i++) {
            Destroy(charactersInPot[i]);
        }
        charactersInPot = new List<GameObject>();
    }

    public void Explode() {
        GetParticleSystem("Explode Particles").Play();
        GetComponent<AudioSource>().Play();
        Camera.main.GetComponent<CameraShake>().TriggerShake(2);
        RemoveCharactersFromPot();
    }

    public void IngredientDelivered(GameObject character) {
        // Set character GO in pot
        character.GetComponent<Animator>().SetTrigger("GoIntoPot");
        character.GetComponent<SpriteRenderer>().sortingOrder = 0;
        character.transform.position =
            new Vector3(transform.position.x + Random.Range(-0.3f, 0.3f), transform.position.y + 0.4f, 0);
        charactersInPot.Add(character);
        GetParticleSystem("Splash Particles").Play();

        soupData.IngredientDelivered(character.name);
        //ips.IngredientDelivered(character);
    }

    private ParticleSystem GetParticleSystem(string systemName) {
        foreach (ParticleSystem childParticleSystem in particleSystems) {
            if (childParticleSystem.name == systemName) {
                return childParticleSystem;
            }
        }
        return null;
    }
}
