/*using System.Collections;
using UnityEngine;

public class RifleScript : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform gunTip;
    
    private float bulletSpeed = 20f;
    private int maxShotsBeforeReload = 10;
    private float reloadTime = 1.5f;
    private float shootCooldown = 0.2f;

    private int currentShots = 0;
    private bool isReloading = false;
    private float lastShotTime = 0f;

    private void Update()
    {
        if (!gameObject.activeSelf || isReloading) return;

        if (Input.GetKey(KeyCode.Mouse0) && Time.time >= lastShotTime + shootCooldown)
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
        FireBullet();
        currentShots++;
    }

    private void FireBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, gunTip.position, gunTip.rotation);
        if (bullet != null)
        {
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = gunTip.right * bulletSpeed;
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
}
*/