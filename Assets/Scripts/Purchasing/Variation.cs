using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Variation
{
    readonly int id;
    public float Price
    {
        get; protected set;
    }
    public string Name { get; protected set; }

    public Variation(int id, string name, float price)
    {
        this.id = id;
        this.Name = name;
        this.Price = price;
    }
}
