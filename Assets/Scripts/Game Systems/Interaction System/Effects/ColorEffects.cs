using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class ColorEffects : Effect
{
    [DisplayIfStringMatch("chosenMethod", "ChangeColor")]
    public Color color;

    void Awake()
    {
        methodArray = GetMethodNames((typeof(ColorEffects))).ToArray();
    }

    public void ChangeColor(Color32 color)
    {
        gameObject.GetComponent<Image>().color = color;
    }

    public void ColorBlack()
    {
        gameObject.GetComponent<Image>().color = Color.black;
    }
}
