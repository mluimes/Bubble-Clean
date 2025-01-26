using UnityEngine;

public class Rifle : Weapon
{
    protected override void Awake()
    {
        base.Awake();
        magazineSize = 30;
        fireRate = 6f; // Más rápido
        bulletsPerShot = 1;
        projectileLifetime = 2f;
        spreadAngle = 2f; // Dispersión leve
        projectileSpeed = 15f;
        reloadTime = 3f;
    }

    override public void Fire(Vector3 shootDirection, Vector3 gunMouthPosition)
    {
        base.Fire(shootDirection, gunMouthPosition);
    }
}
