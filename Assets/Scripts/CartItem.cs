using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CartItem : MonoBehaviour
{
    public TextMeshProUGUI artistName;
    public Image itemImage;
    public TextMeshProUGUI productID;
    public TextMeshProUGUI itemCountText;

    public int ItemCount { get; protected set; }
    public Art Art { get; protected set; }

    Cart cart;

    public void SetArt(Cart cart, Art art)
    {
        this.cart = cart;
        Art = art;

        artistName.text = Art.artist.name;
        itemImage.sprite = art.artImage;
        productID.text = "Product ID: " + art.productID;
    }

    public void AddCount()
    {
        ItemCount++;
        UpdateText();
    }

    public void RemoveCount()
    {
        if (ItemCount - 1 >= 1)
            ItemCount--;
        else
            ItemCount = 1;

        UpdateText();
    }

    public void RemoveItem()
    {
        cart.RemoveFromCart(this);
    }

    void UpdateText()
    {
        itemCountText.text = ItemCount.ToString();
    }
}
