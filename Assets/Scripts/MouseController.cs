using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour {

    public GameObject mobPrefab;

    void Start() {

    }
    
    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            GameObject mob = Instantiate(mobPrefab, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);
            mob.transform.position = new Vector3(mob.transform.position.x, mob.transform.position.y, 0);
        }
    }
}
