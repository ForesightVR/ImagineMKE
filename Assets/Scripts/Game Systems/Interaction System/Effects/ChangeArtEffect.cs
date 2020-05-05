using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class ChangeArtEffect : Effect
{
    Image image;
    ArtistCanvas artistCanvas;
    Product art;

    void Awake()
    {
        methodArray = GetMethodNames((typeof(ChangeArtEffect))).ToArray();
        image = GetComponent<Image>();
    }

    public void SetInfo(ArtistCanvas canvas, Product art)
    {
        artistCanvas = canvas;
        this.art = art;
    }

    public void ChangeArt()
    {
        artistCanvas.SetArt(art);
    }
}
