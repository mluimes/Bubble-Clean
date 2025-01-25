using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walker : Enemy
{
    private Animator animator;
    private float lastAttackTime;

    [Header("Projectile Settings")]
    [SerializeField] private GameObject walkerProjectile;
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
        FollowPlayer();

        // // Check if within attack range and cooldown has passed
        // if (Vector3.Distance(transform.position, player.position) <= _attackRange && Time.time - lastAttackTime >= attackCooldown)
        // {
        //     Attack();
        // }
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
        // Trigger the Fire event in the animator

        // Instantiate the projectile at the fire point
        if (walkerProjectile != null && firePoint != null)
        {
            animator.SetTrigger("Attack");
            StartCoroutine(FireProjectileWithDelay());
            Instantiate(walkerProjectile, firePoint.position, firePoint.rotation);
        }

        lastAttackTime = Time.time; // Reset the attack cooldown timer
    }

    IEnumerator FireProjectileWithDelay()
    {
        yield return new WaitForSeconds(0.4f); // Adjust the delay as needed
        Instantiate(walkerProjectile, firePoint.position, firePoint.rotation);
    }
}
