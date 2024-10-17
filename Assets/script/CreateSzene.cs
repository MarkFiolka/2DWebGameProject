using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateSzene : MonoBehaviour
{
    public GameObject userDev;
    public GameObject playerDev;
    public GameObject weaponDev;
    public GameObject backpackDev;
    public GameObject cameraDev;

    private int szeneObjects = 100;
    public GameObject obj;
    private int count = 1;
    
    // Start is called before the first frame update
    void Start()
    {
        GameObject newCameraDev = Instantiate(cameraDev);
        newCameraDev.transform.parent = userDev.transform;
        GameObject newPlayerDev = Instantiate(playerDev);
        newPlayerDev.transform.parent = userDev.transform;
        GameObject newWeaponDev = Instantiate(weaponDev);
        newWeaponDev.transform.parent = newPlayerDev.transform;
        GameObject newBackpackDev = Instantiate(backpackDev);
        newBackpackDev.transform.parent = newPlayerDev.transform;

        for (int i = 0; i < szeneObjects; i++)
        {
            for (int j = 0; j < szeneObjects; j++)
            {
                Instantiate(obj, new Vector3(-(szeneObjects/2) + i, -(szeneObjects/2) + j, 0), Quaternion.identity);
                Debug.unityLogger.Log(count + " objects spawned");
                count++;
            }
        }
    }
}
