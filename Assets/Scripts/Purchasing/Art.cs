using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Art
{
    public int productId;
    public List<Variation> variations;
    public string name;
    public string description;
    public string cause;
    public Sprite artImage;
    public Artist artist;
    public int vendorId;

    public Art(int vendorId, int productID, string name, string description, string cause)
    {
        this.vendorId = vendorId;
        this.productId = productID;
        this.variations = new List<Variation>();
        this.name = name;
        this.description = description;
        this.cause = cause;
    }

    public Art (Artist artist, int productID, string name, string description, string cause)
    {
        this.artist = artist;
        this.productId = productID;
        this.variations = new List<Variation>();
        this.name = name;
        this.description = description;
        this.cause = cause;
    }

    public Variation GetDefaultVariation()
    {
        return variations[0];
    }
}
