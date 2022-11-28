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
            ErrCatch("alembicModel���� meshRenderer�� ã�� �� �����ϴ�.");
            return;
        }
    }
    public void SettingMaterial()
    {
        if (meshRenderers == null) { 
            ErrCatch("meshRenderer�� ���õ��� �ʾҽ��ϴ�.");
            return;
        }
        if(meshRenderers.Length != materials.Length)
        {
            ErrCatch("meshRenderers�� materials�� ������ �ٸ��ϴ�. �����ϰ� �����ּ���.");
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
