using System.Collections;
using UnityEditor.Animations;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public int magazineSize;
    public float fireRate;
    public int bulletsPerShot;
    public float projectileLifetime;
    public Projectile projectilePrefab;
    public float spreadAngle; // Útil para armas como la escopeta
    public float projectileSpeed = 10f;
    public float reloadTime;
    protected int currentAmmo;
    protected float lastShotTime;
    Animator animator;

    protected virtual void Awake()
    {
        currentAmmo = magazineSize;
        animator = GetComponent<Animator>();
    }

    public virtual void Fire(Vector3 shootDirection, Vector3 gunMouthPosition)
    {
        if (Time.time - lastShotTime < 1 / fireRate || currentAmmo <= 0)
            return;

        for (int i = 0; i < bulletsPerShot; i++)
        {
            Vector3 randomDirection = shootDirection;

            if (spreadAngle > 0)
            {
                randomDirection = Quaternion.Euler(
                    Random.Range(-spreadAngle, spreadAngle),
                    Random.Range(-spreadAngle, spreadAngle),
                    0
                ) * shootDirection;
            }

            animator.SetTrigger("Shoot");
            var projectile = Instantiate(projectilePrefab, gunMouthPosition, Quaternion.LookRotation(randomDirection));
            projectile.Fire(projectileSpeed, randomDirection);
            projectile.SetLifetime(projectileLifetime); // Usar el tiempo de vida definido para esta arma
        }

        currentAmmo--;
        lastShotTime = Time.time;

        if (currentAmmo == 0)
        {
            Debug.Log("Out of ammo!");
            Reload();
        }
    }

    public virtual void Reload()
    {
        animator.SetTrigger("Reload");
        StartCoroutine(ReloadCoroutine());
        Debug.Log("Reloading...");
    }

    private IEnumerator ReloadCoroutine()
    {
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = magazineSize;
    }
}
