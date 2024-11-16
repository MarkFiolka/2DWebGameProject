using UnityEngine;
using System.Collections.Generic;
using Unity.Netcode;

public class WeaponHolder : NetworkBehaviour
{
    public GameObject[] weaponR;
    public GameObject[] weaponL;
    public GameObject[] weaponD;
    public GameObject backpackD;

    public GameObject posR;
    public GameObject posD;
    public GameObject posL;

    private List<GameObject> spawnedWeapons = new List<GameObject>();

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            // Initialize any client-specific setup if necessary
        }
    }

    [ServerRpc]
    public void EquipRServerRpc(int num, ulong clientId)
    {
        if (num >= 0 && num < weaponR.Length)
        {
            GameObject instance = Instantiate(weaponR[num], posR.transform.position, posR.transform.rotation);
            instance.transform.SetParent(posR.transform);
            var networkObject = instance.GetComponent<NetworkObject>();
            if (networkObject != null)
            {
                networkObject.SpawnWithOwnership(clientId);
                spawnedWeapons.Add(instance);
            }
        }
        else
        {
            Debug.LogError("Invalid index for weaponR");
        }
    }

    [ServerRpc]
    public void EquipLServerRpc(int num, ulong clientId)
    {
        if (num >= 0 && num < weaponL.Length)
        {
            GameObject instance = Instantiate(weaponL[num], posL.transform.position, posL.transform.rotation);
            instance.transform.SetParent(posL.transform);
            var networkObject = instance.GetComponent<NetworkObject>();
            if (networkObject != null)
            {
                networkObject.SpawnWithOwnership(clientId);
                spawnedWeapons.Add(instance);
            }
        }
        else
        {
            Debug.LogError("Invalid index for weaponL");
        }
    }

    [ServerRpc]
    public void EquipDServerRpc(int num, ulong clientId)
    {
        if (num >= 0 && num < weaponD.Length)
        {
            GameObject instance = Instantiate(weaponD[num], posD.transform.position, posD.transform.rotation);
            instance.transform.SetParent(posD.transform);
            var networkObject = instance.GetComponent<NetworkObject>();
            if (networkObject != null)
            {
                networkObject.SpawnWithOwnership(clientId);
                spawnedWeapons.Add(instance);
            }
        }
        else
        {
            Debug.LogError("Invalid index for weaponD");
        }
    }

    [ServerRpc]
    public void EquipBPServerRpc(ulong clientId)
    {
        GameObject instance = Instantiate(backpackD, transform.position, transform.rotation);
        instance.transform.SetParent(transform);
        var networkObject = instance.GetComponent<NetworkObject>();
        if (networkObject != null)
        {
            networkObject.SpawnWithOwnership(clientId);
            spawnedWeapons.Add(instance);
        }
    }

    [ServerRpc]
    public void RemoveAllWeaponsServerRpc()
    {
        foreach (GameObject weapon in spawnedWeapons)
        {
            if (weapon != null)
            {
                NetworkObject networkObject = weapon.GetComponent<NetworkObject>();
                if (networkObject != null && networkObject.IsSpawned)
                {
                    networkObject.Despawn();
                }
            }
        }
        spawnedWeapons.Clear();
    }

    // These methods can be called by the local player to initiate server RPCs
    public void EquipR(int num)
    {
        if (IsOwner)
        {
            EquipRServerRpc(num, OwnerClientId);
        }
    }

    public void EquipL(int num)
    {
        if (IsOwner)
        {
            EquipLServerRpc(num, OwnerClientId);
        }
    }

    public void EquipD(int num)
    {
        if (IsOwner)
        {
            EquipDServerRpc(num, OwnerClientId);
        }
    }

    public void EquipBP()
    {
        if (IsOwner)
        {
            EquipBPServerRpc(OwnerClientId);
        }
    }

    public void RemoveAllWeapons()
    {
        if (IsOwner)
        {
            RemoveAllWeaponsServerRpc();
        }
    }
}
