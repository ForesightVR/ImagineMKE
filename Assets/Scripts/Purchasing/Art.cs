using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Art
{
    public int productID;
    public List<Variation> variations;
    public string name;
    public string description;
    public string cause;
    public Sprite artImage;
    public Artist artist;

    public Art (Artist artist, int productID, string name, string description, string cause)
    {
        this.artist = artist;
        this.productID = productID;
        this.variations = new List<Variation>();
        this.name = name;
        this.description = description;
        this.cause = cause;
    }
}
