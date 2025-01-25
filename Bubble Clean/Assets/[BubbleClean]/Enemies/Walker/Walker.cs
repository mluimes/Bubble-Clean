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
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float projectileLifetime;
    [SerializeField] private Transform firePoint;

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

        // Check if within attack range
        bool isInAttackRange = Vector3.Distance(transform.position, player.position) <= attackRange;

        if (isInAttackRange)
        {
            animator.SetBool("IsWalking", false); // Stop walking animation
            Attack(); // Attack the player
        }
        else
        {
            FollowPlayer(); // Continue moving if not in range
        }
    }

    protected override void FollowPlayer()
    {
        if (player != null)
        {
            // Calculate direction to the player
            Vector3 direction = (player.position - transform.position).normalized;

            animator.SetBool("IsWalking", true); // Set the Walking parameter in the animator

            // Rotate towards the player
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);

            // Move towards the player
            transform.position += direction * speed * Time.deltaTime;
        }
    }

    void Attack()
    {
        // Ensure cooldown is respected
        if (Time.time - lastAttackTime < attackCooldown) return;

        // Instantiate the projectile at the fire point
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
}
