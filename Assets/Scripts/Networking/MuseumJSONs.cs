using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Text.RegularExpressions;

public class MuseumJSONs : MonoBehaviour
{
    [TextArea]
    public string vendorJSON;
    [TextArea]
    public string[] productJSONs;
    // Start is called before the first frame update
    public static MuseumJSONs instance;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static string StripHTML(string input)
    {
        return Regex.Replace(input, "<.*?>", String.Empty);
    }
}
