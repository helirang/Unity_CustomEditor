using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace KFMGT4
{
    public class MMDTextureSetting : EditorWindow
    {
        string[] errMsgs = {
        "파일 이름을 입력해 주세요",
        "파일이 존재하지 않습니다",
        "파일 안에 데이터가 없습니다",
        "폴더 경로 오류 또는 폴더 안에 파일이 없습니다.",
        "MMD 데이터 읽기가 비정상적으로 종료되었습니다."
        };

        string[] normalMsgs =
        {
        "텍스쳐 추출 완료",
        "마테리얼 추출이 완료 되었습니다.",
        "모든 텍스쳐 셋팅이 완료되었습니다."
        };

        enum ETexureType
        {
            MainTex,
            NormalTex,
            ToonTex,
            ALL
        }

        [System.Serializable]
        struct MmdData
        {
            public MmdData(int num)
            {
                textArray = new string[(int)ETexureType.ALL];
            }

            public string[] textArray;
        }

        Vector2 scrollPosition;

        [Header("텍스쳐 필드 값")]
        string[] trims = { ".tga", ".png", ".jpg", ".bmp" };
        [SerializeField] TextAsset mmdTextFile;
        [SerializeField] List<string> needTextureList = new List<string>();
        [SerializeField] List<MmdData> texDataList = new List<MmdData>();
        [SerializeField] List<Texture2D> textureList = new List<Texture2D>();

        [Header("마테리얼 필드 값")]
        GameObject alembicModel;
        [SerializeField] GameObject AlembicModel
        {
            get { return alembicModel; }
            set 
            { 
                if(alembicModel != value)
                {
                    alembicModel = value;
                    materialList.Clear();
                }
            }
        }
        [SerializeField] List<Material> materialList = new List<Material>();

        [MenuItem("CustomTools/MMD_TextureSetting")]
        static void Init()
        {
            MMDTextureSetting wnd = GetWindow<MMDTextureSetting>();
            wnd.titleContent = new GUIContent("MMD_TextureSetting");
            wnd.minSize = new Vector2(300, 400);
            wnd.Show();
        }

        void ErrCatch(string message)
        {
            Debug.LogWarning(message);
        }

        void PropertyMaker(string propertyName)
        {
            SerializedObject so = new SerializedObject(this);
            EditorGUILayout.PropertyField(so.FindProperty(propertyName));
        }

        void ListPropertyMaker(string propertyName)
        {
            SerializedObject so = new SerializedObject(this);
            EditorGUILayout.PropertyField(so.FindProperty(propertyName));
            so.ApplyModifiedProperties();
        }

        void OnGUI()
        {
            using (var scrollViewScope =
                new EditorGUILayout.ScrollViewScope(scrollPosition))
            {
                scrollPosition = scrollViewScope.scrollPosition;

                EditorGUILayout.Space(20f);

                GUILayout.Label("TxtureSetting", EditorStyles.boldLabel);
                PropertyMaker("mmdTextFile");
                EditorGUILayout.Space(20f);
                if (GUILayout.Button("Read Only Maint Texture"))
                {
                    ReadTextureData(ETexureType.MainTex);
                }
                if (GUILayout.Button("Read All Texture"))
                {
                    ReadTextureData(ETexureType.ALL);
                }
                EditorGUILayout.Space(20f);
                GUILayout.Label("TxtureData", EditorStyles.boldLabel);
                EditorGUILayout.IntField("ReadTextLine", texDataList.Count);
                ListPropertyMaker("needTextureList");

                EditorGUILayout.Space(20f);
                GUILayout.Label("MaterialSetting", EditorStyles.boldLabel);
                AlembicModel = (GameObject)EditorGUILayout.ObjectField(AlembicModel, typeof(GameObject), true);
                if (GUILayout.Button("Read AlembicModel Material"))
                {
                    SettingMaterial();
                }

                EditorGUILayout.Space(20f);
                GUILayout.Label("MaterialData", EditorStyles.boldLabel);
                EditorGUILayout.IntField("MaterialCount", materialList.Count);
                ListPropertyMaker("materialList");

                EditorGUILayout.Space(20f);
                GUILayout.Label("TextureSetMaterial", EditorStyles.boldLabel);
                EditorGUILayout.IntField("TextureCount", textureList.Count);
                ListPropertyMaker("textureList");
                if (GUILayout.Button("FinalButton"))
                {
                    SettingTexture();
                }
            }
        }

        void SetModel(GameObject gameObject)
        {

        }

        void ReadTextureData(ETexureType readTexureType)
        {
            texDataList.Clear();
            needTextureList.Clear();

            var lines = mmdTextFile.text.Split("\n"[0]);
            int typeNum = 0;

            switch (readTexureType)
            {
                case ETexureType.MainTex:
                    typeNum = (int)ETexureType.MainTex + 1;
                    break;
                case ETexureType.ALL:
                    typeNum = (int)ETexureType.ALL;
                    break;
                default:
                    ErrCatch("현재 메인 텍스쳐와 전체 텍스쳐 읽기만 지원됩니다.");
                    return;
            }

            if (lines != null && lines.Length > 0)
            {
                foreach (string line in lines)
                {
                    //PMX 에디터에서 재질 복사시에 생기는 불필요 데이터 제거
                    //첫번째 줄에 ;Material과 마지막 줄에 빈 line 제거
                    if (line.IndexOf("Material") == -1 || line.IndexOf(";Material")!=-1)
                    {
                        continue;
                    }
                    SettingTextData(line, typeNum);
                }
            }

            Debug.Log(normalMsgs[0]);
        }

        /// <summary>
        /// 텍스트 파일에 있는 텍스쳐 파일명을 읽어들이는 함수.
        /// </summary>
        /// <param name="line"> 텍스트 파일 1줄 데이터 </param>
        /// <param name="textureTypeNum"> 읽을 텍스쳐 타입 개수 </param>
        void SettingTextData(string line, int textureTypeNum)
        {

            //스트링[3] 배열 생성. 배열에 
            MmdData mmdData = new MmdData(textureTypeNum);

            for (int i = 0; i < textureTypeNum; i++)
            {
                int strEnd = -1;

                //trims를 이용해 각 텍스처 데이터 추출
                foreach (var trim in trims)
                {
                    strEnd = line.IndexOf(trim);
                    if (strEnd != -1) break;
                }

                if (strEnd != -1)
                {
                    string data = "";

                    //예) misaki_tex.tga
                    //tga에 a부터 역으로 데이터를 읽어가는 for문
                    for (int j = strEnd + 3; j >= 0; j--)
                    {
                        if (line[j] == '/' || line[j] == '\\' || line[j] == '\"') break;
                        else data = line[j] + data;
                    }

                    //필요한 텍스쳐 이름 추가
                    if (!needTextureList.Contains(data))
                        needTextureList.Add(data);

                    //구조체에 값 셋팅 ( 스트링 배열의 텍스쳐 종류는 ETexureType를 기준으로 한다.)
                    mmdData.textArray[i] = data.Substring(0, data.IndexOf('.', 0));

                    //추출한 텍스트 지우기
                    line = line.Substring(strEnd + 4);
                }
                else break;
            }

            //모든 데이터가 추출된 구조체를 할당
            texDataList.Add(mmdData);
        }

        void SettingTexture()
        {
            string textureName = "";
            Dictionary<string, Texture2D> texDiction = new Dictionary<string, Texture2D>();

            if (texDataList.Count != materialList.Count)
            {
                ErrCatch("ReadTextLine과 materialList 개수를 똑같이 만들어주세요.");
                return;
            }

            foreach (Texture2D tempTex in textureList)
            {
                texDiction.Add(tempTex.name, tempTex);
            }

            for (int i = 0; i < texDataList.Count; i++)
            {
                MmdData mmdData = texDataList[i];
                
                for (int j = 0; j < (int)ETexureType.ALL; j++)
                {
                    textureName = mmdData.textArray[j];
                    if (textureName == string.Empty) continue;
                    if (texDiction.ContainsKey(textureName))
                        materialList[i].mainTexture = texDiction[textureName];
                    else
                    {
                        ErrCatch(i + "번째 이미지가 없습니다. 파일명 : " + textureName);
                        ErrCatch("마테리얼 셋팅이 비정상적으로 종료되었습니다.");
                        return;
                    }
                }
            }
            Debug.Log(normalMsgs[2]);
        }

        public void SettingMaterial()
        {
            List<MeshRenderer> meshRendererlList = null;
            materialList.Clear();

            bool isInSceneObj = true;

            if (alembicModel.scene.path == null || alembicModel.scene.path == string.Empty)
                isInSceneObj = false;

            if (alembicModel != null)
                meshRendererlList = new List<MeshRenderer>(alembicModel.GetComponentsInChildren<MeshRenderer>());
            else
                ErrCatch("alembicModel을 찾을 수 없습니다.");
            if (alembicModel != null && meshRendererlList == null)
                ErrCatch("alembicModel에서 meshRenderer를 찾을 수 없습니다.");

            for (int i = 0; i < meshRendererlList.Count; i++)
            {
                if(isInSceneObj)
                    materialList.Add(meshRendererlList[i].material);
                else
                    materialList.Add(meshRendererlList[i].sharedMaterial);
            }
            Debug.Log(meshRendererlList.Count);
            Debug.Log(normalMsgs[1]);
        }
    }
}