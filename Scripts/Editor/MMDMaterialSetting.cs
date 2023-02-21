using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using System.IO;

public class MMDMaterialSetting : EditorWindow
{

    string[] normalMsgs =
    {
        "마테리얼 셋팅이 완료되었습니다."
    };

    public Vector2 scrollPosition;
    [SerializeField] GameObject alembicModel;
    [SerializeField] List<Material> materialList = new List<Material>();
    [SerializeField] List<MeshRenderer> meshRendererlList = new List<MeshRenderer>();

    [MenuItem("CustomTools/MMD_MaterialSetting")]
    static void Init()
    {
        MMDMaterialSetting wnd = GetWindow<MMDMaterialSetting>();
        wnd.titleContent = new GUIContent("MMD_MaterialSetting");
        wnd.Show();
    }

    void ErrCatch(string message)
    {
        Debug.LogWarning(message);
    }

    void ListPropertyMaker(string propertyName)
    {
        //to show the list of geo
        ScriptableObject target = this;
        SerializedObject so = new SerializedObject(target);
        EditorGUILayout.PropertyField(so.FindProperty(propertyName));
        so.ApplyModifiedProperties();
    }

    void OnGUI()
    {
        using (var scrollViewScope = 
            new EditorGUILayout.ScrollViewScope(scrollPosition))
        {
            scrollPosition = scrollViewScope.scrollPosition;
            if (GUILayout.Button("Read All MeshRenderers"))
            {
                SettinMeshRenderer();
            }
            if (GUILayout.Button("Setting Start"))
            {
          
            }
            GUILayout.Label("AlembicModel", EditorStyles.boldLabel);
            alembicModel = (GameObject)EditorGUILayout.ObjectField(alembicModel, typeof(GameObject),true);
            GUILayout.Label("ListData", EditorStyles.boldLabel);
            ListPropertyMaker("materialList");
            ListPropertyMaker("meshRendererlList");
        }
    }
    public void SettinMeshRenderer()
    {
        if(alembicModel != null)
            meshRendererlList = new List<MeshRenderer>(alembicModel.GetComponentsInChildren<MeshRenderer>());
        else
            ErrCatch("alembicModel을 찾을 수 없습니다.");
        if (alembicModel != null && meshRendererlList == null)
            ErrCatch("alembicModel에서 meshRenderer를 찾을 수 없습니다.");
    }

    public void SettingMaterial()
    {
        if (meshRendererlList == null)
        {
            ErrCatch("meshRenderer가 셋팅되지 않았습니다.");
            return;
        }
        if (meshRendererlList.Count != materialList.Count)
        {
            ErrCatch("meshRenderers와 materials의 개수가 다릅니다. 동일하게 맞춰주세요.");
            return;
        }

        for (int i = 0; i < meshRendererlList.Count; i++)
        {
            meshRendererlList[i].material = materialList[i];
        }
        Debug.Log(normalMsgs[0]);
    }
}
