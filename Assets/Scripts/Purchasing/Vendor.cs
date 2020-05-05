using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Foresight.Utilities;

public class Vendor
{
    public int vendorId;
    public string name;
    public string description;
    public List<Product> products;
    public Sprite image;
    int productIndex;

    public Vendor(int vendorId, string name, string description)
    {
        this.vendorId = vendorId;
        this.name = name;
        this.description = description;
        this.products = new List<Product>();
        this.image = StoredJSONs.instance.GetVendorSprite(vendorId);
    }

    public string GetCause()
    {
        if (products.Count > 0)
            return products[0].cause;
        else
            return "Empty";
    }

    public Product GetNextProduct()
    {
        Product product = products[productIndex];
        productIndex = MathUtilities.IncrementLoop(productIndex, products.Count - 1);
        return product;
    }

}
