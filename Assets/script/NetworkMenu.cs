using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NetworkMenu  : MonoBehaviour
{
    void Start()
    {
        Screen.fullScreenMode = FullScreenMode.Windowed;

        Screen.SetResolution(250, 250, false);
    }
    
    public void Update()
    {
        //Start as Host with Keypad 1
        if(Input.GetKeyDown(KeyCode.Keypad1))
        {
            NetworkManager.Singleton.StartHost();
        }

        //Start as Client with Keypad 2
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            NetworkManager.Singleton.StartClient();
        }

        //Start as Server with Keypad 3
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            NetworkManager.Singleton.StartServer();
        }
    }
}


/*[SerializeField] private bool isServer = false; // Set this to true for server, false for client

private void Start()
{
    Screen.fullScreenMode = FullScreenMode.Windowed;
    Screen.SetResolution(250, 250, false);

    // Automatically start as a server or client based on the boolean
    if (isServer)
    {
        NetworkManager.Singleton.StartServer();
    }
    else
    {
        NetworkManager.Singleton.StartClient();
    }
}*/