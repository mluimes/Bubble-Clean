using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roller : Enemy
{
    [Header("Roller Settings")]
    [SerializeField] private Transform model; // Reference to the sphere model
    [SerializeField] private float rollSpeed = 10f; // Speed of the rolling animation
    
    protected override void FollowPlayer()
    {
        if (player == null) return;

        // Calculate direction to player
        Vector3 direction = (player.position - transform.position).normalized;

        // Rotate the GameObject towards the player
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);

        // Move the GameObject forward
        transform.position += direction * speed * Time.deltaTime;

        // Rotate the model to simulate rolling
        float rollAmount = speed * Time.deltaTime * rollSpeed;
        if (model != null)
        {
            model.Rotate(Vector3.right * rollAmount, Space.Self);
        }
        else
        {
            Debug.LogWarning("Model reference is not set for the Roller.");
        }
    }

    override protected void TakeDamage(int damageAmount)
    {
        pointsManager.AddPoints(scoreValue);
        currentHealth -= damageAmount;
        Debug.Log($"Enemy health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }


    protected override void MeleePlayer(GameObject playerObject)
    {
        if (Time.time - lastDamageTime >= meleeCooldown)
        {
            var playerHealth = playerObject.GetComponentInChildren<PlayerHealth>();
            if (playerHealth != null)
            {
                // Inflict damage on the player
                playerHealth.TakeDamage(meleeDamage);
                lastDamageTime = Time.time;
                Debug.Log("Player damaged!");
            }

            // Make the enemy die
            Die();
        }
    }

    protected override void Die()
    {
        if (isDead) return; // Evitar que se ejecute más de una vez

        isDead = true; // Marcar como muerto
        Debug.Log("Enemy died. Added " + scoreValue.ToString() + " pts.");
        InvokeDeath();
        Destroy(gameObject);
    }
}
