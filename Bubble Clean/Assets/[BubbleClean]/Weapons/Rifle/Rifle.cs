using UnityEngine;

public class Rifle : Weapon
{
    protected override void Awake()
    {
        base.Awake();
        magazineSize = 30;
        fireRate = 0.1f; // Más rápido
        bulletsPerShot = 1;
        projectileLifetime = 5f;
        spreadAngle = 2f; // Dispersión leve
        projectileSpeed = 25f;
        reloadTime = 3f;
    }
}
