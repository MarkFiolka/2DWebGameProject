using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraCOntroller : MonoBehaviour
{
    public GameObject playerPos;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(playerPos.transform.position.x, playerPos.transform.position.y, 0);
    }
}