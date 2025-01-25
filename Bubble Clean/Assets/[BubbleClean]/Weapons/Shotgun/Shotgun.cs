using UnityEngine;

public class Shotgun : Weapon
{
    protected override void Awake()
    {
        base.Awake();
        magazineSize = 8;
        fireRate = 1f;
        bulletsPerShot = 6;
        projectileLifetime = 2f;
        spreadAngle = 15f; // Más dispersión
        projectileSpeed = 15f;
        reloadTime = 0.5f;
    }
}
