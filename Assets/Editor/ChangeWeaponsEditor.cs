using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(changeweapons))]
public class ChangeWeaponsEditor : Editor
{
    /*public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        changeweapons changeWeaponScript = (changeweapons)target;

        if (changeWeaponScript.wh == null)
        {
            EditorGUILayout.HelpBox("Weapon Holder (wh) is not assigned.", MessageType.Warning);
            return;
        }

        if (GUILayout.Button("Rechten Arm Ausrüsten"))
        {
            changeWeaponScript.wh.equipR(changeWeaponScript.num);
        }

        if (GUILayout.Button("Linken Arm Ausrüsten"))
        {
            changeWeaponScript.wh.equipL(changeWeaponScript.num);
        }
        
        if (GUILayout.Button("Untere ability Ausrüsten"))
        {
            changeWeaponScript.wh.equipD(changeWeaponScript.num);
        }
        
        if (GUILayout.Button("Backpack Ausrüsten"))
        {
            changeWeaponScript.wh.equipBP();
        }
        
        if (GUILayout.Button("Alle Waffen entfernen"))
        {
            changeWeaponScript.wh.RemoveAllWeapons();
        }
    }*/
}