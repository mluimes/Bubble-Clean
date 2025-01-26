using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance { get; private set; }

    private Weapon currentlyEquippedWeapon;
    private List<WeaponBox> weaponBoxes = new List<WeaponBox>();

    private ProjectileShooter projectileShooter;

    private void Awake()
    {
        projectileShooter = FindObjectOfType<ProjectileShooter>();
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RegisterWeaponBox(WeaponBox box)
    {
        if (!weaponBoxes.Contains(box))
        {
            weaponBoxes.Add(box);
        }
    }

    public void EquipWeapon(Weapon newWeapon)
    {
        // Desactivar el arma equipada actualmente
        if (currentlyEquippedWeapon != null)
        {
            currentlyEquippedWeapon.gameObject.SetActive(false);
        }

        // Activar la nueva arma
        currentlyEquippedWeapon = newWeapon;
        currentlyEquippedWeapon.gameObject.SetActive(true);

        projectileShooter.EquipSpecificWeapon(newWeapon);

        // Actualizar el estado en todas las cajas
        UpdateWeaponBoxes();
    }

    private void UpdateWeaponBoxes()
    {
        foreach (var box in weaponBoxes)
        {
            box.UpdateBoxState(currentlyEquippedWeapon);
        }
    }

    public Weapon GetCurrentlyEquippedWeapon()
    {
        return currentlyEquippedWeapon;
    }
}
