using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artist
{
    public int vendorId;
    public string name;
    public string description;
    public List<Art> artPieces;
    public Sprite artistImage;

    public Artist(int vendorId, string name, string description)
    {
        this.vendorId = vendorId;
        this.name = name;
        this.description = description;
        this.artPieces = new List<Art>();
    }

}
