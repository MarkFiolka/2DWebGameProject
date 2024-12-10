using UnityEngine;
using Unity.Netcode;

public class CameraFollowPlayer : MonoBehaviour
{
    private Transform target; 
    [SerializeField] private Vector3 offset = new Vector3(0, 0, -10);
    [SerializeField] private float smoothSpeed = 5f;
    private void Start()
    {
        AssignLocalPlayer();
    }

    private void LateUpdate()
    {
        if (target == null)
        {
            AssignLocalPlayer();
            return;
        }

        Vector3 desiredPosition = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
    }

    private void AssignLocalPlayer()
    {
        foreach (var playerObject in GameObject.FindGameObjectsWithTag("Player"))
        {
            var networkObject = playerObject.GetComponent<NetworkObject>();
            if (networkObject != null && networkObject.IsOwner)
            {
                target = playerObject.transform;
                Debug.Log("CameraFollowPlayer: Assigned local player as target.");
                break;
            }
        }

        if (target == null)
        {
            Debug.LogWarning("CameraFollowPlayer: No local player found yet.");
        }
    }
}