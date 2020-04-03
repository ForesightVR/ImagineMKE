using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class ChangeArtEffect : Effect
{
    Image image;
    ArtistCanvas artistCanvas;

    void Awake()
    {
        methodArray = GetMethodNames((typeof(ChangeArtEffect))).ToArray();
        image = GetComponent<Image>();
    }

    public void SetArtistCanvas(ArtistCanvas canvas)
    {
        artistCanvas = canvas;
    }

    public void ChangeArt()
    {
        artistCanvas.artPiece.sprite = image.sprite;
    }
}
