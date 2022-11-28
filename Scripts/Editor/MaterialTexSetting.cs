using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class MaterialTexSetting : EditorWindow
{
    string[] errMsgs = {
        "���� �̸��� �Է��� �ּ���",
        "������ �������� �ʽ��ϴ�",
        "���� �ȿ� �����Ͱ� �����ϴ�",
        "���� ��� ���� �Ǵ� ���� �ȿ� ������ �����ϴ�.",
    };

    string[] normalMsgs =
    {
        "���� �б� �Ϸ�",
        "�ؽ��� ���� ������ �Ϸ�Ǿ����ϴ�."
    };

    enum PropertyNames
    {
        dataList,
        materialList,
        textureList
    }
    string[] scrollPropertysName = {
        "dataList",
        "materialList",
        "textureList"
    };
    Vector2[] scrollPositions = { new Vector2(0,0), new Vector2(0,0), new Vector2(0,0)};

    string filePath,fileName;
    bool groupEnabled;
    //public Vector2 scrollPosition = new Vector2(0,0);
    [SerializeField] List<string> dataList = new List<string>();
    [SerializeField] List<Material> materialList = new List<Material>();
    [SerializeField] List<Texture2D> textureList = new List<Texture2D>();

    [MenuItem("Window/MaterialTextSetting")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        MaterialTexSetting window = (MaterialTexSetting)EditorWindow.GetWindow(typeof(MaterialTexSetting));
        window.Show();
    }

    void CatchERR(string message)
    {
        Debug.LogWarning(message);
    }

    void ListPropertyMaker(string propertyName)
    {
        //to show the list of geo
        ScriptableObject target = this;
        SerializedObject so = new SerializedObject(target);
        SerializedProperty stringsProperty = so.FindProperty(propertyName);
        if (stringsProperty.isExpanded) 
        {
            int idx = (int)System.Enum.Parse(typeof(PropertyNames), propertyName);
            scrollPositions[idx] = GUILayout.BeginScrollView(scrollPositions[idx], GUILayout.Width(300), GUILayout.Height(200));
            EditorGUILayout.PropertyField(stringsProperty, true);
            so.ApplyModifiedProperties();
            GUILayout.EndScrollView();
        }
        else
        {
            EditorGUILayout.PropertyField(stringsProperty, true);
            so.ApplyModifiedProperties();
        }
    }

    void OnGUI()
    {
        GUILayout.Label("Base Function", EditorStyles.boldLabel);
        fileName = EditorGUILayout.TextField("TextFileName", fileName);
        if (GUILayout.Button("Read Text Test"))
        {
            ReadMaTex(fileName);
        }
        if (GUILayout.Button("Material Tex Setting"))
        {
            SettingTexture();
        }
        GUILayout.Label("ListData", EditorStyles.boldLabel);
        ListPropertyMaker("dataList");
        ListPropertyMaker("materialList");
        ListPropertyMaker("textureList");
        groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", groupEnabled);
        filePath = EditorGUILayout.TextField("FilePath", filePath);
        EditorGUILayout.EndToggleGroup();
    }

    void ReadMaTex(string fName)
    {
        dataList.Clear();
        if (fName == null)
        {
            CatchERR(errMsgs[0]);
            return;
        }
        string filePath = Path.Combine(Application.streamingAssetsPath, fName);
        FileInfo fileInfo = new FileInfo(filePath);
        if (!fileInfo.Exists)
        {
            CatchERR(fName + errMsgs[1]);
            return;
        }
        string[] lines = fileInfo.Exists ? File.ReadAllLines(filePath) : null;
        
        if(lines != null && lines.Length > 0)
        {
            string strTrim = string.Format(@"""Tex\");
            string subTrim = string.Format(@"""tex/");
            foreach (string line in lines)
            {
                int strStart = line.IndexOf(strTrim) == -1 ?
                    line.IndexOf(subTrim) : line.IndexOf(strTrim);
                if (strStart != -1)
                {
                    strStart += strTrim.Length;
                    int strEnd = line.IndexOf('"', strStart);
                    string data = line.Substring(strStart, strEnd - strStart);
                    dataList.Add(data.Substring(0, data.IndexOf('.', 0)));
                }
                else
                {
                    dataList.Add("none");
                }
            }
            Debug.Log(normalMsgs[0]);
        }
        else
        {
            CatchERR(errMsgs[2]);
        }
    }

    void SettingTexture()
    {
        bool isNormal = true;
        Dictionary<string, Texture2D> texDiction = new Dictionary<string, Texture2D>();

        if(dataList.Count != materialList.Count)
        {
            Debug.Log("dataList�� materialList ������ �Ȱ��� ������ּ���.");
            return;
        }

        foreach (Texture2D tempTex in textureList)
        {
            texDiction.Add(tempTex.name, tempTex);
        }

        for(int i=0; i< dataList.Count; i++)
        {
            string key = dataList[i];

            if(texDiction.ContainsKey(key)) materialList[i].mainTexture = texDiction[key];
            else if(key != "none")
            {
                Debug.Log(i + "��° �̹����� �����ϴ�. ���ϸ� : " + key);
                isNormal = false;
                break;
            }
        }

        if (isNormal) Debug.Log(normalMsgs[1]);
        else Debug.LogWarning("���׸��� ������ ������������ ����Ǿ����ϴ�.");
    }
}