using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    private int currentHealth;
    private int maxHealth;

    public bool IsDead => currentHealth == 0;

    // Método para inicializar la salud
    public void Initialize(int maxHealth)
    {
        this.maxHealth = maxHealth;
        currentHealth = maxHealth;
    }

    public void Reduce(int amount)
    {
        currentHealth = Mathf.Max(currentHealth - amount, 0);
    }

    public void Restore(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}
