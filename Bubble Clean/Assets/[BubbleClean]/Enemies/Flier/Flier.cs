using UnityEngine;

public class Flier : MonoBehaviour
{
    [Header("Movement Settings")]
    public Transform player; // Referencia al jugador
    public float speed = 5f; // Velocidad de movimiento
    public float rotationSpeed = 2f; // Velocidad de giro hacia el jugador
    public float hoverHeight = 2f; // Altura base del vuelo
    public float oscillationAmplitude = 0.5f; // Amplitud de oscilación
    public float oscillationSpeed = 2f; // Velocidad de oscilación
    public float attackRange = 1.5f; // Rango para atacar

    [Header("Health Settings")]
    public int maxHealth = 3; // Salud máxima

    private FlierHealth health; // Componente de salud
    private Vector3 directionToPlayer; // Dirección calculada hacia el jugador

    private void Awake()
    {
        health = new FlierHealth(maxHealth);
    }

    private void Update()
    {
        if (player == null) return;

        FollowPlayer();

        // Destruir si la salud llega a 0
        if (health.IsDead)
        {
            Die();
        }
    }

    private void FollowPlayer()
    {
        // Calcular dirección hacia el jugador
        directionToPlayer = (player.position - transform.position).normalized;

        // Rotar hacia el jugador sin afectar la oscilación
        Vector3 flatDirection = new Vector3(directionToPlayer.x, 0, directionToPlayer.z); // Sin altura
        Quaternion lookRotation = Quaternion.LookRotation(flatDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);

        // Calcular la posición objetivo con oscilación
        float oscillation = Mathf.Sin(Time.time * oscillationSpeed) * oscillationAmplitude; // Movimiento seno
        Vector3 targetPosition = player.position + Vector3.up * hoverHeight + Vector3.up * oscillation;

        // Si está cerca del jugador, descender
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= attackRange)
        {
            targetPosition.y = 0.5f; // Altura baja al atacar
        }

        // Mover gradualmente hacia el objetivo
        Vector3 movement = (targetPosition - transform.position).normalized;
        transform.position += movement * speed * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("PlayerProjectile"))
        {
            TakeDamage(1);
        }
    }

    public void TakeDamage(int damage)
    {
        health.Reduce(damage);
    }

    private void Die()
    {
        // Aquí puedes agregar efectos o animaciones antes de destruir
        Destroy(gameObject);
    }
}
