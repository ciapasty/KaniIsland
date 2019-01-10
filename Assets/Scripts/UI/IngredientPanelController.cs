using UnityEngine;
using System.Collections;
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

        StartCoroutine("NewIngredientsDelay");
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

    private IEnumerator NewIngredientsDelay() {
        yield return new WaitForSeconds(0.6f);
        GetNewIngredients();
    }

    private void GetNewIngredients() {
        Ingredient[] igns = soupData.GetIngredients();
        ingredientsGOs = new GameObject[igns.Length];

        for (int i = 0; i < igns.Length; i++) {
            ingredientsGOs[i] = CreateIngredientImageGO(igns[i].sprite);
        }

        ingredientsPrev = igns;
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

        Animator ignAnim = ingredientsGOs[index].GetComponent<Animator>();
        ignAnim.SetFloat("Speed", Random.Range(0.8f, 1.2f));
        ignAnim.SetTrigger("Delivered");

    }

    private GameObject CreateIngredientImageGO(Sprite sprite) {
        GameObject go = Instantiate(ingredientImagePrefab);
        go.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().sprite = sprite;
        go.GetComponent<Animator>().SetFloat("Speed", Random.Range(0.8f, 1.2f));
        go.transform.SetParent(transform);
        return go;
    }

    private void DestroyIngredientImageGOs() {
        for (int i = 0; i < ingredientsGOs.Length; i++) {
            ingredientsGOs[i].GetComponent<Animator>().SetTrigger("Disappear");
        }
        ingredientsGOs = null;
    }

}
