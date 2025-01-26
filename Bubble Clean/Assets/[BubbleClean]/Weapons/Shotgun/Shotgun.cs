using UnityEngine;

public class Shotgun : Weapon
{
    protected override void Awake()
    {
        base.Awake();
        magazineSize = 2;
        fireRate = 1f;
        bulletsPerShot = 6;
        projectileLifetime = 1f;
        spreadAngle = 15f; // Más dispersión
        projectileSpeed = 10f;
        reloadTime = 1.95f;
    }
}
