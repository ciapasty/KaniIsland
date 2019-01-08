using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {

    public void OnPlayClick() {
        StartCoroutine(LoadGameSceneAsync());
    }

    public void OnExitClick() {
        Debug.Log("Game closed!");
        Application.Quit(0);
    }

    IEnumerator LoadGameSceneAsync() {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Scenes/GameScene");

        while (!asyncLoad.isDone) {
            yield return null;
        }
    }

}
