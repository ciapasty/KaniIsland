using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {

    public UnityEngine.UI.Image transitionImage;

    private void Start() {
        transitionImage.GetComponent<Animator>().SetTrigger("TransitionIn");
    }

    public void OnPlayClick() {
        transitionImage.GetComponent<Animator>().SetTrigger("TransitionOut");
        GetComponent<AudioSource>().Play();
        StartCoroutine(LoadGameSceneAsync());
    }

    IEnumerator LoadGameSceneAsync() {
        yield return new WaitForSeconds(1f);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Scenes/GameScene");

        while (!asyncLoad.isDone) {
            yield return null;
        }
    }

}
