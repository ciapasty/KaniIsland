﻿using UnityEngine;
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
    public SpriteCollection availableIngredients;

    public Ingredient[] ingredients;

    public Ingredient[] GetIngredients() {
        return ingredients != null ? (Ingredient[])ingredients.Clone() : new Ingredient[0];
    }

    public void GetNewIngredients() {

        int count = Random.Range(2, 5);
        ingredients = new Ingredient[count];

        for (int i = 0; i < count; i++) {
            Ingredient ign = new Ingredient();
            Sprite sprite = availableIngredients.collection[
                    Random.Range(0, availableIngredients.collection.Length - 1)
                ];
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
            onSoupComplete.Raise();
            // TODO: Detect if all ingredients were correct
            //if (AllIngredientsCorrect()) {
            //    // TODO: Points counting
            //    onSoupComplete.Raise();
            //} else {
            //    // TODO: Points counting
            //    onSoupComplete.Raise();
            //    Debug.Log("Congratulations! Soup prepared correctly!");
            //}

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