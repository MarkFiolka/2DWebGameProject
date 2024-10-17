using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraCOntroller : MonoBehaviour
{
    private GameObject playerPos;
    
    // Start is called before the first frame update
    void Start()
    {
        playerPos = GameObject.FindGameObjectWithTag("userDev");
    }

    // Update is called once per frame
    void Update()
    {
        playerPos.transform.position = new Vector3(playerPos.transform.position.x, playerPos.transform.position.y, 0);
    }
}