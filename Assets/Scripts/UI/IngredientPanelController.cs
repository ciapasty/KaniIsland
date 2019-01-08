using UnityEngine;
using System.Collections.Generic;

public class IngredientPanelController : MonoBehaviour {

    public SoupData soupData;

    public GameObject ingredientImagePrefab;
    public Sprite correctSprite;
    public Sprite wrongSprite;
    public AudioClip correctSound;
    public AudioClip wrongSound;
    public GameObject islandGodText;

    private AudioSource audioSource;

    private Ingredient[] ingredientsPrev;
    private GameObject[] ingredientsGOs;

    private void OnEnable() {
        audioSource = GetComponent<AudioSource>();
    }

    public void OnNewIngredients() {
        if (ingredientsGOs != null) DestroyIngredientImageGOs();

        Ingredient[] igns = soupData.GetIngredients();
        ingredientsGOs = new GameObject[igns.Length];

        for (int i = 0; i < igns.Length; i++) {
            ingredientsGOs[i] = CreateIngredientImageGO(igns[i].sprite);
        }

        ingredientsPrev = igns;
    }

    public void OnIngredientDelivered() {
        Ingredient[] igns = soupData.GetIngredients();

        for (int i = 0; i < igns.Length; i++) {
            if (igns[i].state != ingredientsPrev[i].state) {
                UpdateIngredientImage(i, igns[i].state);
            }
        }

        ingredientsPrev = igns;
    }

    public void OnSoupComplete() {
        islandGodText.GetComponent<Animator>().SetTrigger("ShowText");
    }

    private void UpdateIngredientImage(int index, IngredientState state) {
        GameObject child = ingredientsGOs[index].transform.GetChild(1).gameObject;
        UnityEngine.UI.Image correctImage = child.GetComponent<UnityEngine.UI.Image>();
        if (state == IngredientState.Delivered) {
            correctImage.sprite = correctSprite;
            audioSource.PlayOneShot(correctSound);
        }

        if (state == IngredientState.Failed) {
            correctImage.sprite = wrongSprite;
            audioSource.PlayOneShot(wrongSound);
        }

        child.GetComponent<Animator>().SetTrigger("SetState");

    }

    private GameObject CreateIngredientImageGO(Sprite sprite) {
        GameObject go = Instantiate(ingredientImagePrefab);
        go.GetComponent<UnityEngine.UI.Image>().sprite = sprite;
        go.transform.SetParent(transform);
        return go;
    }

    private void DestroyIngredientImageGOs() {
        for (int i = 0; i < transform.childCount; i++) {
            Destroy(transform.GetChild(i).gameObject);
        }
        ingredientsGOs = null;
    }

}
