﻿using System.Collections;
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

    int originalPrice;

    string[] sizes = new string[] { "5 x 7", "8 x 10", "11 x 14", "18 x 24" };

    public Art(int vendorId, int productID, string name, string description, string cause, int originalPrice)
    {
        this.vendorId = vendorId;
        this.productId = productID;
        this.variations = new List<Variation>();
        this.name = name;
        this.description = description;
        this.cause = cause;
        this.originalPrice = originalPrice;
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

    public List<Variation> CreateBaseVariations(List<int> variationIds)
    {
        //float start = Time.time;
        var variations = new List<Variation>();

        //var categories = new string[] { "Archival Gloss", "Archival Matte", "Regular Gloss", "Regular Matte" };
        
        var prices = new int[] { 5, 15, 30, 120 };

        int variationIndex = 0;// we always start with the orginial

        //foreach (string cat in categories)
        //{
        for(int index = 0; index < sizes.Length; index++)
        {
            variationIndex++;
            if (variationIndex < variationIds.Count)
                variations.Add(new Variation(variationIds[variationIndex], $"{sizes[index]}", prices[index]));                
        }
        //}

        if (originalPrice != 0)
            variations.Add(new Variation(variationIds[variationIndex], "Original", originalPrice));

        return variations;
    }

}
