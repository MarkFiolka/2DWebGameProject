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
        }
    }
}