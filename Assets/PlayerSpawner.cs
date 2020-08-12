using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{

    public Player playerPrefab;

    // Start is called before the first frame update
    void Start()
    {
        GameObject existingPlayer = GameObject.Find("Player");
        if (existingPlayer == null) {
            Instantiate(playerPrefab, transform.position, Quaternion.identity);
        }
    }
}
