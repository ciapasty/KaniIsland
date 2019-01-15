using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public GameObject menuCanvas;
    public UnityEngine.UI.Image transitionImage;

    public GameEvent pauseGameEvent;
    public GameEvent resumeGameEvent;

    public GameTimer gameTimer;

    private bool isGamePaused = false;

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

        if (!isGamePaused) {
            gameTimer.UpdateTime(Time.deltaTime);
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

    private void HideMenuResumeGame() {
        menuCanvas.SetActive(false);
        resumeGameEvent.Raise();
        isGamePaused = false;
        Time.timeScale = 1;

    }

    private void ShowMenuPauseGame() {
        menuCanvas.SetActive(true);
        pauseGameEvent.Raise();
        isGamePaused = true;
        Time.timeScale = 0;
    }

    // Scene transition

    private IEnumerator LoadMenuSceneAsync() {
        yield return new WaitForSeconds(1);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Scenes/MainMenuScene");

        while (!asyncLoad.isDone) {
            yield return null;
        }
    }

}
