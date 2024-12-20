/*using UnityEngine;

public class WeaponRotation : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Camera mainCamera;

    private SpriteRenderer weaponSprite;
    private SpriteRenderer playerSprite;

    private void Start()
    {
        weaponSprite = GetComponent<SpriteRenderer>();
        if (weaponSprite == null)
        {
            Debug.LogError("SpriteRenderer is not attached to the weapon object.");
        }

        if (player != null)
        {
            playerSprite = player.GetComponent<SpriteRenderer>();
            if (playerSprite == null)
            {
                Debug.LogError("SpriteRenderer is not attached to the player object.");
            }
        }
        else
        {
            Debug.LogError("Player reference is missing. Assign the player GameObject in the Inspector.");
        }
    }

    private void Update()
    {
        if (!gameObject.activeSelf) return;

        if (weaponSprite != null && playerSprite != null)
        {
            HandleWeaponRotation();
            AdjustWeaponLayer();
        }
        else
        {
            Debug.LogWarning("Skipping Update: Missing required SpriteRenderer references.");
        }
    }

    private void HandleWeaponRotation()
    {
        Vector3 mouseScreenPosition = Input.mousePosition;

        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(
            mouseScreenPosition.x,
            mouseScreenPosition.y,
            Mathf.Abs(Camera.main.transform.position.z - transform.position.z)
        ));

        float angle = AngleBetweenPoints(transform.position, mouseWorldPosition);
        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));

        FlipWeaponSprite(angle);
    }

    private void FlipWeaponSprite(float angle)
    {
        weaponSprite.flipY = angle > 90 || angle < -90;
    }

    private void AdjustWeaponLayer()
    {
        if (weaponSprite == null || playerSprite == null)
        {
            Debug.LogError("AdjustWeaponLayer: Missing SpriteRenderer reference(s).");
            return;
        }

        weaponSprite.sortingOrder = playerSprite.sortingOrder + 1;
    }

    private float AngleBetweenPoints(Vector2 a, Vector2 b)
    {
        return Mathf.Atan2(b.y - a.y, b.x - a.x) * Mathf.Rad2Deg;
    }
}*/
