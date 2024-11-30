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

        Screen.SetResolution(1920/2, 1080/2, false);
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