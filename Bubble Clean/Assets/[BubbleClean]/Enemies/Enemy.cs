using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Health Settings")]
    protected int maxHealth = 3; // Salud máxima
    protected int currentHealth; // Salud actual
    private EnemyHealth health; // Componente de salud

    [Header("Damage Settings")]
    protected int damage = 1; // Daño que inflige al jugador
    private float damageCooldown = 1f; // Tiempo mínimo entre daños en segundos
    private float lastDamageTime; // Último tiempo de daño realizado

    [Header("Score Settings")]
    protected int scoreValue = 10; // Puntos que otorga al ser destruido

    [Header("Movement Settings")]
    protected float speed = 5f; // Velocidad de movimiento
    protected float rotationSpeed = 2f; // Velocidad de giro hacia el jugador
    protected Transform player; // Referencia al jugador

    private bool isAttacking = false; // Indica si está atacando
    private bool hasCollided = false; // Indica si ha colisionado con bala

    protected virtual void Awake()
    {
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (player != null && !isAttacking)
        {
            FollowPlayer();
        }

        hasCollided = false;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (hasCollided) return;

        if (other.gameObject.CompareTag("PlayerProjectile"))
        {
            Debug.Log("Hit by projectile!");
            hasCollided = true; 
            TakeDamage(1);
        }

        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player detected! Attacking...");
            isAttacking = true; // Detener movimiento al atacar
            DamagePlayer(other.gameObject);
        }

    }

    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Comprobar si ya está atacando
            if (!isAttacking)
            {
                Debug.Log("Starting attack...");
                isAttacking = true;
            }

            // Continuar dañando al jugador dentro del rango
            DamagePlayer(other.gameObject);
        }
        else
        {
            // Asegurarse de no entrar en ataque si no es el jugador
            isAttacking = false;
        }
    }


    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player out of range. Resuming movement.");
            isAttacking = false; // Reanudar movimiento al dejar de atacar
        }
    }

    protected virtual void FollowPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;

        // Rotar hacia el jugador
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);

        // Mover hacia el jugador
        transform.position += direction * speed * Time.deltaTime;
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        Debug.Log($"Enemy health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void DamagePlayer(GameObject playerObject)
    {
        if (Time.time - lastDamageTime >= damageCooldown)
        {
            var playerHealth = playerObject.GetComponentInChildren<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                lastDamageTime = Time.time;
                Debug.Log("Player damaged!");
            }
        }
    }

    protected virtual void Die()
    {
        Debug.Log("Enemy died.");
        Destroy(gameObject);
    }
}
