using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SetUpEyes : MonoBehaviour
{
    [InspectorButton("SetUp")]
    public bool setUp;
    SkinnedMeshRenderer skin;
    MeshRenderer meshRenderer;
    MeshFilter meshFilter;

    private void SetUp()
    {
        if (!skin)
            skin = GetComponent<SkinnedMeshRenderer>();

        if (!meshRenderer)
        {
            meshRenderer = GetComponent<MeshRenderer>();

            if (!meshRenderer)
                meshRenderer = gameObject.AddComponent<MeshRenderer>();
        }

        if (!meshFilter)
        {
            meshFilter = GetComponent<MeshFilter>();

            if (!meshFilter)
                meshFilter = gameObject.AddComponent<MeshFilter>();
        }

        if (skin && meshRenderer && meshFilter)
        {
            meshFilter.mesh = skin.sharedMesh;
            meshRenderer.material = skin.materials[0];

            DestroyImmediate(skin);
            DestroyImmediate(this);
        }
    }
}
