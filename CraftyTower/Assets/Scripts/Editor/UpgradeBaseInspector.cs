using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UpgradeBase))]
public class UpgradeBaseInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        UpgradeBase upgradeBase = (UpgradeBase)target;

        if (GUILayout.Button("Test Rarity"))
        {
            upgradeBase.HitMeDebug();
        }
    }
}