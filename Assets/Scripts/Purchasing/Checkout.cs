using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Checkout : MonoBehaviour
{
    public void PerformCheckout()
    {
        StartCoroutine(CreatePurchase());
    }

    IEnumerator CreatePurchase()
    {
        foreach (CartItem item in Cart.Instance.artInCart)
        {
            string link = $"{MuseumBank.websiteRoot}/cart/?add-to-cart={item.Variation.Id}&quantity={item.ItemCount}";
            Debug.Log($"Link:{link}");
            yield return StatePurchase(link);
        }

        Application.OpenURL($"{MuseumBank.websiteRoot}/checkout");
    }

    IEnumerator StatePurchase(string link)
    {
        UnityWebRequest request = new UnityWebRequest(link);

        yield return request.SendWebRequest();

        if (request.isNetworkError)
            throw new System.Exception(request.error);
    }
}
