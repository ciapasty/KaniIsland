using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsPanelController : MonoBehaviour {

    public SoupData soupData;

    private UnityEngine.UI.Text text;

    private void OnEnable() {
        text = transform.GetChild(1).GetComponent<UnityEngine.UI.Text>();
    }

    public void OnSoupComplete() {
        string newPoints = soupData.points.ToString();
        if (newPoints != text.text) {
            text.text = newPoints;
            GetComponent<Animator>().SetTrigger("PointsUpdate");
        }
    }

}
