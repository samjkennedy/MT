using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public Enemy[] enemies;

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(enemies[Random.Range(0, enemies.Length)], transform.position, Quaternion.identity, transform.parent.transform);
        Destroy(gameObject);
    }
}
