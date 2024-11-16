using UnityEngine;
using Unity.Netcode;

public class CameraController : NetworkBehaviour
{
    public GameObject cursorPrefab;
    private GameObject cursor;

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            cursor = Instantiate(cursorPrefab);
        }
    }

    void Update()
    {
        if (!IsOwner) return;

        if (cursor != null)
        {
            UpdateCursor();
        }
        else
        {
            Debug.LogWarning("Cursor is not set in CameraController.");
        }
    }

    private void UpdateCursor()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Camera.main.nearClipPlane));
        cursor.transform.position = new Vector3(worldPosition.x, worldPosition.y, cursor.transform.position.z);
    }
}