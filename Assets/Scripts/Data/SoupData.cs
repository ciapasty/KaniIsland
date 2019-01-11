using UnityEngine;
using System.Collections;

public enum IngredientState { Waiting, Delivered, Failed }

public struct Ingredient {
    public Sprite sprite;
    public IngredientState state;
}

[CreateAssetMenu]
public class SoupData : ScriptableObject {

    public string playerTag;

    public GameEvent onNewIngredients;
    public GameEvent onIngredientDelivered;
    public GameEvent onSoupComplete;
    public PrefabCollection availableIngredients;

    public Ingredient[] ingredients;

    public int points = 0;

    public Ingredient[] GetIngredients() {
        return ingredients != null ? (Ingredient[])ingredients.Clone() : new Ingredient[0];
    }

    public void GetNewIngredients() {

        int count = Random.Range(2, 5);
        ingredients = new Ingredient[count];

        for (int i = 0; i < count; i++) {
            Ingredient ign = new Ingredient();
            Sprite sprite = availableIngredients.prefabs[
                    Random.Range(0, availableIngredients.prefabs.Length - 1)
                ].GetComponent<SpriteRenderer>().sprite;
            ign.sprite = sprite;
            ign.state = IngredientState.Waiting;
            ingredients[i] = ign;
        }

        onNewIngredients.Raise();
    }

    public void IngredientDelivered(string name) {
        // Search for matching ingredient
        for (int i = 0; i < ingredients.Length; i++) {
            if (ingredients[i].sprite.name == name && ingredients[i].state == IngredientState.Waiting) {
                SetIngredientState(i, IngredientState.Delivered);
                // Return on first match
                return;
            }
        }

        // No match found. Fail first waiting ingredient
        for (int i = 0; i < ingredients.Length; i++) {
            if (ingredients[i].state == IngredientState.Waiting) {
                SetIngredientState(i, IngredientState.Failed);
                // Return on first match
                return;
            }
        }
    }

    private void IngredientsCollectedCheck() {
        // Check if recipe is complete
        if (AllIngredientsDelivered()) {
            if (AllIngredientsCorrect()) {
                points += ingredients.Length * 2;
                onSoupComplete.Raise();
            } else {
                for (int i = 0; i < ingredients.Length; i++) {
                    if (ingredients[i].state == IngredientState.Delivered)
                        points += 1;
                }
            }

            Debug.Log(playerTag + " has " + points + " points.");

            GetNewIngredients();
        }
    }

    private void SetIngredientState(int index, IngredientState state) {
        ingredients[index].state = state;
        onIngredientDelivered.Raise();
        IngredientsCollectedCheck();
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
