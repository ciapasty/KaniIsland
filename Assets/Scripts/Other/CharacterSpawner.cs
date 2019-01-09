﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpawner : MonoBehaviour {

    public GameObject character;

    float spawnTimer = 0;
    int spawnCount = 1;
    readonly int characterLimit = 40;

    void Update() {
        if (transform.childCount <= characterLimit) {
            if (spawnTimer <= 0) {
                spawnCount = Random.Range(1, 10);
                for (int i = 0; i < spawnCount; i++) {
                    GameObject go = Instantiate(character, new Vector3(Random.Range(-2, 2), transform.position.y, 0), Quaternion.identity);
                    go.name = go.GetComponent<SpriteRenderer>().sprite.name;
                    go.transform.SetParent(this.transform);
                }
                spawnTimer = Random.Range(1, 5);
            } else {
                spawnTimer -= Time.deltaTime;
            }
        }
    }
}
