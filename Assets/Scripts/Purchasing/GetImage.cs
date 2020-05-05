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
            if (PullStorage.vendors.Count <= 0 || PullStorage.vendors[0].products.Count <= 0)
                return;

            Sprite newSprite = PullStorage.vendors[0].products[0].image;

            if (newSprite)
                image.sprite = newSprite;
        }
    }
}
