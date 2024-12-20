using UnityEngine;
using System.Collections.Generic;

public class GameSzene : MonoBehaviour
{
    private User _user;

    [SerializeField] private GameObject player;
    [SerializeField] private List<GameObject> weaponList;

    private GameObject mainWeaponObj;
    private GameObject secondaryWeaponObj;
    private Transform mainWeaponTransform;
    private Transform secondaryWeaponTransform;
    private bool isMainWeaponActive = true;

    public void SetUser(User user)
    {
        _user = user;
    }

    public void InstantiateSzene()
    {
        if (_user == null)
        {
            Debug.LogError("User data is not set in GameSzene.");
            return;
        }

        PullAndStoreDBData();
    }

    private void PullAndStoreDBData()
    {
        if (player == null)
        {
            Debug.LogError("Player prefab is not assigned.");
            return;
        }

        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        GameObject playerObj = Instantiate(player, new Vector3(_user.PosX, _user.PosY, 0), Quaternion.identity);

        mainWeaponTransform = playerObj.transform.Find("MainWeapon");
        secondaryWeaponTransform = playerObj.transform.Find("SecondWeapon");
        SpriteRenderer playerRenderer = playerObj.GetComponent<SpriteRenderer>();
        playerRenderer.sortingOrder = 5;

        foreach (var weapon in weaponList)
        {
            if (weapon.name == _user.MainWeapon && mainWeaponTransform != null)
            {
                mainWeaponObj = Instantiate(weapon, mainWeaponTransform);
                SetupWeapon(mainWeaponObj, mainWeaponTransform, isActive: true, -180);
            }

            if (weapon.name == _user.SecondWeapon && secondaryWeaponTransform != null)
            {
                secondaryWeaponObj = Instantiate(weapon, secondaryWeaponTransform);
                SetupWeapon(secondaryWeaponObj, secondaryWeaponTransform, isActive: false, 0);
            }
        }
    }

    private void SetupWeapon(GameObject weaponObj, Transform parentTransform, bool isActive, float rotationY)
    {
        weaponObj.transform.SetParent(parentTransform);
        weaponObj.transform.localPosition = Vector3.zero;
        weaponObj.transform.localRotation = Quaternion.Euler(0, rotationY, 0);
        weaponObj.SetActive(isActive);

        SpriteRenderer weaponRenderer = weaponObj.GetComponent<SpriteRenderer>();
        if (weaponRenderer != null)
        {
            weaponRenderer.sortingOrder = 6;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            SwapWeapons();
        }
    }

    private void SwapWeapons()
    {
        if (mainWeaponObj == null || secondaryWeaponObj == null)
        {
            Debug.LogWarning("Weapons are not properly initialized.");
            return;
        }

        isMainWeaponActive = !isMainWeaponActive;

        mainWeaponObj.SetActive(isMainWeaponActive);
        secondaryWeaponObj.SetActive(!isMainWeaponActive);

        _user.MainWeapon = isMainWeaponActive ? mainWeaponObj.name : secondaryWeaponObj.name;
        _user.SecondWeapon = isMainWeaponActive ? secondaryWeaponObj.name : mainWeaponObj.name;

        Debug.Log($"Swapped weapons: Main is now {_user.MainWeapon}, Secondary is now {_user.SecondWeapon}");
    }
}
