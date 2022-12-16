using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetRandomMaterial : MonoBehaviour
{
    [SerializeField]
    private List<Material> materialList;

    private SkinnedMeshRenderer skinnedRenderer;

    private MeshRenderer meshRenderer;
    // Start is called before the first frame update

    private int index;

    void Awake()
    {
        skinnedRenderer = GetComponent<SkinnedMeshRenderer>();

        meshRenderer = GetComponent<MeshRenderer>();
    }

    void Start()
    {
        index = Random.Range(0, materialList.Count);

        while (index == ResourceProvider.i.informationManager.beforeMaterialIndex)
        {
            index = Random.Range(0, materialList.Count);
        }

        if(skinnedRenderer != null) skinnedRenderer.material = materialList[index];
        else meshRenderer.material = materialList[index];


        ResourceProvider.i.informationManager.beforeMaterialIndex = index;
    }
}
