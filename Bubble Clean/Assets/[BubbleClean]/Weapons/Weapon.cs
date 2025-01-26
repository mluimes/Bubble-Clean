using System.Collections;
using UnityEngine.UI;
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

    public Transform gunMouth;

    protected int currentAmmo;
    protected float lastShotTime;
    private Animator animator;
    private ParticleSystem muzzleFlash;

    [SerializeField] private Image magazineBar;

    protected virtual void Awake()
    {
        currentAmmo = magazineSize;
        animator = GetComponentInChildren<Animator>();

        if (gunMouth == null)
        {
            Debug.LogError($"gunMouth not found in {gameObject.name}. Please add it to the prefab.");
        }
        else
        {
            muzzleFlash = gunMouth.GetComponentInChildren<ParticleSystem>();
        }
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

            if (animator != null)
            {
                animator.SetTrigger("Shoot");
            }

            muzzleFlash.Play();
            var projectile = Instantiate(projectilePrefab, gunMouthPosition, Quaternion.LookRotation(randomDirection));
            projectile.Fire(projectileSpeed, randomDirection);
            projectile.SetLifetime(projectileLifetime); // Usar el tiempo de vida definido para esta arma
        }

        currentAmmo--;
        UpdateUI();
        lastShotTime = Time.time;

        if (currentAmmo == 0)
        {
            Debug.Log("Out of ammo!");
            Reload();
        }
    }

    public virtual void Reload()
    {
        if(currentAmmo == magazineSize)
            return;
        if (animator != null)
        {
            animator.SetTrigger("Reload");
        }
        StartCoroutine(ReloadCoroutine());
        Debug.Log("Reloading...");
    }

    private IEnumerator ReloadCoroutine()
    {
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = magazineSize;
        UpdateUI();
    }

    void UpdateUI() {
        magazineBar.fillAmount = 1/(float)magazineSize * (float)currentAmmo;
    }

    public Vector3 GetGunMouthPosition()
    {
        if (gunMouth != null)
        {
            return gunMouth.position;
        }

        Debug.LogError("gunMouth is not assigned!");
        return Vector3.zero;
    }
}
