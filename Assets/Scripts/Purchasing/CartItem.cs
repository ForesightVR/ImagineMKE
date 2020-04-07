using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using UnityEngine.UI;

public class CartItem : MonoBehaviour
{
    public TextMeshProUGUI artistName;
    public Image itemImage;
    public TMP_Dropdown variantDropDown;
    public TextMeshProUGUI itemCountText;
    public TextMeshProUGUI priceText;

    public int ItemCount { get; protected set; }
    public Art Art { get; protected set; }
    public Variation Variation { get; protected set; }

    Cart cart;

    public void SetArt(Cart cart, Art art)
    {
        this.cart = cart;
        Art = art;

        artistName.text = Art.artist.name;
        itemImage.sprite = art.artImage;

        Variation = art.variations[0];

        variantDropDown.options.Clear();

        for (int index = 0; index < art.variations.Count; index++)
        {
            variantDropDown.options.Add(new TMP_Dropdown.OptionData(art.variations[index].Name));
        }
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
        priceText.text = (Variation.Price * ItemCount).ToString("C");
    }

    public void OnDropdownChange()
    {
        Debug.Log(variantDropDown.value);

        Variation = Art.variations[variantDropDown.value];
        UpdateText();
    }


}
