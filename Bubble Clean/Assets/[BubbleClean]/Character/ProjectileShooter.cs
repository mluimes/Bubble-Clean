using System.Collections.Generic;
using UnityEngine;

public class ProjectileShooter : MonoBehaviour
{
    [SerializeField] private Transform hand;
    [SerializeField] private Transform cameraTransform; // Cámara para dirección de disparo

    private Weapon equippedWeapon;
    private Transform gunMouth;

    private void Start()
    {
        if (hand != null)
        {
            foreach (Transform child in hand)
            {
                gunMouth = child.Find("gunMouth");
                if (gunMouth != null)
                    break;
            }

            if (gunMouth == null)
                Debug.LogError("gunMouth object not found in any child of 'hand'.");
        }
        else
        {
            Debug.LogError("Hand is not assigned in the Inspector.");
        }

        // Equipar la primera arma por defecto (puedes agregar un sistema para cambiar armas)
        equippedWeapon = hand.GetComponentInChildren<Weapon>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && equippedWeapon != null)
        {
            Vector3 shootDirection = cameraTransform.forward.normalized;
            equippedWeapon.Fire(shootDirection, gunMouth.position);
        }

        if (Input.GetKeyDown(KeyCode.R) && equippedWeapon != null)
        {
            equippedWeapon.Reload();
        }
    }
}
