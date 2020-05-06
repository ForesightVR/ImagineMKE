using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public VendorDatabase database;
    public StoredJSONs storedJSONs;

    public static DataManager instance;

    void Awake()
    {
        instance = this;
        PullStorage.FormatPullInfo();
    }

    public string GetVendorData()
    {
        return storedJSONs.vendorJSON.text;
    }
    public int GetNumProductPages()
    {
        return storedJSONs.productJSONs.Length;
    }
    public string GetProductPageData(int pageIndex)
    {
        if (pageIndex >= storedJSONs.productJSONs.Length)
            return "";

        return storedJSONs.productJSONs[pageIndex].text;
    }
    public List<string> GetAllProductData()
    {
        return storedJSONs.productJSONs.Select(x => x.text).ToList();
    }
    public Sprite GetVendorSprite(int vendorID)
    {
        var vendorData = database.vendorDataCollection.FirstOrDefault(x => x.vendorID == vendorID);

        if (vendorData.vendorImage != null)
            return vendorData.vendorImage;
        else
            return null;
    }
    public Sprite GetProductSprite(int productID)
    {
        var productData = database.productDataCollection.FirstOrDefault(x => x.productID == productID);

        if (productData.productImage != null)
            return productData.productImage;
        else
            return null;
    }
    public string GetImageLink(string data)
    {
        string searchCharacters = "src=\"";

        var searchCharIndex = data.IndexOf(searchCharacters);

        string imageLink;

        if (searchCharIndex > 0)
        {
            imageLink = ParseHTMLAttribute(searchCharIndex + searchCharacters.Length, data);
        }
        else
        {
            imageLink = "";  //we could have a link to a default image if their description doesn't have an iamge
            Debug.Log("No Image Found!");
        }

        return imageLink;
    }
    static string ParseHTMLAttribute(int startingIndex, string htmlString)
    {
        var endingIndex = htmlString.IndexOf('"', startingIndex);

        return htmlString.Substring(startingIndex, endingIndex - startingIndex);
    }

}
