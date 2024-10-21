using System.Collections.Generic;
using UnityEngine;

public class weaponholder : MonoBehaviour
{
    public GameObject[] weaponR; // Weapons for right// Weapons for bottom
    public GameObject[] weaponL; // Weapons for left
    public GameObject[] weaponD;
    public GameObject backpackD;
    
    public GameObject posR;
    public GameObject posD;
    public GameObject posL;
    

    private GameObject player;

    // List to keep track of instantiated weapons
    private List<GameObject> spawnedWeapons = new List<GameObject>();

    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Equip right weapon
    public void equipR(int num)
    {
        if (num >= 0 && num < weaponR.Length)
        {
            GameObject instance = Instantiate(weaponR[num], posR.transform.position, posR.transform.rotation);
            instance.transform.SetParent(player.transform); // Set parent on the instantiated object
            spawnedWeapons.Add(instance); // Add the weapon to the list
        }
        else
        {
            Debug.LogError("Invalid index for weaponR");
        }
    }
    
    public void equipD(int num)
    {
        if (num >= 0 && num < weaponD.Length)
        {
            GameObject instance = Instantiate(weaponD[num], posD.transform.position, posD.transform.rotation);
            instance.transform.SetParent(player.transform); // Set parent on the instantiated object
            spawnedWeapons.Add(instance); // Add the weapon to the list
        }
        else
        {
            Debug.LogError("Invalid index for weaponR");
        }
    }

    // Equip bottom weapon
    public void equipBP()
    { 
        GameObject instance = Instantiate(backpackD, player.transform.position, player.transform.rotation);
        instance.transform.SetParent(player.transform); // Set parent on the instantiated object
        spawnedWeapons.Add(instance); // Add the weapon to the list
    }

    // Equip left weapon
    public void equipL(int num)
    {
        if (num >= 0 && num < weaponL.Length)
        {
            GameObject instance = Instantiate(weaponL[num], posL.transform.position, posL.transform.rotation);
            instance.transform.SetParent(player.transform); // Set parent on the instantiated object
            spawnedWeapons.Add(instance); // Add the weapon to the list
        }
        else
        {
            Debug.LogError("Invalid index for weaponL");
        }
    }

    // Method to remove all spawned weapons
    public void RemoveAllWeapons()
    {
        foreach (GameObject weapon in spawnedWeapons)
        {
            if (weapon != null)
            {
                Destroy(weapon); // Destroy each instantiated weapon
            }
        }

        // Clear the list after all weapons are destroyed
        spawnedWeapons.Clear();
    }
}
