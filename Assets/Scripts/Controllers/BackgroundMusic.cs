using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour {

    private AudioSource audioSource;

    void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    public void Play() {
        audioSource.Play();
    }

    public void Stop() {
        audioSource.Stop();
    }
}
