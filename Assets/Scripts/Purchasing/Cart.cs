using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Cart : MonoBehaviour
{
    public static Cart Instance;
    public GameObject cart;
    public CartItem cartItem;
    public Transform cartItemList;
    public List<CartItem> artInCart;
    public GameObject addToCartMessage;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.C))
            cart.SetActive(!cart.activeSelf);
    }

    public void AddToCart(Art art)
    {
        addToCartMessage.SetActive(true);
        foreach (CartItem _cartItem in artInCart) //Check if the item is in the cart. If it is add another one and return.
        {
            if (_cartItem.Art == art && _cartItem.Variation == art.GetDefaultVariation())
            {
                _cartItem.AddCount();
                return;
            }
        }

        //If art is not in the cart, make a new CartItem for that art piece

        CartItem newItem = Instantiate(cartItem, cartItemList);
        newItem.SetArt(this, art);
        newItem.AddCount();

        artInCart.Add(newItem);
    }

    public void RemoveFromCart(CartItem _cartItem)  //If the cart has this item remove and destroy it.
    {
        if (artInCart.Contains(_cartItem))
            artInCart.Remove(_cartItem);

        Destroy(_cartItem.gameObject);
    }
}
