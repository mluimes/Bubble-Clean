using UnityEngine;

public class Pistol : Weapon
{
    protected override void Awake()
    {
        base.Awake();
        magazineSize = 12;
        fireRate = 3f;
        bulletsPerShot = 1;
        projectileLifetime = 3f;
        spreadAngle = 0;
        projectileSpeed = 15f;
        reloadTime = 2.4f;
    }
}
