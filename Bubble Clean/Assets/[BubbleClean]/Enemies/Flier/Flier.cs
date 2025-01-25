using UnityEngine;

public class Flier : Enemy
{
    [Header("Flying Settings")]
    public float hoverHeight = 2f; // Altura de vuelo
    public float oscillationAmplitude = 0.5f; // Amplitud de oscilación
    public float oscillationSpeed = 2f;
    
    [Header("Base Settings")]
    [SerializeField] private int _maxHealth = 3;
    [SerializeField] private int _damage = 1;
    [SerializeField] private int _scoreValue = 10;
    [SerializeField] private float _speed = 2f;
    [SerializeField] private float _rotationSpeed = 2f;
    
    protected override void Awake()
    {
        base.Awake();
        maxHealth = 3;
        currentHealth = maxHealth;
        damage = 1;
        scoreValue = 10;
        speed = 2f;
        rotationSpeed = 2f;
    }

    protected override void FollowPlayer()
    {
        // Calcular dirección hacia el jugador
        Vector3 directionToPlayer = (player.position - transform.position).normalized;

        // Rotar hacia el jugador sin afectar la oscilación
        Vector3 flatDirection = new Vector3(directionToPlayer.x, 0, directionToPlayer.z); // Sin altura
        Quaternion lookRotation = Quaternion.LookRotation(flatDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);

        // Calcular la posición objetivo con oscilación
        float oscillation = Mathf.Sin(Time.time * oscillationSpeed) * oscillationAmplitude; // Movimiento seno
        Vector3 targetPosition = player.position + Vector3.up * hoverHeight + Vector3.up * oscillation;

        // Mover gradualmente hacia el objetivo
        Vector3 movement = (targetPosition - transform.position).normalized;
        transform.position += movement * speed * Time.deltaTime;
    }
}
