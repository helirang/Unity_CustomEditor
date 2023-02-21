using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using System.IO;

public class MMDTextureSetting : EditorWindow
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

    public Vector2 scrollPosition;

    string filePath,fileName;
    bool groupEnabled;
    //public Vector2 scrollPosition = new Vector2(0,0);
    [SerializeField] List<string> dataList = new List<string>();
    [SerializeField] List<Material> materialList = new List<Material>();
    [SerializeField] List<Texture2D> textureList = new List<Texture2D>();

    [MenuItem("CustomTools/MMD_TextureSetting")]
    static void Init()
    {
        MMDTextureSetting wnd = GetWindow<MMDTextureSetting>();
        wnd.titleContent = new GUIContent("MMD_TextureSetting");
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
    }

    void ReadMaTex(string fName)
    {
        dataList.Clear();
        if (fName == null)
        {
            ErrCatch(errMsgs[0]);
            return;
        }
        string filePath = Path.Combine(Application.streamingAssetsPath, fName);
        FileInfo fileInfo = new FileInfo(filePath);
        if (!fileInfo.Exists)
        {
            ErrCatch(fName + errMsgs[1]);
            return;
        }
        string[] lines = fileInfo.Exists ? File.ReadAllLines(filePath) : null;
        
        if(lines != null && lines.Length > 0)
        {
            string strTrim = string.Format(".png");
            string subTrim = string.Format(".jpg");
            foreach (string line in lines)
            {
                int strEnd = line.IndexOf(strTrim) == -1 ?
                    line.IndexOf(subTrim) : line.IndexOf(strTrim);
                if (strEnd != -1)
                {
                    string data = "";
                    for (int i = strEnd + 3; i >= 0; i--)
                    {
                        if (line[i] == '/' || line[i] == '\\' || line[i] == '\"') break;
                        else data = line[i] + data;
                    }
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
            ErrCatch(errMsgs[2]);
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
