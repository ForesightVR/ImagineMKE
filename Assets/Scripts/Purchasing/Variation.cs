using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Variation
{
    public int Id { get; protected set; }
    public float Price { get; protected set; }
    public string Name { get; protected set; }

    public Variation(int id, string name, float price)
    {
        this.Id = id;
        this.Name = name;
        this.Price = price;
    }
}
