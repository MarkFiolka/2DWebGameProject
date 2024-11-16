using System.Collections.Generic;
using UnityEngine;

public class WeaponDatabase
{
    private Dictionary<string, WeaponData> weaponData;

    public WeaponDatabase()
    {
        // Initialize sample data
        weaponData = new Dictionary<string, WeaponData>
        {
            { "RocketLauncher_L", new WeaponData(0.9f, 5f, 1.5f, 5f, 20f, 3f, 2) },
            { "RocketLauncher_R", new WeaponData(0.9f, 10f, 0.5f, 5f, 20f, 1f, 4) },
        };
    }

    public WeaponData GetWeaponData(string weaponName)
    {
        if (weaponData.ContainsKey(weaponName))
        {
            return weaponData[weaponName];
        }

        Debug.LogError($"Weapon '{weaponName}' not found in database.");
        return null;
    }
}

public class WeaponData
{
    public float SlowdownValue { get; private set; }
    public float Recoil { get; private set; }
    public float Cooldown { get; private set; }
    public float Lifetime { get; private set; }
    public float RocketForce { get; private set; }
    public float ReloadCooldown { get; private set; }
    public float MagazineSize { get; private set; }

    public WeaponData(float slowdownValue, float recoil, float cooldown, float lifetime, float rocketForce, float reloadCooldown, float magazineSize)
    {
        SlowdownValue = slowdownValue;
        Recoil = recoil;
        Cooldown = cooldown;
        Lifetime = lifetime;
        RocketForce = rocketForce;
        ReloadCooldown = reloadCooldown;
        MagazineSize = magazineSize;
    }
}