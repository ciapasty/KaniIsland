using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGameEndTextController : MonoBehaviour {

    public SoupData p1SoupData;
    public SoupData p2SoupData;

    public void OnGameEnded() {
        if (p1SoupData.points > p2SoupData.points) {
            GetComponent<UnityEngine.UI.Text>().text = "Player 1 wins!";
        } else if (p1SoupData.points < p2SoupData.points) {
            GetComponent<UnityEngine.UI.Text>().text = "Player 2 wins!";
        }

        GetComponent<Animator>().SetTrigger("Show");
    }

}
