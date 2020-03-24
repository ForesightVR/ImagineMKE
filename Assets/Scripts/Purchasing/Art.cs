using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Art
{
    public int productID;
    public List<int> variationIDs;
    public string name;
    public string description;
    public string cause;
    public Sprite artImage;

    public Art (int productID, List<int> variationIDs, string name, string description, string cause)
    {
        this.productID = productID;
        this.variationIDs = variationIDs;
        this.name = name;
        this.description = description;
        this.cause = cause;
    }
}
