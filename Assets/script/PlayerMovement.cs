using Unity.Netcode;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] private float moveSpeed = 3f;

    private void Update()
    {
        if (!IsOwner) return; 
        HandleMovement();
    }

    private void HandleMovement()
    {
        Vector3 moveDirection = new Vector3(0, 0, 0);
        
        if (Input.GetKey(KeyCode.W)) moveDirection.x = +1f;
        if (Input.GetKey(KeyCode.D)) moveDirection.x = +1f;
        if (Input.GetKey(KeyCode.S)) moveDirection.x = -1f;
        if (Input.GetKey(KeyCode.A)) moveDirection.x = -1f;

        float moveSpeed = 3f; 
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }
}