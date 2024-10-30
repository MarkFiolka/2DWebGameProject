using System.Collections.Generic;
using UnityEngine;

public class weaponholder : MonoBehaviour
{
    public GameObject[] weaponR;
    public GameObject[] weaponL;
    public GameObject[] weaponD;
    public GameObject backpackD;
    
    public GameObject posR;
    public GameObject posD;
    public GameObject posL;
    

    private GameObject player;

    private List<GameObject> spawnedWeapons = new List<GameObject>();

    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void equipR(int num)
    {
        if (num >= 0 && num < weaponR.Length)
        {
            GameObject instance = Instantiate(weaponR[num], posR.transform.position, posR.transform.rotation);
            instance.transform.SetParent(player.transform);
            spawnedWeapons.Add(instance);
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
            instance.transform.SetParent(player.transform);
            spawnedWeapons.Add(instance);
        }
        else
        {
            Debug.LogError("Invalid index for weaponR");
        }
    }

    public void equipBP()
    { 
        GameObject instance = Instantiate(backpackD, player.transform.position, player.transform.rotation);
        instance.transform.SetParent(player.transform);
        spawnedWeapons.Add(instance);
    }

    
    public void equipL(int num)
    {
        if (num >= 0 && num < weaponL.Length)
        {
            GameObject instance = Instantiate(weaponL[num], posL.transform.position, posL.transform.rotation);
            instance.transform.SetParent(player.transform);
            spawnedWeapons.Add(instance);
        }
        else
        {
            Debug.LogError("Invalid index for weaponL");
        }
    }

    public void RemoveAllWeapons()
    {
        foreach (GameObject weapon in spawnedWeapons)
        {
            if (weapon != null)
            {
                Destroy(weapon);
            }
        }

        spawnedWeapons.Clear();
    }
}
