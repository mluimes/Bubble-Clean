using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public CharacterController Controller; // Asigna el CharacterController aquí
    public Transform CameraTransform; // La cámara del jugador
    public float MouseSensitivity = 100f;
    public float MoveSpeed = 5f;
    public float JumpForce = 5f;
    public float Gravity = -9.81f;

    private float xRotation = 0f; // Para limitar la rotación vertical
    private Vector3 velocity; // Para la gravedad y movimiento
    private bool isGrounded;

    void Awake()
    {
        Application.targetFrameRate = 60;
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Bloquear el cursor
        // Asegurarse de que la cámara apunte hacia adelante
        xRotation = 0f; 
        CameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    void Update()
    {
        // Movimiento de la cámara (rotación)
        float mouseX = Input.GetAxis("Mouse X") * MouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * MouseSensitivity * Time.deltaTime;

        // Limita la rotación vertical
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -70f, 50f);

        // Aplica la rotación a la cámara y al cuerpo
        CameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);

        // Movimiento del jugador (Horizontal y Vertical)
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        // Si el jugador está tocando el suelo, no aplicar gravedad
        isGrounded = Controller.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Mantiene al personaje pegado al suelo
        }

        // Salto
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(JumpForce * -2f * Gravity);
        }

        // Aplica la gravedad
        velocity.y += Gravity * Time.deltaTime;

        // Mueve al jugador con el CharacterController
        Controller.Move(move * MoveSpeed * Time.deltaTime);
        Controller.Move(velocity * Time.deltaTime); // Aplica la gravedad
    }
}
