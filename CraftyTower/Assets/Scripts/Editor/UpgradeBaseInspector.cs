using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ArrowUpgrade))]
public class UpgradeBaseInspector : Editor
{
    public override void OnInspectorGUI()
    {
        ArrowUpgrade upgradeBase = (ArrowUpgrade)target;

        EditorGUILayout.LabelField("Generate Rarity", "Generate a upgrade of each rarty.");
        if (GUILayout.Button("Test Rarity"))
        {
            upgradeBase.HitMeDebug();
        }
        DrawDefaultInspector();
    }
}