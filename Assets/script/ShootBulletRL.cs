using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class ShootBulletRL : NetworkBehaviour
{
    [Header("Rocket Settings")]
    public GameObject rocketPrefab;
    public string weaponNameBase = "RocketLauncher";

    private string weaponName;
    private float slowdownValue, recoil, cooldown, lifetime, rocketForce, reloadCooldown, magazineSize;
    private float timeSinceLastShot;

    private Rigidbody2D playerRb;

    private NetworkVariable<int> currentAmmo = new NetworkVariable<int>();
    private NetworkVariable<bool> isReloading = new NetworkVariable<bool>(false); // Sync reload state
    private WeaponDatabase weaponDatabase;

    private bool IsLeftWeapon => transform.parent.CompareTag("left");
    private bool IsRightWeapon => transform.parent.CompareTag("right");

    void Start()
    {
        InitializeWeapon();
        InitializeAmmo();
    }

    void Update()
    {
        if (!IsOwner || isReloading.Value) return;

        HandleShootingInput();
    }

    private void InitializeWeapon()
    {
        playerRb = GetComponentInParent<Rigidbody2D>();

        if (IsLeftWeapon)
        {
            weaponName = $"{weaponNameBase}_L";
        }
        else if (IsRightWeapon)
        {
            weaponName = $"{weaponNameBase}_R";
        }
        else
        {
            Debug.LogError("Parent object must have either 'left' or 'right' tag.");
            return;
        }

        weaponDatabase = new WeaponDatabase();
        LoadWeaponValues(weaponName);
    }

    private void InitializeAmmo()
    {
        currentAmmo.Value = (int)magazineSize;
        currentAmmo.OnValueChanged += OnAmmoChanged;
        isReloading.OnValueChanged += OnReloadingChanged;
    }

    private void LoadWeaponValues(string weaponName)
    {
        WeaponData data = weaponDatabase.GetWeaponData(weaponName);
        if (data != null)
        {
            slowdownValue = data.SlowdownValue;
            recoil = data.Recoil;
            cooldown = data.Cooldown;
            lifetime = data.Lifetime;
            rocketForce = data.RocketForce;
            reloadCooldown = data.ReloadCooldown;
            magazineSize = data.MagazineSize;
        }
        else
        {
            Debug.LogError($"Weapon data for '{weaponName}' not found in database.");
        }
    }

    private void HandleShootingInput()
    {
        timeSinceLastShot += Time.deltaTime;

        bool shouldShoot = (IsLeftWeapon && Input.GetMouseButton(0)) || (IsRightWeapon && Input.GetMouseButton(1));
        if (shouldShoot && timeSinceLastShot >= cooldown && currentAmmo.Value > 0)
        {
            timeSinceLastShot = 0f;
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 shootDirection = (mousePosition - transform.position).normalized;

            RequestShootServerRpc(shootDirection);
        }
        else if (currentAmmo.Value <= 0 && !isReloading.Value)
        {
            RequestReloadServerRpc();
        }
    }

    [ServerRpc]
    private void RequestShootServerRpc(Vector2 shootDirection)
    {
        if (currentAmmo.Value <= 0 || isReloading.Value) return;

        currentAmmo.Value--;

        GameObject rocket = Instantiate(rocketPrefab, transform.position, Quaternion.identity);
        Rigidbody2D rb = rocket.GetComponent<Rigidbody2D>();
        rb.AddForce(shootDirection.normalized * rocketForce, ForceMode2D.Impulse);

        NetworkObject rocketNetworkObject = rocket.GetComponent<NetworkObject>();
        if (rocketNetworkObject != null)
        {
            rocketNetworkObject.Spawn();
        }

        Destroy(rocket, lifetime);
    }

    [ServerRpc]
    private void RequestReloadServerRpc()
    {
        if (!isReloading.Value)
        {
            StartCoroutine(Reload());
        }
    }

    private IEnumerator Reload()
    {
        isReloading.Value = true;
        Debug.Log($"Reloading {weaponName}...");

        yield return new WaitForSeconds(reloadCooldown);

        if (IsServer)
        {
            currentAmmo.Value = (int)magazineSize;
        }

        Debug.Log($"{weaponName} reloaded!");
        isReloading.Value = false;
    }

    private void OnAmmoChanged(int oldValue, int newValue)
    {
        Debug.Log($"Ammo updated: {newValue}/{magazineSize}");
    }

    private void OnReloadingChanged(bool oldValue, bool newValue)
    {
        Debug.Log(newValue ? $"Reloading started for {weaponName}" : $"Reloading finished for {weaponName}");
    }
}
