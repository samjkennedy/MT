using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ConsumableController : MonoBehaviour
{

    public static ConsumableController instance;

    private static int maxCoins = 99;
    private static int maxKeys = 99;

    private int coins;
    public TextMeshProUGUI coinCounter;
    private int keys;
    public TextMeshProUGUI keyCounter;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        UpdateCoinCounter();
        UpdateKeyCounter();
    }

    void Update() {
        if (Input.GetKey(KeyCode.C)) {
            AddCoins(1);
        }
        if (Input.GetKey(KeyCode.K)) {
            AddKeys(1);
        }
    }

    public void AddCoins(int coins) {
        this.coins = Mathf.Min(maxCoins, this.coins + coins);
        UpdateCoinCounter();
    }

    public void RemoveCoins(int coins) {
        this.coins = Mathf.Max(0, this.coins - coins);
        UpdateCoinCounter();
    }

    public bool AtMaxCoins() {
        return coins == maxCoins;
    }

    public int GetCoins() {
        return coins;
    }

    private void UpdateCoinCounter() {
        coinCounter.text = coins < 10 ? "0" + coins : coins.ToString();
    }

    public void AddKeys(int keys) {
        this.keys = Mathf.Min(maxKeys, this.keys + keys);
        UpdateKeyCounter();
    }

    public void RemoveKeys(int keys) {
        this.keys = Mathf.Max(0, this.keys - keys);
        UpdateKeyCounter();
    }

    public bool AtMaxKeys() {
        return keys == maxKeys;
    }

    public int GetKeys() {
        return keys;
    }

    private void UpdateKeyCounter() {
        keyCounter.text = keys < 10 ? "0" + keys : keys.ToString();
    }

}
