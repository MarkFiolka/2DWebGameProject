using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateSzene : MonoBehaviour
{
    public GameObject userDev;
    public GameObject playerDev;
    public GameObject cursor;

    private CamaraController cameraController;  // Declare the CameraController reference

    private int szeneObjects = 100;
    public GameObject obj;

    private int count = 1;
    void Start()
    {
        GameObject newCursor = Instantiate(cursor);
        
        cameraController = FindObjectOfType<CamaraController>();

        if (cameraController != null)
        {
            cameraController.FindCursor();
        }
        else
        {
            Debug.LogError("CameraController not found!");
        }
        
        GameObject newPlayerDev = Instantiate(playerDev);
        newPlayerDev.transform.parent = userDev.transform;
        
        for (int i = 0; i < szeneObjects; i++)
        {
            for (int j = 0; j < szeneObjects; j++)
            {
                Instantiate(obj, new Vector3(-(szeneObjects / 2) + i, -(szeneObjects / 2) + j, 0), Quaternion.identity);
                count++;
            }
        }
    }
}