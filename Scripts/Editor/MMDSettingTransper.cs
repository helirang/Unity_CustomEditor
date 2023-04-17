using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace KFMGT4
{
    public class MMDSettingTransper : EditorWindow
    {
        [SerializeField] GameObject baseModel;
        [SerializeField] GameObject targetModel;
        public Vector2 scrollPosition;

        [MenuItem("CustomTools/MMD_SettingTransper")]
        static void Init()
        {
            MMDSettingTransper wnd = GetWindow<MMDSettingTransper>();
            wnd.titleContent = new GUIContent("MMD_SettingTransper");
            wnd.minSize = new Vector2(300, 400);
            wnd.Show();
        }

        void ErrCatch(string message)
        {
            Debug.LogWarning(message);
        }

        void OnGUI()
        {
            using (var scrollViewScope =
                new EditorGUILayout.ScrollViewScope(scrollPosition))
            {
                scrollPosition = scrollViewScope.scrollPosition;

                EditorGUILayout.Space(20f);
                GUILayout.Label("BaseModel", EditorStyles.boldLabel);
                baseModel = (GameObject)EditorGUILayout.ObjectField(baseModel, typeof(GameObject), true);
                GUILayout.Label("TargetModel", EditorStyles.boldLabel);
                targetModel = (GameObject)EditorGUILayout.ObjectField(targetModel, typeof(GameObject), true);

                EditorGUILayout.Space(20f);
                GUILayout.Label("Data Base -> Target", EditorStyles.boldLabel);
                if (GUILayout.Button("Material Transper"))
                {
                    SettingCopy();
                }
            }
        }

        void SettingCopy()
        {
            if (!ModelCheck())
            {
                return;
            }

            List<MeshRenderer> baseRenders = new List<MeshRenderer>();
            List<MeshRenderer> targetRendes = new List<MeshRenderer>();

            baseRenders = new List<MeshRenderer>
                (baseModel.GetComponentsInChildren<MeshRenderer>());
            targetRendes = new List<MeshRenderer>
                (targetModel.GetComponentsInChildren<MeshRenderer>());

            Dictionary<string, MeshRenderer> baseRenderToName =
                new Dictionary<string, MeshRenderer>();

            foreach (var baseRender in baseRenders)
            {
                baseRenderToName.Add(baseRender.name, baseRender);
            }

            Queue<MeshRenderer> removeQueue = new Queue<MeshRenderer>();

            foreach (var target in targetRendes)
            {
                if (baseRenderToName.ContainsKey(target.name))
                {
                    var data = baseRenderToName[target.name];
                    target.shadowCastingMode = data.shadowCastingMode;
                    target.motionVectorGenerationMode = data.motionVectorGenerationMode;
                    target.sharedMaterials = data.sharedMaterials;
                    Debug.Log(target.name);
                }
                else
                {
                    Debug.Log(target.name);
                    removeQueue.Enqueue(target);
                }
            }

            if(removeQueue.Count > 0) 
                DeleteNotUseObjectInAsset(removeQueue);

            Debug.Log("데이터 전송이 완료되었습니다.");
        } 
        
        bool ModelCheck()
        {
            bool result = true;

            if (baseModel == null || targetModel == null)
            {
                result = false;
                ErrCatch("모델 필드가 비어있습니다. 모든 모델을 셋팅해주세요.");
            }
            else if (baseModel.scene.path != null || baseModel.scene.path == string.Empty)
            {
                result = false;
                ErrCatch("베이스 모델이 프로젝트 파일이 아닙니다. " +
                    "폴더에 있는 모델 파일을 넣어주세요,");
            }
            else if (targetModel.scene.path != null || targetModel.scene.path == string.Empty)
            {
                result = false;
                ErrCatch("타겟 모델이 프로젝트 파일이 아닙니다. " +
                    "폴더에 있는 모델 파일을 넣어주세요,");
            }

            return result;
        }

        void DeleteNotUseObjectInAsset(Queue<MeshRenderer> objects)
        {
            int count = objects.Count;
            string assetPath = AssetDatabase.GetAssetPath(targetModel);
            bool isDestroyParents = false;

            using (var editingScope = new PrefabUtility.EditPrefabContentsScope(assetPath))
            {
                var prefabRoot = editingScope.prefabContentsRoot;

                List<MeshRenderer> prefabMeshrenderer = new List<MeshRenderer>
                (prefabRoot.GetComponentsInChildren<MeshRenderer>());

                Dictionary<string, MeshRenderer> prefabRenderToName =
                new Dictionary<string, MeshRenderer>();

                foreach (var render in prefabMeshrenderer)
                {
                    prefabRenderToName.Add(render.name, render);
                }

                // 매쉬랜더러의 부모가 루트가 아니면, 부모 게임 오브젝트를 파괴
                isDestroyParents = 
                    prefabMeshrenderer[0].transform.parent.name != targetModel.name ?
                    true : false;

                for (int i = 0; i < count; i++)
                {
                    string key = objects.Dequeue().name;
                    if (prefabRenderToName.ContainsKey(key))
                    {
                        if(isDestroyParents) 
                            Object.DestroyImmediate
                                (prefabRenderToName[key].transform.parent.gameObject);
                        else Object.DestroyImmediate
                                (prefabRenderToName[key].gameObject);
                    }   
                }
            }
        }
    }
}

