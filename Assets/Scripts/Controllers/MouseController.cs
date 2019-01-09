using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour {

    public SoupData soupData;

    void Start() {

    }
    
    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            soupData.GetNewIngredients();
        }
    }
}
