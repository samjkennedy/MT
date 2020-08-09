using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{

    //health is an int to allow discrete changes to health (1 heart, 2 hearts etc)
    //NB 1 health = half a heart, 2 health = one whole heart
    public int maxHeartContainers = 10; 
    public int maxHealth;
    public int currentHealth;

    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite halfHeart;
    public Sprite emptyHeart;

    // Start is called before the first frame update
    void Start()
    {
        //Dirty hack, maybe fix me
        DontDestroyOnLoad(gameObject);
        UpdateHearts();
    }

    void Update() {
        UpdateHearts();
    }

    public int GetCurrentHealth() {
        return currentHealth;
    }

    public void Decrease(int decrease) {
        currentHealth = Mathf.Max(currentHealth - decrease, 0);
        UpdateHearts();
    }

    public void Increase(int increase) {
        currentHealth = Mathf.Min(currentHealth + increase, maxHealth);
        UpdateHearts();
    }

    public void IncreaseMaxContainers(int increase) {
        increase *= 2;
        maxHealth = Mathf.Min(maxHealth + increase, maxHeartContainers);
        UpdateHearts();
    }

    public void DecreaseMaxContainers(int decrease) {
        decrease *= 2;
        maxHealth = Mathf.Max(maxHealth - decrease, 0);
        UpdateHearts();
    }

    public void HealToMax() {
        currentHealth = maxHealth;
        UpdateHearts();
    }

    private void UpdateHearts() {
        for (int i = 0; i < maxHeartContainers; i++)
        {
            if (i < currentHealth/2) {
                hearts[i].sprite = fullHeart;
            } else if (i*2 + 1 == currentHealth) {
                hearts[i].sprite = halfHeart;
            } else {
                hearts[i].sprite = emptyHeart;
            }

            if (i < maxHealth/2) {
                hearts[i].enabled = true;
            } else {
                hearts[i].enabled = false;
            }
        }
    }
}
