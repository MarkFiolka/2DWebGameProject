using UnityEditor;
using UnityEngine;

public class changeqeapons : MonoBehaviour
{
    private weaponholder wh;
    
    public int num = 0;

    void Start()
    {
        wh = GetComponent<weaponholder>();
    }

    [CustomEditor(typeof(changeqeapons))]
    public class TestButtonScriptEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            changeqeapons changeWeaponScript = (changeqeapons)target;

            // Button to equip right weapon
            if (GUILayout.Button("Rechten Arm Ausrüsten"))
            {
                changeWeaponScript.wh.equipR(changeWeaponScript.num);
            }

            // Button to equip left weapon
            if (GUILayout.Button("Linken Arm Ausrüsten"))
            {
                changeWeaponScript.wh.equipL(changeWeaponScript.num);
            }
            
            // Button to remove all weapons
            if (GUILayout.Button("Untere ability Ausrüsten"))
            {
                changeWeaponScript.wh.equipD(changeWeaponScript.num);
            }
            
            // Button to equip bottom weapon
            if (GUILayout.Button("Backpack Ausrüsten"))
            {
                changeWeaponScript.wh.equipBP();
            }
            
            // Button to remove all weapons
            if (GUILayout.Button("Alle Waffen entfernen"))
            {
                changeWeaponScript.wh.RemoveAllWeapons();
            }
        }
    }
}