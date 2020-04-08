using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetParent : MonoBehaviour
{
    [InspectorButton("SetUp")]
    public bool setUp;

    public string childName = "CC_Game_Eye";
    public string parentName = "CC_Base_Head";

    void SetUp()
    {
        GameObject child = null;
        GameObject parent = null;

        List<Transform> children = new List<Transform>();

        GetAllChildren(transform, ref children);

        foreach(Transform t in children)
        {
            if (t.name == childName)
                child = t.gameObject;
            else if (t.name == parentName)
                parent = t.gameObject;
        }

        if (child && parent)
        {
            child.transform.parent = parent.transform;
            DestroyImmediate(this);
        }
        else
        {
            Debug.Log("Could not find parent or child!");
        }
    }

    public static void GetAllChildren(Transform parent, ref List<Transform> transforms)
    {

        foreach (Transform t in parent)
        {

            transforms.Add(t);

            GetAllChildren(t, ref transforms);

        }

    }
}
