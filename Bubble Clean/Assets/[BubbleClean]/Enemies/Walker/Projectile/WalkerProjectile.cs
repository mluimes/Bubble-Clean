using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkerProjectile : MonoBehaviour
{
    private Rigidbody rb;

    float speed;
    float timeAlive;
    int damage;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Fire(Vector3 direction, float _speed, float _timeAlive, int _damage)
    {
        speed = _speed;
        timeAlive = _timeAlive;
        damage = _damage;
        // anyadir velocidad a la direccion
        direction = direction.normalized;
        rb.velocity = direction * speed;
    }

    private void Update()
    {
        // Reduce el tiempo de vida del proyectil
        timeAlive -= Time.deltaTime;

        float growthFactor = Mathf.Clamp01(timeAlive / 1);
        transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one * 0.6f, growthFactor);

        // Destruye el proyectil cuando su tiempo de vida llega a 0
        if (timeAlive <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            DamagePlayer(other.gameObject);
        }
        Destroy(gameObject);
    }

    private void DamagePlayer(GameObject playerObject)
    {
        var playerHealth = playerObject.GetComponentInChildren<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
            Debug.Log("Player damaged!");
        }
    }
}
