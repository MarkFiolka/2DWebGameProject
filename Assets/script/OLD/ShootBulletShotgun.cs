using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBulletShotgun : MonoBehaviour
{
    public GameObject bulletPrefab;
    private float timeSinceLastShot = 0f;
    private GameObject player;
    private Rigidbody2D playerRb;
    private bool isFrozen;
    private bool canShoot = true;
    private bool isLeft = false;
    private bool isRight = false;
    private float ammo;

    private float slowdownValue;
    private float recoil;
    private float cooldown;
    private float lifetime;
    private float bulletForce;
    private float spread;
    private float magazineSize;
    private float reloadCooldown;

    private float spreadAngle;
    private int shotsRemaining;
    private bool isReloading = false;
    private int bulletCount;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerRb = player.GetComponent<Rigidbody2D>();

        CheckLeftRight();
        SetStartValues();
    }

    private void SetStartValues()
    {
        SetValues(0.95f, 3f, 0.5f, 1f, 10f, 10f, 2f, 3f, 10f, 3, 5); // Default values
    }

    private void CheckLeftRight()
    {
        if (transform.parent.tag == "right")
        {
            isRight = true;
            isLeft = false;
        }
        else if (transform.parent.tag == "left")
        {
            isRight = false;
            isLeft = true;
        }
    }

    public void SetValues(float oSlowdownValue, float oRecoil, float oCooldown, float oLifetime, float oBulletForce, float oSpread, float oMagazineSize, float oReloadCooldown, float oSpreadAngle, int oShotsRemaining, int oBulletCount)
    {
        slowdownValue = oSlowdownValue;
        recoil = oRecoil;
        cooldown = oCooldown;
        lifetime = oLifetime;
        bulletForce = oBulletForce;
        spread = oSpread;

        magazineSize = oMagazineSize;
        ammo = oMagazineSize;
        
        reloadCooldown = oReloadCooldown;
        spreadAngle = oSpreadAngle;
        shotsRemaining = oShotsRemaining;
        bulletCount = oBulletCount;

        Debug.Log("Values are: " + " slowdownValue: " + slowdownValue + " recoil: " + recoil + " cooldown: " + cooldown + " lifetime: " + lifetime + " bulletForce: " + bulletForce + " spread: " + spread + " spreadAngle: " + spreadAngle + " shotsRemaining: " + shotsRemaining + " bulletCount: " + bulletCount);
    }

    void Update()
    {
        ApplySlowdown();

        if (isReloading) return;

        timeSinceLastShot += Time.deltaTime;

        if (timeSinceLastShot >= cooldown && magazineSize > 0 && canShoot)
        {
            if ((Input.GetKey(KeyCode.Mouse0) && isLeft) || (Input.GetKey(KeyCode.Mouse1) && isRight))
            {
                Shoot();
                timeSinceLastShot = 0f;
                shotsRemaining--;
                magazineSize--;
            }
        }
        else if (magazineSize <= 0 && !isReloading)
        {
            StartCoroutine(Reload());
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            isFrozen = !isFrozen;
            Time.timeScale = isFrozen ? 0 : 1;
            SetStartValues();
        }
    }

    private void ApplySlowdown()
    {
        if (playerRb.velocity.magnitude > 0.01f)
        {
            playerRb.velocity *= slowdownValue;
        }
        else
        {
            playerRb.velocity = Vector2.zero;
        }
    }

    private IEnumerator Reload()
    {
        isReloading = true;
        canShoot = false;
        Debug.Log("Reloading...");
        yield return new WaitForSeconds(reloadCooldown);
        magazineSize = ammo;
        shotsRemaining = (int)ammo;
        canShoot = true;
        isReloading = false;
        Debug.Log("Reloaded. Magazine size: " + magazineSize + ", Shots remaining: " + shotsRemaining);
    }

    private void Shoot()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        Vector2 shootDirection = (mousePosition - transform.position).normalized;

        float distanceToMouse = Vector2.Distance(transform.position, mousePosition);
        float randomAngle = Random.Range(-spread, spread);

        if (distanceToMouse < 2f)
        {
            for (int i = 0; i < bulletCount; i++)
            {
                float angle = randomAngle + (spreadAngle / (bulletCount - 1)) * (i - bulletCount / 2);
                Vector2 finalShootDirection = Quaternion.Euler(0, 0, angle) * transform.up;

                GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                rb.AddForce(finalShootDirection * bulletForce, ForceMode2D.Impulse);
                Destroy(bullet, lifetime);
            }
        }
        else
        {
            float angleStep = spreadAngle / (bulletCount - 1);
            float startAngle = -spreadAngle / 2;

            for (int i = 0; i < bulletCount; i++)
            {
                float angle = startAngle + (angleStep * i);
                Vector2 finalShootDirection = Quaternion.Euler(0, 0, angle) * shootDirection;

                GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                rb.AddForce(finalShootDirection * bulletForce, ForceMode2D.Impulse);
                Destroy(bullet, lifetime);
            }
        }

        ApplyRecoil();
    }

    private void ApplyRecoil()
    {
        Vector2 fDirection = -playerRb.transform.up;
        playerRb.AddForce(fDirection * recoil, ForceMode2D.Impulse);
    }
}
