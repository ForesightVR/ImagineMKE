using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class MuesumJSONs : MonoBehaviour
{
    [TextArea]
    public string vendorJSON;
    [TextArea]
    public string[] productJSONs;
    // Start is called before the first frame update
    public static MuesumJSONs instance;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
