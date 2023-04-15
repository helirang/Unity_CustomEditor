using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace KFMGT4
{
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
        [SerializeField] Shader materialShader;
        [SerializeField] ShaderData guid;

        [MenuItem("CustomTools/MMD_MaterialSetting")]
        static void Init()
        {
            MMDMaterialSetting wnd = GetWindow<MMDMaterialSetting>();
            wnd.titleContent = new GUIContent("MMD_MaterialSetting");
            wnd.minSize = new Vector2(300, 400);
            wnd.Show();
        }

        void ErrCatch(string message)
        {
            Debug.LogWarning(message);
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
                GUILayout.Label("AlembicModel", EditorStyles.boldLabel);
                alembicModel = (GameObject)EditorGUILayout.ObjectField(alembicModel, typeof(GameObject), true);
                if (GUILayout.Button("Read All MeshRenderers"))
                {
                    SettingMeshRenderer();
                }

                EditorGUILayout.Space(20f);
                materialShader = (Shader)EditorGUILayout.ObjectField(materialShader, typeof(Shader), true);
                if (GUILayout.Button("Setting Start"))
                {
                    SettingMaterial();
                }
                EditorGUILayout.Space(20f);
 
                EditorGUILayout.Space(20f);
                GUILayout.Label("ListData", EditorStyles.boldLabel);
                EditorGUILayout.IntField("MeshRenderer Count", meshRendererlList.Count);
                ListPropertyMaker("meshRendererlList");
                EditorGUILayout.Space(20f);
                EditorGUILayout.IntField("Material Count", materialList.Count);
                ListPropertyMaker("materialList");
            }
        }
        public void SettingMeshRenderer()
        {
            if (alembicModel != null)
                meshRendererlList = new List<MeshRenderer>(alembicModel.GetComponentsInChildren<MeshRenderer>());
            else
                ErrCatch("alembicModel을 찾을 수 없습니다.");
            if (alembicModel != null && meshRendererlList == null)
                ErrCatch("alembicModel에서 meshRenderer를 찾을 수 없습니다.");
        }

        public void SettingMaterial()
        {
            if (alembicModel == null)
            {
                ErrCatch("alembicModel을 찾을 수 없습니다.");
                return;
            }

            string folderPath = CreateFolder();
            if(folderPath != string.Empty)
            {
                CreateMaterial(folderPath);
            }
            else
            {
                ErrCatch("폴더 생성에 실패하였습니다.");
            }
        }

        /// <summary>
        /// 폴더 생성 함수
        /// </summary>
        /// <returns>
        /// <para>폴더 생성에 성공하면 폴더의 GUID를 반환 </para>
        /// <para>실패하면 string.Empty를 반환한다.</para>
        /// </returns>
        public string CreateFolder()
        {
            string path = AssetDatabase.GetAssetPath(alembicModel);
            string folderName = $"{alembicModel.name}_Materials";
            string result = string.Empty;

            if(meshRendererlList.Count <= 0)
            {
                ErrCatch("MeshRenderer 셋팅을 완료해야 작동 가능합니다.");
                return result;
            }

            if(materialShader == null)
            {
                ErrCatch("쉐이더 필드가 비어있습니다. 채워주세요.");
                return result;
            }

            if(path == string.Empty)
            {
                ErrCatch("하이라키 창에 있는 모델은 셋팅 할 수 없습니다.\n" +
                    "프로젝트 창에 있는 모델을 alembicModel에 넣어주세요.");
                return result;
            }

            path = Path.GetDirectoryName(path);

            var createPath = path + @"\" + folderName;

            if (AssetDatabase.IsValidFolder(createPath))
            {
                ErrCatch("이미 있는 폴더입니다. 폴더를 삭제하거나 폴더명을 바꿔주세요.");
            }
            else
            {
                result = AssetDatabase.CreateFolder(path, folderName);
            }
            return result;
        }
        
        void CreateMaterial(string guid)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            string materialName = @"\M";
            for (int i = 0; i < meshRendererlList.Count; i++)
            {
                Material material = new Material(materialShader);
                string newAssetPath = path + materialName + i.ToString() + ".mat";
                AssetDatabase.CreateAsset(material, newAssetPath);
                meshRendererlList[i].material = material;
            }
            Debug.Log(normalMsgs[0]);
        }
    }
}