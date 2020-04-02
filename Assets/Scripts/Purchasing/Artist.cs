using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Foresight.Utilities;


public class Artist
{
    public int vendorId;
    public string name;
    public string description;
    public List<Art> artPieces;
    public Sprite artistImage;
    int artIndex;

    public Artist(int vendorId, string name, string description)
    {
        this.vendorId = vendorId;
        this.name = name;
        this.description = description;
        this.artPieces = new List<Art>();
    }

    public string GetCause()
    {
        if (artPieces.Count > 0)
            return artPieces[0].cause;
        else
            return "Empty";
    }

    public Art GetNextArt()
    {
        Art art = artPieces[artIndex];
        artIndex = MathUtilities.IncrementLoop(artIndex, artPieces.Count - 1);
        return art;
    }

}
