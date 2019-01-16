using System;
using System.Collections;
using UnityEngine;

public class UITimePanelController : MonoBehaviour {

    public GameTimer gameTimer;

    private UnityEngine.UI.Text timeTextField;

    // Start is called before the first frame update
    void Start() {
        timeTextField = GetComponentInChildren<UnityEngine.UI.Text>();
        StartCoroutine(GetTime());
    }

    IEnumerator GetTime() {
        for (;;) {
            TimeSpan t = TimeSpan.FromSeconds(gameTimer.GetTime());
            timeTextField.text = t.ToString(@"m\:ss");
            yield return new WaitForSeconds(0.5f);
        }
    }
}
