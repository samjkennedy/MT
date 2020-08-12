using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string currentSceneName;
    public string sceneToLoadName;
    public Player playerPrefab;

    public Vector3 spawnLocation;

    void Update() {
        if (Input.GetKeyDown(KeyCode.Return)) {
            SceneManager.LoadScene(sceneToLoadName);
            Scene scene = SceneManager.GetSceneByName(sceneToLoadName);
            Player player = Instantiate(playerPrefab, spawnLocation, Quaternion.identity);

            SceneManager.MoveGameObjectToScene(player.gameObject, scene);
        }
    }
}
