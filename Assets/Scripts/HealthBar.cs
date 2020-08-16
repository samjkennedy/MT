using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    public Sprite[] healthBars;
    public Enemy enemy;

    private int maxHealth;
    private int currentHealth;

    SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        enemy = transform.parent.GetComponent<Enemy>();
        maxHealth = enemy.health;
    }

    //TODO: Move this into a method that enemies call on health change, not every frame!!!
    void Update()
    {
        currentHealth = enemy.health;
        if (currentHealth == maxHealth) {
            sr.sprite = null;
        } else {
            int index = 12 - ((int) (12 * ((float)currentHealth / maxHealth)));
            sr.sprite = healthBars[index];
        }
    }
}
