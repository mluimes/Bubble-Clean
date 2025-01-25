using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody rb;
    private float timeAlive = 0f; // Tiempo desde que el proyectil fue disparado
    private Vector3 initialDirection;

    // Valor para randomizar la oscilación
    private float randomOscillationStart;

    [SerializeField] private float oscillationSpeed = 1f;
    [SerializeField] private float oscillationAmount = 0.1f;
    [SerializeField] private float timeAliveLimit = 3f;
    [SerializeField] private float maxSize = 1f;  // Tamaño máximo de la burbuja
    [SerializeField] private float growthTime = .3f; // Tiempo para alcanzar el tamaño máximo

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        randomOscillationStart = Random.Range(0f, Mathf.PI * 2);  // Generar un valor aleatorio entre 0 y 2pi
        transform.localScale = Vector3.zero; // Empezar con tamaño 0
    }

    public void Fire(float speed, Vector3 direction)
    {
        initialDirection = direction.normalized;  // Guardamos la dirección inicial
        rb.velocity = initialDirection * speed;  // Movemos el proyectil hacia adelante
    }

    private void Update()
    {
        timeAlive += Time.deltaTime;

        // Asegurarse de que el proyectil dure 4 segundos
        if (timeAlive >= timeAliveLimit)
        {
            Destroy(gameObject);
            return;
        }

        // crecimiento en el tiempo
        float growthFactor = Mathf.Clamp01(timeAlive / growthTime);
        transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one * maxSize, growthFactor);


        // Movimiento en espiral: hacerlo más rápido con un inicio aleatorio
        float oscillation = Mathf.Sin((timeAlive * oscillationAmount) + randomOscillationStart) * oscillationSpeed;  // Seno aleatorio para cada proyectil

        // Añadir la oscilación al movimiento
        Vector3 spiralMovement = initialDirection + transform.right * oscillation;
        spiralMovement += transform.up * oscillation;

        // Aplicar la velocidad original y la oscilación
        rb.velocity = spiralMovement * rb.velocity.magnitude;
    }

    private void OnCollisionEnter(Collision other)
    {
        Destroy(gameObject);  // Destruir el proyectil al colisionar
    }

    public void SetLifetime(float lifetime)
    {
        timeAliveLimit = lifetime;
    }
}
