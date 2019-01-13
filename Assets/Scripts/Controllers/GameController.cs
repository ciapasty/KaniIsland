using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public GameObject menuCanvas;
    public UnityEngine.UI.Image transitionImage;

    private void Awake() {
        menuCanvas.SetActive(false);
    }

    private void Start() {
        transitionImage.GetComponent<Animator>().SetTrigger("TransitionIn");
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

    // Menu

    public void OnResumeClick() {
        HideMenuResumeGame();
    }

    public void OnExitClick() {
        transitionImage.GetComponent<Animator>().SetTrigger("TransitionOut");
        // TODO: Better pausing!
        Time.timeScale = 1;
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

    // Scene transition

    IEnumerator LoadMenuSceneAsync() {
        yield return new WaitForSeconds(1);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Scenes/MainMenuScene");

        while (!asyncLoad.isDone) {
            yield return null;
        }
    }

}
