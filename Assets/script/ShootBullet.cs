using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBullet : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject player;
    public float bulletForce = 100f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            GameObject bullet = Instantiate(bulletPrefab, player.transform.position, player.transform.rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            Vector2 shootDirection = player.transform.up;
            rb.AddForce(shootDirection * bulletForce, ForceMode2D.Impulse);
            Destroy(bullet, 3f);
        }
    }
}