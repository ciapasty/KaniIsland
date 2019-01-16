using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public GameObject countdownPanel;
    public GameObject menuPanel;
    public UnityEngine.UI.Image transitionImage;
    public NpcSpawner spawner;

    public GameEvent pauseGameEvent;
    public GameEvent resumeGameEvent;
    public GameEvent gameEndedEvent;

    public GameTimer gameTimer;

    private bool isGamePaused = true;

    private void Awake() {
        menuPanel.SetActive(false);
    }

    private void Start() {
        transitionImage.GetComponent<Animator>().SetTrigger("TransitionIn");
        isGamePaused = true;
        spawner.enabled = false;
        AudioListener.pause = false;
        gameTimer.Reset();
        StartCoroutine(PlayCountdown());
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (menuPanel.activeSelf) {
                HideMenuResumeGame();
            } else {
                ShowMenuPauseGame();
            }
        }
        if (!isGamePaused) {
            if (gameTimer.GetTime() <= 0) {
                gameEndedEvent.Raise();
                isGamePaused = true;
                Time.timeScale = 0;
            }

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
        menuPanel.SetActive(false);
        ResumeGame();
    }

    private void ShowMenuPauseGame() {
        menuPanel.SetActive(true);
        PauseGame();
    }

    private void PauseGame() {
        pauseGameEvent.Raise();
        isGamePaused = true;
        Time.timeScale = 0;
        AudioListener.pause = true;
    }

    private void ResumeGame() {
        resumeGameEvent.Raise();
        isGamePaused = false;
        Time.timeScale = 1;
        AudioListener.pause = false;
    }

    // Countdown

    private IEnumerator PlayCountdown() {
        yield return new WaitForSeconds(0.6f);

        countdownPanel.GetComponent<Animator>().SetTrigger("Run");

        yield return new WaitForSeconds(3);

        ResumeGame();
        spawner.enabled = true;
    }

    // Scene transition

    private IEnumerator LoadMenuSceneAsync() {
        yield return new WaitForSeconds(1);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Scenes/MainMenuScene");

        while (!asyncLoad.isDone) {
            yield return null;
        }

        AudioListener.pause = false;
    }

}
