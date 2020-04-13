using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetImage : MonoBehaviour
{
    Image image;
    Sprite sprite;

    private void Start()
    {
        image = GetComponent<Image>();
        sprite = image.sprite;
    }

    private void Update()
    {
        if(image.sprite == sprite)
        {
            if (MuseumBank.artists.Count <= 0 || MuseumBank.artists[0].artPieces.Count <= 0)
                return;

            Sprite newSprite = MuseumBank.artists[0].artPieces[0].artSprite;

            if (newSprite)
                image.sprite = newSprite;
        }
    }
}
