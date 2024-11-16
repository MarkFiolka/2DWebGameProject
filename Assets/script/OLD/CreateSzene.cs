using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CreateSzene : NetworkBehaviour
{
    public GameObject userDev;
    public GameObject playerDev;
    public GameObject cursor;
    public GameObject floor;

    private int szeneObjects = 5;
    public GameObject obj;

    private int count = 1;

    void Start()
    {
        // Only the server should set up the scene objects
        if (IsServer)
        {
            SetupSceneObjects();
        }

        // Only the local player should create their player and cursor
        if (IsOwner)
        {
            SetupPlayerAndCursor();
        }
    }

    private void SetupPlayerAndCursor()
    {
        // Instantiate the cursor locally (does not need to be networked if unique per player)
        GameObject newCursor = Instantiate(cursor);

        // Spawn the player object on the network with ownership assigned to the local player
        GameObject newPlayerDev = Instantiate(playerDev);
        NetworkObject networkObject = newPlayerDev.GetComponent<NetworkObject>();
        if (networkObject != null)
        {
            networkObject.SpawnWithOwnership(OwnerClientId); // Assign ownership to the local player
        }
        else
        {
            Debug.LogError("NetworkObject component missing from playerDev prefab.");
        }

        // Set the player as a child of userDev (if required)
        newPlayerDev.transform.SetParent(userDev.transform);
    }

    private void SetupSceneObjects()
    {
        // Instantiate grid objects on the floor (server only), and make them networked objects
        for (int i = 0; i < szeneObjects; i++)
        {
            for (int j = 0; j < szeneObjects; j++)
            {
                GameObject instance = Instantiate(obj, new Vector3(-(szeneObjects / 2) + i, -(szeneObjects / 2) + j, 0), Quaternion.identity);
                instance.transform.SetParent(floor.transform);

                // Spawn the object on the network so that it's visible to all clients
                NetworkObject networkObject = instance.GetComponent<NetworkObject>();
                if (networkObject != null)
                {
                    networkObject.Spawn();
                }
                else
                {
                    Debug.LogError("NetworkObject component missing from scene object prefab.");
                }
                count++;
            }
        }
    }
}
