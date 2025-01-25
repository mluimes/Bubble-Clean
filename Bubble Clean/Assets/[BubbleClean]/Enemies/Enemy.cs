using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] protected int maxHealth; // Salud máxima
    [SerializeField] protected int currentHealth; // Salud actual
    [SerializeField] protected EnemyHealth health; // Componente de salud

    [Header("Melee Settings")]
    [SerializeField] protected int meleeDamage; // Daño cuerpo a cuerpo
    [SerializeField] protected float meleeCooldown; // Tiempo mínimo entre daños en segundos
    [SerializeField] protected float lastDamageTime; // Último tiempo de daño realizado
    
    [Header("Attack Settings")]
    [SerializeField] protected int attackDamage; // Daño de ataque
    [SerializeField] protected float attackCooldown; // Tiempo mínimo entre ataques en segundos
    [SerializeField] protected float attackRange; // Rango de ataque
    

    [Header("Score Settings")]
    [SerializeField] protected int scoreValue; // Puntos que otorga al ser destruido

    [Header("Movement Settings")]
    [SerializeField] protected float speed; // Velocidad de movimiento
    [SerializeField] protected float rotationSpeed; // Velocidad de giro hacia el jugador
    protected Transform player; // Referencia al jugador

    private bool isAttacking = false; // Indica si está atacando
    private Collider enemyCollider;

    protected virtual void Awake()
    {
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Buscar el Collider en los hijos
        enemyCollider = GetComponentInChildren<Collider>();
        if (enemyCollider == null)
        {
            Debug.LogError("No Collider found for Enemy. Ensure the Collider is assigned to the Enemy or its child objects.");
        }
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
            Debug.Log("Melee dmg");
            isAttacking = true;
            MeleePlayer(other.gameObject);
        }
    }

    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!isAttacking)
            {
                isAttacking = true;
            }
            MeleePlayer(other.gameObject);
        }
        else
        {
            isAttacking = false;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Resuming movement.");
            isAttacking = false;
        }
    }

    protected virtual void FollowPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;

        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);

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

    private void MeleePlayer(GameObject playerObject)
    {
        if (Time.time - lastDamageTime >= meleeCooldown)
        {
            var playerHealth = playerObject.GetComponentInChildren<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(meleeDamage);
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
