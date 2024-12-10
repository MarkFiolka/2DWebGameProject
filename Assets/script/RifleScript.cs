using System.Collections;
using UnityEngine;

public class RifleScript : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform gunTip;
    private int level = 1;

    private float bulletSpeed;
    private int bulletsPerShot;
    private float spreadAngle;
    private int maxShotsBeforeReload;
    private float reloadTime;
    private float bulletDamage;
    private float shootCooldown;
    private float maxDistance;
    private float criticalChance;

    private int currentShots = 0;
    private bool isReloading = false;
    private float lastShotTime = 0f;
    private bool isRightGun = false;
    private bool isLeftGun = false;

    private void Start()
    {
        string parentName = transform.parent?.name;
        if (parentName == "WR") isRightGun = true;
        if (parentName == "WL") isLeftGun = true;

        SetWeaponStats(level);
    }

    private void Update()
    {
        if (isReloading) return;

        if (isRightGun && Input.GetKey(KeyCode.Mouse1) && Time.time >= lastShotTime + shootCooldown)
        {
            Shoot();
        }
        else if (isLeftGun && Input.GetKey(KeyCode.Mouse0) && Time.time >= lastShotTime + shootCooldown)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        if (currentShots >= maxShotsBeforeReload)
        {
            StartCoroutine(Reload());
            return;
        }

        lastShotTime = Time.time;
        FireBullets();
        currentShots++;
    }

    private void FireBullets()
    {
        float angleStep = bulletsPerShot > 1 ? spreadAngle / (bulletsPerShot - 1) : 0f;
        float startAngle = bulletsPerShot > 1 ? -spreadAngle / 2 : 0f;

        for (int i = 0; i < bulletsPerShot; i++)
        {
            float angleOffset = startAngle + (i * angleStep);
            Quaternion rotation = transform.rotation * Quaternion.Euler(0, 0, angleOffset);
            Vector3 direction = rotation * Vector3.right;

            SpawnBullet(direction);
        }
    }

    private void SpawnBullet(Vector3 direction)
    {
        GameObject bullet = Instantiate(bulletPrefab, gunTip.position, Quaternion.identity);
        if (bullet != null)
        {
            BulletScript bulletScript = bullet.GetComponent<BulletScript>();
            IdentificationScript bulletIdScript = bullet.GetComponent<IdentificationScript>();

            if (bulletScript != null && bulletIdScript != null)
            {
                string bulletId = System.Guid.NewGuid().ToString();

                float damage = bulletDamage;
                if (Random.value <= criticalChance)
                {
                    damage *= 2; // Critical hit
                    Debug.Log("Critical Hit!");
                }

                // Assign shooter and bullet IDs
                string shooterId = GetComponent<IdentificationScript>().GetPlayerId();
                bulletScript.Initialize(direction, bulletSpeed, damage, maxDistance, shooterId, bulletId);
                bulletIdScript.SetPlayerId(shooterId);
                bulletIdScript.SetBulletId(bulletId);
            }
            else
            {
                Debug.LogError("BulletScript or IdentificationScript missing on bullet prefab!");
            }
        }
        else
        {
            Debug.LogError("Failed to instantiate bullet!");
        }
    }

    private IEnumerator Reload()
    {
        isReloading = true;
        yield return new WaitForSeconds(reloadTime);
        currentShots = 0;
        isReloading = false;
    }

    private void SetWeaponStats(int level)
    {
        bulletSpeed = 5f + (level * 0.5f) + Mathf.Pow(level, 1.05f);
        bulletsPerShot = Mathf.Clamp(1 + Mathf.FloorToInt(level / 8), 1, 12);
        spreadAngle = Mathf.Max(30f - (level * 0.25f), 3f);
        maxShotsBeforeReload = Mathf.Min(20 + (level * 2), 150);
        reloadTime = Mathf.Max(2f - (level * 0.02f), 0.3f);
        bulletDamage = 10f + (level * 1.5f) + Mathf.Pow(level, 1.3f);
        shootCooldown = Mathf.Max(0.5f - (level * 0.004f), 0.05f);
        maxDistance = 10f + (level * 0.5f);
        criticalChance = Mathf.Clamp(level * 0.02f, 0f, 1f);
    }

    public void UpgradeWeapon(int newLevel)
    {
        level = Mathf.Clamp(newLevel, 1, 100);
        SetWeaponStats(level);
    }
}
