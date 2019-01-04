using UnityEngine;
using System.Collections;

public class IngredientPanelController : MonoBehaviour {

    private IngredientsController ic;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void SetIngredientsController(IngredientsController ic) {
        this.ic = ic;
    }

    private void OnAnimationFinish() {
        ic.IngredientsCollectedCheck();
    }
}
