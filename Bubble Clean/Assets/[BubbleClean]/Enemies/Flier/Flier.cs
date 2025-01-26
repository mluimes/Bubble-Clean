using UnityEngine;

public class Flier : Enemy
{
    [Header("Flying Settings")]
    public float hoverHeight = 2f; // Altura de vuelo
    public float oscillationAmplitude = 0.5f; // Amplitud de oscilación
    public float oscillationSpeed = 2f;

    protected override void FollowPlayer()
    {
        // Calcular dirección hacia el jugador
        Vector3 directionToPlayer = (player.position - transform.position).normalized;

        // Rotar hacia el jugador sin afectar la oscilación
        Vector3 flatDirection = new Vector3(directionToPlayer.x, 0, directionToPlayer.z); // Sin altura
        Quaternion lookRotation = Quaternion.LookRotation(flatDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);

        // Calcular la distancia al jugador
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Ajustar la altura y la amplitud de oscilación en función de la distancia al jugador
        float targetHeight = hoverHeight;
        float adjustedOscillationAmplitude = oscillationAmplitude;

        if (distanceToPlayer <= attackRange)
        {
            // Reducir la altura y la amplitud de oscilación al atacar
            targetHeight = 0.2f;
            adjustedOscillationAmplitude = oscillationAmplitude * 0.1f; // Oscilación reducida al 30%
        }

        // Calcular la posición objetivo con oscilación ajustada
        float oscillation = Mathf.Sin(Time.time * oscillationSpeed) * adjustedOscillationAmplitude; // Movimiento seno ajustado
        Vector3 targetPosition = player.position + Vector3.up * targetHeight + Vector3.up * oscillation;

        // Mover gradualmente hacia el objetivo
        Vector3 movement = (targetPosition - transform.position).normalized;
        transform.position += movement * speed * Time.deltaTime;
    }
}
