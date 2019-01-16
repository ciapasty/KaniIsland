using System;
using System.Collections;
using UnityEngine;

public class UITimePanelController : MonoBehaviour {

    public GameTimer gameTimer;

    public AudioClip countSound;

    private UnityEngine.UI.Text timeTextField;

    private int currCountdown = 5;
    private int prevCountdown = 0;

    // Start is called before the first frame update
    void Start() {
        timeTextField = GetComponentInChildren<UnityEngine.UI.Text>();
        StartCoroutine(GetTime());
    }

    private void Countdown(float time) {
        currCountdown = (int)time;
        if (time > 0 && time < 6) {
            if (currCountdown != prevCountdown) {
                PlayCount();
            }
            prevCountdown = currCountdown;
        }
    }

    public void PlayCount() {
        GetComponent<Animator>().SetTrigger("Count");
        GetComponent<AudioSource>().PlayOneShot(countSound);
    }

    IEnumerator GetTime() {
        for (;;) {
            float time = gameTimer.GetTime();
            Countdown(time);
            TimeSpan t = TimeSpan.FromSeconds(time);
            timeTextField.text = t.ToString(@"m\:ss");
            yield return new WaitForSeconds(0.1f);
        }
    }
}
