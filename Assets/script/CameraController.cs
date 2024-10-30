using UnityEngine;

public class CamaraController : MonoBehaviour
{
    private bool isLocked = true;
    private GameObject playerPos;
    private GameObject cursorSprite;
    private Camera camDev;

    public float cameraSpeed = 10f;
    public float edgeThreshold = 10f;
    public float scrollSpeed = 5f;
    public float minZoom = 5f;
    public float maxZoom = 20f;

    private Vector3 moveDirection = Vector3.zero;

    private void Start()
    {
        camDev = Camera.main; // Get camera by tag
        FindCursor();
    }

    private void Update()
    {
        UpdateCursor();
        ToggleLock();

        if (!isLocked)
        {
            HandleCameraMovement(); // Move camera based on mouse position
        }

        HandleCameraZoom(); // Zoom in and out using the scroll wheel
    }

    private void HandleCameraMovement()
    {
        moveDirection = Vector3.zero;
        Vector3 mousePosition = Input.mousePosition;
        
        if (mousePosition.x <= edgeThreshold)
        {
            moveDirection.x = -1;
        }
        
        if (mousePosition.x >= Screen.width - edgeThreshold)
        {
            moveDirection.x = 1;
        }
        
        if (mousePosition.y <= edgeThreshold)
        {
            moveDirection.y = -1;
        }
        
        if (mousePosition.y >= Screen.height - edgeThreshold)
        {
            moveDirection.y = 1;
        }
        
        transform.Translate(moveDirection * cameraSpeed * Time.deltaTime, Space.World);
    }

    private void HandleCameraZoom()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        
        camDev.orthographicSize -= scrollInput * scrollSpeed;
        
        camDev.orthographicSize = Mathf.Clamp(camDev.orthographicSize, minZoom, maxZoom);
    }

    public void FindCursor()
    {
        cursorSprite = GameObject.FindWithTag("Cursor");
    }

    private void UpdateCursor()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 worldMousePos = camDev.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, camDev.nearClipPlane));
        cursorSprite.transform.position = worldMousePos;
        Cursor.visible = false;
    }

    private void ToggleLock()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isLocked = !isLocked;
        }

        if (isLocked)
        {
            playerPos = GameObject.FindGameObjectWithTag("Player");
            if (playerPos != null)
            {
                transform.position = playerPos.transform.position;
            }
        }
    }
}
