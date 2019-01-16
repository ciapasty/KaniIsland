using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICountdownPanelController : MonoBehaviour {

    public AudioClip countSound;
    public AudioClip startSound;

    public void PlayCount() {
        GetComponent<AudioSource>().PlayOneShot(countSound);
    }

    public void PlayStart() {
        GetComponent<AudioSource>().PlayOneShot(startSound);
    }

}
