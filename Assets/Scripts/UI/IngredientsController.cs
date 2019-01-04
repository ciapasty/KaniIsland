using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum IngredientState { Waiting, Delivered, Failed }

struct Ingredient {
    public string name;
    public IngredientState state;
    public GameObject UIImageGO;
}

public class IngredientsController : MonoBehaviour {

    public GameObject ingredientImagePrefab;
    public PotController pot;
    public Sprite correctSprite;
    public Sprite wrongSprite;
    public Sprite[] availableIngredients;

    private Ingredient[] ingredients;

    void Start() {
        GetNewIngredients();
    }

    // Public methods

    public void IngredientsCollectedCheck() {
        // Check if recipe is complete
        if (AllIngredientsDelivered()) {
            if (AllIngredientsCorrect()) {
                // TODO: Points counting
                Debug.Log("Congratulations! Soup prepared correctly!");
            } else {
                // TODO: Points counting
                Debug.Log("Congratulations! Soup prepared correctly!");
            }

            // TODO: Add animation for the whole panel
            GetNewIngredients();
        }
    }

    public void IngredientDelivered(GameObject character) {
        // Search for matching ingredient
        for (int i = 0; i < ingredients.Length; i++) {
            if (ingredients[i].name == character.name && ingredients[i].state == IngredientState.Waiting) {
                ingredients[i].state = IngredientState.Delivered;
                GameObject child = ingredients[i].UIImageGO.transform.GetChild(1).gameObject;
                UnityEngine.UI.Image correctImage = child.GetComponent<UnityEngine.UI.Image>();
                correctImage.sprite = correctSprite;
                child.GetComponent<Animator>().SetTrigger("SetState");
                // Return on first match
                return;
            }
        }

        // No match found. Fail first waiting ingredient
        for (int i = 0; i < ingredients.Length; i++) {
            if (ingredients[i].state == IngredientState.Waiting) {
                ingredients[i].state = IngredientState.Failed;
                GameObject child = ingredients[i].UIImageGO.transform.GetChild(1).gameObject;
                UnityEngine.UI.Image wrongImage = child.GetComponent<UnityEngine.UI.Image>();
                wrongImage.sprite = wrongSprite;
                child.GetComponent<Animator>().SetTrigger("SetState");
                // Return on first match
                return;
            }
        }
    }

    // Private

    private void GetNewIngredients() {
        RemoveIngredientImages();
        pot.RemoveCharactersFromPot();

        int count = Random.Range(2, 5);
        ingredients = new Ingredient[count];

        for (int i = 0; i < count; i++) {
            Ingredient ign = new Ingredient();
            Sprite sprite = availableIngredients[
                    Random.Range(0, availableIngredients.Length - 1)
                ];
            ign.name = sprite.name;
            ign.state = IngredientState.Waiting;
            ign.UIImageGO = CreateIngredientImageGO(sprite);
            ingredients[i] = ign;
        }
    }

    private GameObject CreateIngredientImageGO(Sprite sprite) {
        GameObject go = Instantiate(ingredientImagePrefab);
        go.GetComponent<UnityEngine.UI.Image>().sprite = sprite;
        go.transform.GetChild(1).GetComponent<IngredientPanelController>().SetIngredientsController(this);
        go.transform.SetParent(transform);
        return go;
    }

    private void RemoveIngredientImages() {
        for (int i = 0; i < transform.childCount; i++) {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    private bool AllIngredientsDelivered() {
        for (int i = 0; i < ingredients.Length; i++) {
            if (ingredients[i].state == IngredientState.Waiting)
                return false;
        }
        return true;
    }

    private bool AllIngredientsCorrect() {
        for (int i = 0; i < ingredients.Length; i++) {
            if (ingredients[i].state == IngredientState.Failed)
                return false;
        }
        return true;
    }
}
