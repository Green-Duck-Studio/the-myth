using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    // public float health;
    // public float maxHealth;
    // public Image healthBar;

    // public int maxHealth = 100;
    

    // private void Start()
    // {
    //     // maxHealth = health;
    //     
    // }
    //
    // private void Update()
    // {
    //     // healthBar.fillAmount = Mathf.Clamp(health / maxHealth, 0, 100);
    // }
    
    // public float health;
    // public float maxHealth;
    public Image healthBar;

    public float maxHealth = 100;
    private float currentHealth;

    private void Start()
    {
        // maxHealth = health;
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Debug.Log("Die");
            //SceneManager.LoadScene(0);
        }
    }

    public void Damaged(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Debug.Log("Game Over");
        }
    }

    private void Update()
    {
        healthBar.fillAmount = Mathf.Clamp(currentHealth / maxHealth, 0, 100);
    }
}
