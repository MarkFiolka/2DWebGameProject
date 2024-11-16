using UnityEngine;

public class changeweapons : MonoBehaviour
{
    public WeaponHolder wh;  // Make sure this matches the class name of WeaponHolder
    public int num = 0;

    void Start()
    {
        wh = GetComponent<WeaponHolder>();
    }
}