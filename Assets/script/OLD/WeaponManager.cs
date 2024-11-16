using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class WeaponManager : NetworkBehaviour
{
    public GameObject[] weaponPrefabs; // Array of weapon prefabs to choose from
    private List<NetworkObject> spawnedWeapons = new List<NetworkObject>(); // Track spawned NetworkObjects

    private Transform posR;
    private Transform posL;
    private Transform posD;

    private void Start()
    {
        if (IsOwner)
        {
            // Find the player's child transforms for weapon positions
            posR = transform.Find("posR");
            posL = transform.Find("posL");
            posD = transform.Find("posD");

            if (posR == null || posL == null || posD == null)
            {
                Debug.LogError("Weapon positions (posR, posL, posD) are not assigned on the player prefab.");
            }
        }
    }

    [ServerRpc]
    public void SpawnWeaponServerRpc(int weaponIndex, string positionName, ServerRpcParams rpcParams = default)
    {
        if (weaponIndex < 0 || weaponIndex >= weaponPrefabs.Length)
        {
            Debug.LogError("Invalid weapon index.");
            return;
        }

        Transform spawnPosition = null;
        if (positionName == "posR") spawnPosition = posR;
        else if (positionName == "posL") spawnPosition = posL;
        else if (positionName == "posD") spawnPosition = posD;

        if (spawnPosition == null)
        {
            Debug.LogError("Invalid position name or position transform not found.");
            return;
        }

        GameObject weaponInstance = Instantiate(weaponPrefabs[weaponIndex], spawnPosition.position, spawnPosition.rotation);
        weaponInstance.transform.SetParent(spawnPosition);

        var networkObject = weaponInstance.GetComponent<NetworkObject>();
        if (networkObject != null)
        {
            networkObject.SpawnWithOwnership(rpcParams.Receive.SenderClientId);
            spawnedWeapons.Add(networkObject);
        }
        else
        {
            Debug.LogError("Weapon prefab is missing a NetworkObject component.");
        }
    }

    [ServerRpc]
    public void DespawnWeaponServerRpc(NetworkObjectReference weaponReference, ServerRpcParams rpcParams = default)
    {
        if (IsServer && weaponReference.TryGet(out NetworkObject networkObject))
        {
            if (networkObject != null && networkObject.IsSpawned && spawnedWeapons.Contains(networkObject))
            {
                networkObject.Despawn();
                spawnedWeapons.Remove(networkObject);
            }
        }
    }

    [ServerRpc]
    public void ClearAllWeaponsServerRpc()
    {
        foreach (var weapon in spawnedWeapons)
        {
            if (weapon != null && weapon.IsSpawned)
            {
                weapon.Despawn();
            }
        }
        spawnedWeapons.Clear();
    }

    public void RequestSpawnWeapon(int weaponIndex, string positionName)
    {
        if (IsOwner)
        {
            SpawnWeaponServerRpc(weaponIndex, positionName);
        }
    }

    public void RequestDespawnWeapon(NetworkObject weapon)
    {
        if (IsOwner && weapon != null)
        {
            DespawnWeaponServerRpc(weapon);
        }
    }

    public void RequestClearAllWeapons()
    {
        if (IsOwner)
        {
            ClearAllWeaponsServerRpc();
        }
    }
}
