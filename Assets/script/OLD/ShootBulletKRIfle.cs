using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBulletKRifle : MonoBehaviour
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
    private float magazineSize;
    private float reloadCooldown;

    private bool isReloading = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerRb = player.GetComponent<Rigidbody2D>();

        CheckLeftRight();
        SetStartValues();
    }

    private void SetStartValues()
    {
        // Set default values specific to the rifle
        SetValues(0.95f, 1.5f, 0.3f, 2f, 12f, 15f, 30f, 1.5f); // Default values
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

    // Unified SetValues method to initialize all parameters
    public void SetValues(float oSlowdownValue, float oRecoil, float oCooldown, float oLifetime, float oBulletForce, float oSpread, float oMagazineSize, float oReloadCooldown)
    {
        slowdownValue = oSlowdownValue;
        recoil = oRecoil;
        cooldown = oCooldown;
        lifetime = oLifetime;
        bulletForce = oBulletForce;
        
        magazineSize = oMagazineSize;
        ammo = oMagazineSize;

        reloadCooldown = oReloadCooldown;

        Debug.Log("Values are: " + " slowdownValue: " + slowdownValue + " recoil: " + recoil + " cooldown: " + cooldown + " lifetime: " + lifetime + " bulletForce: " + bulletForce + " magazineSize: " + magazineSize);
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
                magazineSize--; // Decrement magazine size
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
        magazineSize = ammo; // Reset magazine size after reload
        canShoot = true;
        isReloading = false;
        Debug.Log("Reloaded. Magazine size is " + magazineSize);
    }

    private void Shoot()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        Vector2 shootDirection = (mousePosition - transform.position).normalized;
        
        // Calculate the distance between the player and the mouse position
        float distanceToMouse = Vector2.Distance(transform.position, mousePosition);

        // Check if the distance is below a threshold, in which case shoot directly forward
        Vector2 finalShootDirection = distanceToMouse < 2f 
            ? transform.up  // Short distance, shoot forward
            : shootDirection;  // Normal shoot direction toward the mouse

        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(finalShootDirection * bulletForce, ForceMode2D.Impulse);
        Destroy(bullet, lifetime);

        ApplyRecoil(finalShootDirection);
    }

    private void ApplyRecoil(Vector2 shootDirection)
    {
        Vector2 recoilDirection = -shootDirection.normalized; // Opposite direction of the shot
        playerRb.AddForce(recoilDirection * recoil, ForceMode2D.Impulse);
    }
}
