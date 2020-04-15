using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using System.Text.RegularExpressions;

public class MuseumJSONs : MonoBehaviour
{
    [TextArea]
    public string vendorJSON;
    [TextArea]
    public string[] productJSONs;

    public ArtistDatabase database;

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

    public Sprite GetArtistSprite(int vendorID)
    {
        var artistData = database.artistData.FirstOrDefault(x => x.vendorID == vendorID);

        if (artistData.artistImage != null)
            return artistData.artistImage;
        else
            return null;
    }

    public Sprite GetArtSprite(int productID)
    {
        var artData = database.artData.FirstOrDefault(x => x.productID == productID);

        if (artData.artImage != null)
            return artData.artImage;
        else
            return null;
    }
}
