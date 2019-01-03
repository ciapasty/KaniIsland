using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public GameObject menuCanvas;

    private void Awake() {
        menuCanvas.SetActive(false);
        Time.timeScale = 1;
    }
    
    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (menuCanvas.activeSelf) {
                HideMenuResumeGame();
            } else {
                ShowMenuPauseGame();
            }

        }
    }

    public void OnResumeClick() {
        HideMenuResumeGame();
    }

    public void OnExitClick() {
        StartCoroutine(LoadMenuSceneAsync());
    }

    void HideMenuResumeGame() {
        menuCanvas.SetActive(false);
        Time.timeScale = 1;
    }

    void ShowMenuPauseGame() {
        menuCanvas.SetActive(true);
        Time.timeScale = 0;
    }

    IEnumerator LoadMenuSceneAsync() {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Scenes/MainMenuScene");

        while (!asyncLoad.isDone) {
            yield return null;
        }

        Time.timeScale = 1;
    }

}
