using UnityEngine;

public class WeaponRotation : MonoBehaviour
{
    [SerializeField] private GameObject player; // Reference to the player
    [SerializeField] private Camera mainCamera; // Reference to the main camera

    private SpriteRenderer weaponSprite;
    private SpriteRenderer playerSprite;
    private bool isLeftWeapon; // Determines if the weapon is on the left side
    private bool parentChecked = false; // Ensures parent is checked only once

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
        // Check and assign parent only once after the weapon is instantiated
        if (!parentChecked && transform.parent != null)
        {
            CheckParent();
            parentChecked = true; // Prevent further checks
        }

        if (parentChecked && weaponSprite != null && playerSprite != null)
        {
            HandleWeaponRotation();
            AdjustWeaponLayer();
        }
        else if (playerSprite == null || weaponSprite == null)
        {
            Debug.LogWarning("Skipping Update: Missing required SpriteRenderer references.");
        }
    }

    private void CheckParent()
    {
        Transform parentTransform = transform.parent;
        if (parentTransform != null)
        {
            string parentName = parentTransform.name;
            Debug.Log($"Weapon's parent name: {parentName}");

            if (parentName == "WL")
            {
                isLeftWeapon = true;
            }
            else if (parentName == "WR")
            {
                isLeftWeapon = false;
            }
            else
            {
                Debug.LogError($"Weapon parent name '{parentName}' is not WL or WR. Check your hierarchy setup.");
            }
        }
        else
        {
            Debug.LogError("Weapon does not have a parent. Ensure the weapon is assigned to WL or WR.");
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
        if (isLeftWeapon)
        {
            if (angle > -90 && angle < 90)
            {
                weaponSprite.flipY = true;
            }
            else
            {
                weaponSprite.flipY = false; 
            }
        }
        else
        {
            if (angle > -90 && angle < 90)
            {
                weaponSprite.flipY = false;
            }
            else
            {
                weaponSprite.flipY = true;
            }
        }
    }

    private void AdjustWeaponLayer()
    {
        if (weaponSprite == null || playerSprite == null)
        {
            Debug.LogError("AdjustWeaponLayer: Missing SpriteRenderer reference(s).");
            return;
        }

        if (isLeftWeapon)
        {
            weaponSprite.sortingOrder = playerSprite.sortingOrder - 1;
        }
        else
        {
            weaponSprite.sortingOrder = playerSprite.sortingOrder + 1;
        }
    }

    private float AngleBetweenPoints(Vector2 a, Vector2 b)
    {
        return Mathf.Atan2(b.y - a.y, b.x - a.x) * Mathf.Rad2Deg;
    }
}
