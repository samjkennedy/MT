using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    public string sceneToLoadName;

    public Vector3 spawnLocation;

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            SceneManager.LoadSceneAsync(sceneToLoadName);
            other.gameObject.transform.position = spawnLocation;
        }
    }
}
