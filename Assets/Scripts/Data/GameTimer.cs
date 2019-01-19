using System.Collections;
using UnityEngine;

[CreateAssetMenu]
public class GameTimer : ScriptableObject {

    public int startTimeMinutes = 5;

    public float timer;

    public void UpdateTime(float time) {
        timer -= time;
    }

    public float GetTime() {
        return timer;
    }

    public void Reset() {
        timer = startTimeMinutes * 60f;
    }
}