using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MaterialSetting))]
public class MaterialSettingButton : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        MaterialSetting maSet = (MaterialSetting)target;
        if (GUILayout.Button("MeshRenderers Setting"))
        {
            maSet.SettinMeshRenderer();
        }
        if (GUILayout.Button("Materials Setting"))
        {
            maSet.SettingMaterial();
        }

    }
}
