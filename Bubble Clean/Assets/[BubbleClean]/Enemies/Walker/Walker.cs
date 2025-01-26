using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walker : Enemy
{
    private Animator animator;
    private float lastAttackTime;

    [Header("Projectile Settings")]
    [SerializeField] private GameObject walkerProjectile;
    [SerializeField] private int projectileDamage;
    [SerializeField] private float projectileSpeed = 15f;
    [SerializeField] private float projectileLifetime;
    [SerializeField] private Transform firePoint;

    [Header("Audio Settings")]
    [SerializeField] private AudioSource audioSource; // Un solo AudioSource para todos los sonidos
    [SerializeField] private AudioClip walkClip; // El clip del sonido de caminar
    [SerializeField] private AudioClip attackClip;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>(); // Ensure the animator is assigned
        }
    }

    void Update()
    {
        if (player == null) return;

        // Check if the player is within attack range
        bool isInAttackRange = Vector3.Distance(transform.position, player.position) <= attackRange;

        if (isInAttackRange)
        {
            // Always rotate to face the player while in attack range, ignoring vertical movement
            Vector3 directionToPlayer = player.position - transform.position;
            directionToPlayer.y = 0; // Ignore vertical movement
            if (Vector3.Angle(transform.forward, directionToPlayer) > 5f)
            {
                RotateTowardsPlayer();
                animator.SetBool("IsWalking", true);
            }
            else
            {
                animator.SetBool("IsWalking", false); // Stop walking if facing the player
            }

            // Attack the player (if cooldown allows)
            Attack();
        }
        else
        {
            FollowPlayer(); // Continue moving if not in range
        }

        // Check if the enemy is "walking" (moving or rotating)
        // bool isWalking = !isInAttackRange || animator.GetBool("IsWalking");
        // animator.SetBool("IsWalking", isWalking);
    }

    void RotateTowardsPlayer()
    {
        if (player == null) return;

        // Calculate direction to the player
        Vector3 direction = (player.position - transform.position).normalized;

        // Smoothly rotate towards the player
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);

    }

    void Attack()
    {
        // Ensure cooldown is respected
        if (Time.time - lastAttackTime < attackCooldown) return;

        // Reproducir el sonido de disparo
        if (audioSource != null && attackClip != null)
        {
            audioSource.clip = attackClip;
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("No se ha asignado el sonido de disparo.");
        }


        // Start attack animation and fire projectile
        if (walkerProjectile != null && firePoint != null)
        {
            StartCoroutine(FireProjectileWithDelay());
        }

        lastAttackTime = Time.time; // Reset the attack cooldown timer

    }

    IEnumerator FireProjectileWithDelay()
    {
        animator.SetTrigger("Attack");
        Debug.Log("Firing projectile...");
        yield return new WaitForSeconds(0.4f); // Adjust the delay as needed
        var projectile = Instantiate(walkerProjectile, firePoint.position, firePoint.rotation);
        projectile.GetComponent<WalkerProjectile>().Fire(firePoint.forward, projectileSpeed, projectileLifetime, projectileDamage);
    }


    protected override void FollowPlayer()
    {
        if (player != null)
        {
            // Calcular la dirección hacia el jugador
            Vector3 direction = (player.position - transform.position).normalized;

            animator.SetBool("IsWalking", true); // Activar el parámetro "IsWalking" en el animator

            // Reproducir el sonido de caminar si el enemigo se está moviendo
            if (!audioSource.isPlaying || audioSource.clip != walkClip)  // Evitar que suene el audio varias veces
            {
                audioSource.clip = walkClip;
                audioSource.Play();
            }

            // Rotar hacia el jugador
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);

            // Moverse hacia el jugador
            transform.position += direction * speed * Time.deltaTime;
        }
        else
        {
            // Detener el sonido de caminar si no se está moviendo
            animator.SetBool("IsWalking", false);
            if (audioSource.isPlaying && audioSource.clip == walkClip)
            {
                audioSource.Stop();
            }
        }
    }
}
