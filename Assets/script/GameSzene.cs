using UnityEngine;
using System.Collections.Generic;

public class GameSzene : MonoBehaviour
{
    private User _user;

    [SerializeField] private GameObject player;
    [SerializeField] private List<GameObject> weaponList;

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

        Transform wrTransformR = playerObj.transform.Find("WR");
        Transform wrTransformL = playerObj.transform.Find("WL");
        SpriteRenderer playerRenderer = playerObj.GetComponent<SpriteRenderer>();
        playerRenderer.sortingOrder = 5;

        foreach (var weapon in weaponList)
        {
            if (weapon.name == _user.WeaponL && wrTransformL != null)
            {
                GameObject weaponObj = Instantiate(weapon);
                weaponObj.transform.SetParent(wrTransformL);
                weaponObj.transform.localPosition = Vector3.zero;
                weaponObj.transform.localRotation = Quaternion.Euler(0, -180, 0);
                SpriteRenderer weaponRenderer = weaponObj.GetComponent<SpriteRenderer>();
                if (weaponRenderer != null)
                {
                    weaponRenderer.sortingOrder = 4;
                }
            }

            if (weapon.name == _user.WeaponR && wrTransformR != null)
            {
                GameObject weaponObj = Instantiate(weapon);
                weaponObj.transform.SetParent(wrTransformR);
                weaponObj.transform.localPosition = Vector3.zero;
                weaponObj.transform.localRotation = Quaternion.Euler(0, 0, 0);
                SpriteRenderer weaponRenderer = weaponObj.GetComponent<SpriteRenderer>();
                if (weaponRenderer != null)
                {
                    weaponRenderer.sortingOrder = 6;
                }
            }
        }
    }
}