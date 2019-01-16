using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcSpawner : PausableBehaviour {

    public PrefabCollection npcs;

    public int spawnCount = 1;
    public int characterLimit = 40;

    private float spawnTimer = 0;

    void Update() {
        if (!isGamePaused) {
            if (transform.childCount <= characterLimit) {
                if (spawnTimer <= 0) {
                    spawnCount = Random.Range(1, 10);
                    for (int i = 0; i < spawnCount; i++) {
                        GameObject go = Instantiate(
                                npcs.prefabs[Random.Range(0, npcs.prefabs.Length)],
                                new Vector3(Random.Range(-2, 2),
                                transform.position.y, 0),
                                Quaternion.identity
                                );
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
}
