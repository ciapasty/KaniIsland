using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour {

    public GameObject mobPrefab;

    void Start() {

    }
    
    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Camera.main.GetComponent<CameraShake>().TriggerShake(2f);
        }
    }
}
