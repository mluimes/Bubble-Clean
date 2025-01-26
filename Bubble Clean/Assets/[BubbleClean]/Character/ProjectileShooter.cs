using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileShooter : MonoBehaviour
{
    [SerializeField] private Transform weaponSlot; // Donde se colocan las armas
    [SerializeField] private Transform cameraTransform; // Para la dirección del disparo
    private List<Weapon> weapons = new List<Weapon>(); // Todas las armas
    private int currentWeaponIndex = 0; // Índice del arma actual
    private Weapon equippedWeapon; // Arma actualmente equipada

    private void Start()
    {
        // Buscar todas las armas en el WeaponSlot y desactivarlas
        foreach (Transform weapon in weaponSlot)
        {
            var weaponScript = weapon.GetComponent<Weapon>();
            if (weaponScript != null)
            {
                weapons.Add(weaponScript);
                weapon.gameObject.SetActive(false);
            }
        }

        // Equipar la primera arma de la lista (si hay alguna)
        if (weapons.Count > 0)
        {
            EquipWeapon(0);
        }
        else
        {
            Debug.LogError("No weapons found in WeaponSlot!");
        }
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && equippedWeapon != null)
        {
            Vector3 shootDirection = cameraTransform.forward.normalized;
            equippedWeapon.Fire(shootDirection, equippedWeapon.GetGunMouthPosition());
        }

        if (Input.GetKeyDown(KeyCode.R) && equippedWeapon != null)
        {
            equippedWeapon.Reload();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            CycleWeapon(1); // Cambiar al arma siguiente
        }
    }

    private void EquipWeapon(int index)
    {
        if (index < 0 || index >= weapons.Count)
        {
            Debug.LogError("Weapon index out of range!");
            return;
        }

        if (equippedWeapon != null)
        {
            equippedWeapon.gameObject.SetActive(false); // Desactivar el arma actual
        }

        equippedWeapon = weapons[index]; // Asignar la nueva arma
        equippedWeapon.gameObject.SetActive(true); // Activar la nueva arma
        currentWeaponIndex = index;

        Debug.Log($"Equipped weapon: {equippedWeapon.name}");
    }

    public void CycleWeapon(int direction)
    {
        int newIndex = (currentWeaponIndex + direction) % weapons.Count;
        if (newIndex < 0) newIndex += weapons.Count; // Asegurar ciclo inverso

        EquipWeapon(newIndex);
    }

    public void EquipSpecificWeapon(Weapon weaponPrefab)
    {
        if (equippedWeapon != null)
        {
            equippedWeapon.gameObject.SetActive(false);
        }

        // Buscar si ya existe una instancia del prefab
        foreach (Weapon weapon in weapons)
        {
            if (weapon.name == weaponPrefab.name)
            {
                equippedWeapon = weapon;
                equippedWeapon.gameObject.SetActive(true);
                Debug.Log($"Equipped existing weapon: {equippedWeapon.name}");
                return;
            }
        }

        // Si no existe, instanciar y añadirla a la lista
        var newWeapon = Instantiate(weaponPrefab, weaponSlot);
        weapons.Add(newWeapon);
        equippedWeapon = newWeapon;
        Debug.Log($"Equipped new weapon: {equippedWeapon.name}");
    }
}
