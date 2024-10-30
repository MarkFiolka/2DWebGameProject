using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBullet : MonoBehaviour
{
    public GameObject bulletPrefab;  // Bullet prefab to shoot
    public float bulletForce;  // Bullet speed
    public float lifetime;
    public float cooldown;
    

    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            Debug.Log(lifetime);
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;
            Vector2 shootDirection = (mousePosition - transform.position).normalized;
            GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.AddForce(shootDirection * bulletForce, ForceMode2D.Impulse);
            Destroy(bullet, lifetime);
        }
    }
}