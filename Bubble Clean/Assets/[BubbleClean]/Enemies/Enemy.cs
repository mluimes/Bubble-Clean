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
    public float damageCooldown = 0.1f; // Tiempo mínimo entre daños en segundos
    private float lastDamageTime; // Último tiempo de daño recibido
    
    [Header("Score Settings")]
    protected int scoreValue = 10; // Puntos que otorga al ser destruido

    [Header("Movement Settings")]
    protected float speed = 5f; // Velocidad de movimiento
    protected float rotationSpeed = 2f; // Velocidad de giro hacia el jugador
    protected Transform player; // Referencia al jugador

    protected virtual void Awake()
    {
        health = gameObject.AddComponent<EnemyHealth>(); // Añadir el componente de salud
        health.Initialize(maxHealth); // Inicializar la salud con el valor máximo
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (player != null)
        {
            FollowPlayer();
        }
    }

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("PlayerProjectile")) {
            Debug.Log("Hit!");
            TakeDamage(1);
        }
    }

    protected virtual void FollowPlayer() {
        Vector3 direction = (player.position - transform.position).normalized;

        // Rotar hacia el jugador
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);

        // Mover hacia el jugador
        transform.position += direction * speed * Time.deltaTime;
    }

    public void TakeDamage(int damageAmount)
    {
        if (Time.time - lastDamageTime < damageCooldown) return; // Si está en cooldown, no toma daño

        lastDamageTime = Time.time; // Actualiza el tiempo del último daño
        health.Reduce(damageAmount);

        Debug.Log($"Health: {health.GetCurrentHealth()}");

        if (health.IsDead)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
        // Aquí puedes agregar lógica de puntuación o efectos al morir
    }
}
