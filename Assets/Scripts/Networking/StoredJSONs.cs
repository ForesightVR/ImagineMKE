using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using System.Text.RegularExpressions;

public class StoredJSONs : MonoBehaviour
{
    public TextAsset vendorJSON;
    public TextAsset[] productJSONs;

    public VendorDatabase database;

    public static StoredJSONs instance;

    void Awake()
    {
        instance = this;
    }

    public Sprite GetVendorSprite(int vendorID)
    {
        var vendorData = database.vendorDataCollection.FirstOrDefault(x => x.vendorID == vendorID);

        if (vendorData.vendorImage != null)
            return vendorData.vendorImage;
        else
            return null;
    }

    public Sprite GetProductSprite(int productID)
    {
        var productData = database.productDataCollection.FirstOrDefault(x => x.productID == productID);

        if (productData.productImage != null)
            return productData.productImage;
        else
            return null;
    }
}
