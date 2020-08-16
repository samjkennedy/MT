using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootController : MonoBehaviour
{
    public static LootController instance;

    public Coin coinPrefab;
    public HealthPotion healthPotionPrefab;
    public Key keyPrefab;

    void Awake() {
        instance = this;
    }
}
