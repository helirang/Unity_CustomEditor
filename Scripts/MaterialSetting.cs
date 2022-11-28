using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialSetting : MonoBehaviour
{
    [SerializeField]
    private GameObject alembicModel;
    [SerializeField]
    private Material[] materials;
    [SerializeField]
    private MeshRenderer[] meshRenderers;

    private void ErrCatch(string message)
    {
        Debug.LogWarning(message);
    }

    public void SettinMeshRenderer()
    {
        alembicModel = this.gameObject;
        meshRenderers = alembicModel.GetComponentsInChildren<MeshRenderer>();
        if (meshRenderers == null)
        {
            ErrCatch("alembicModel에서 meshRenderer를 찾을 수 없습니다.");
            return;
        }
    }
    public void SettingMaterial()
    {
        if (meshRenderers == null) { 
            ErrCatch("meshRenderer가 셋팅되지 않았습니다.");
            return;
        }
        if(meshRenderers.Length != materials.Length)
        {
            ErrCatch("meshRenderers와 materials의 개수가 다릅니다. 동일하게 맞춰주세요.");
            return;
        }

        int i = 0;
        foreach(var meshRender in meshRenderers)
        {
            meshRender.material = materials[i];
            i++;
        }
    }
}
