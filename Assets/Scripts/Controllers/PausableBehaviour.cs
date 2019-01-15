using UnityEngine;

public abstract class PausableBehaviour : MonoBehaviour {

    protected bool isGamePaused = false;

    public void OnGamePaused() {
        isGamePaused = true;
        AudioListener.pause = true;
    }

    public void OnGameResumed() {
        isGamePaused = false;
        AudioListener.pause = false;
    }

}
