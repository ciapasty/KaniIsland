using System.Collections;
using UnityEngine;

[CreateAssetMenu]
public class GameTimer : ScriptableObject {

    public int startTimeMinutes = 5;

    public float timer;

    private void OnEnable() {
        timer = startTimeMinutes * 60f;
    }

    public void UpdateTime(float time) {
        timer -= time;
    }

    public float GetTime() {
        return timer;
    }

}