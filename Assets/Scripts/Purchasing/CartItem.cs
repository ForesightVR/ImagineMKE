using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using UnityEngine.UI;
using Michsky.UI.ModernUIPack;

public class CartItem : MonoBehaviour
{
    public TextMeshProUGUI artistName;
    public Image itemImage;
    public CustomDropdown variantDropDown;
    public TextMeshProUGUI itemCountText;
    public TextMeshProUGUI priceText;

    public int ItemCount { get; protected set; }
    public Product Art { get; protected set; }
    public Variation Variation { get; protected set; }

    Cart cart;

    public void SetArt(Cart cart, Product art)
    {
        this.cart = cart;
        Art = art;

        artistName.text = Art.owner.name;
        itemImage.sprite = art.image;

        Variation = art.variations[0];

        variantDropDown.dropdownItems.Clear();

        for (int index = 0; index < art.variations.Count; index++)
        {
            CustomDropdown.Item item = new CustomDropdown.Item();
            item.itemName = art.variations[index].Name;
            variantDropDown.dropdownItems.Add(item);
            variantDropDown.SetupDropdown();
        }

        UpdateText();
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

        if (Variation.Price != 0)
            priceText.text = (Variation.Price * ItemCount).ToString("C");
        else
            priceText.text = "Check Online For Details";
    }

    public void OnDropdownChange()
    {
        Variation = Art.variations[variantDropDown.selectedItemIndex];
        UpdateText();
    }
}
