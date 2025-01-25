using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] protected int maxHealth; // Salud máxima
    [SerializeField] protected int currentHealth; // Salud actual
    [SerializeField] protected EnemyHealth health; // Componente de salud

    [Header("Attack Settings")]
    [SerializeField] protected int damage; // Daño que inflige al jugador
    [SerializeField] protected float damageCooldown; // Tiempo mínimo entre daños en segundos
    [SerializeField] protected float lastDamageTime; // Último tiempo de daño realizado
    [SerializeField] protected float attackRange; // Rango de ataque

    [Header("Score Settings")]
    [SerializeField] protected int scoreValue; // Puntos que otorga al ser destruido

    [Header("Movement Settings")]
    [SerializeField] protected float speed; // Velocidad de movimiento
    [SerializeField] protected float rotationSpeed; // Velocidad de giro hacia el jugador
    protected Transform player; // Referencia al jugador

    private bool isAttacking = false; // Indica si está atacando

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
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("PlayerProjectile"))
        {
            Debug.Log("Hit by projectile!");
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
