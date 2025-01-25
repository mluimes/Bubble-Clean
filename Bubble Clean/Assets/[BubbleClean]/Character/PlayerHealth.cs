using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 10; // Salud máxima del jugador
    private int currentHealth;
        [SerializeField] private TextMeshProUGUI txtHealth;


    [Header("Damage Feedback")]
    [SerializeField] private GameObject damageEffect; // Efecto visual al recibir daño (opcional)
    [SerializeField] private AudioClip damageSound; // Sonido al recibir daño (opcional)
    [SerializeField] private AudioClip deathSound; // Sonido al morir (opcional)
    private AudioSource audioSource;

    private bool isDead = false;

    private void Awake()
    {
        currentHealth = maxHealth; // Inicializar la salud
        audioSource = GetComponent<AudioSource>();

        UpdateHealthUI();
    }

    public void TakeDamage(int amount)
{
    if (isDead) return; // No tomar daño si ya está muerto

    currentHealth -= amount;
    currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Asegurar que la salud no sea negativa
    UpdateHealthUI();

    // Reproducir feedback visual y sonoro
    if (damageEffect != null)
    {
        Instantiate(damageEffect, transform.position, Quaternion.identity);
    }
    if (damageSound != null && audioSource != null)
    {
        audioSource.PlayOneShot(damageSound);
    }

    // Llamar al efecto de daño en el jugador
    TakeDamageEffect takeDamageEffect = FindObjectOfType<TakeDamageEffect>();
    if (takeDamageEffect != null)
    {
        takeDamageEffect.StartCoroutine(takeDamageEffect.TakeDamageCoroutine());
    }

    if (currentHealth <= 0)
    {
        Die();
    }
}


    public void Heal(int amount)
    {
        if (isDead) return; // No curar si está muerto

        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Asegurar que la salud no exceda el máximo
    }

    private void Die()
    {
        isDead = true;

        // Reproducir sonido de muerte si existe
        if (deathSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(deathSound);
        }

        // Agregar lógica para manejar la muerte (reiniciar nivel, mostrar pantalla de muerte, etc.)
        Debug.Log("Player has died!");
        // Por ejemplo, desactivar al jugador:
        gameObject.SetActive(false);
    }

    void UpdateHealthUI()
    {
        txtHealth.text = $"Vida: " + currentHealth.ToString() + "/" + maxHealth.ToString();
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public bool IsDead()
    {
        return isDead;
    }
}
