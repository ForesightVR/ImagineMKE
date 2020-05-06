using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;
using Foresight.Utilities;
using Valve.Newtonsoft.Json.Linq;

public static class PullStorage
{  
    public static List<Vendor> vendors = new List<Vendor>();
    public static List<Product> products = new List<Product>();

    static int piecesCompleted = 0;

    public static int currentProductPieces;
    public static int maxProductPieces;
    public static bool isDone;

    public static float downloadProgress;

    static bool isTesting = false;

    public static void FormatPullInfo()
    {
        if (!isTesting && !isDone)
        {
            ConvertAllVendors();
            for (int i = 0; i < DataManager.instance.GetNumProductPages(); i++)
            {
                ConvertAllProducts(i);               
            }

            foreach (Vendor vendor in vendors)
            {
                var pieces = products.Where(x => x.vendorId == vendor.vendorId).ToList();

                foreach (Product product in pieces)
                {
                    vendor.products.Add(product);
                    product.owner = vendor;
                }
            }

            //maxProductPieces = products.Count;
        }

        //TODO: Add some sort of system that tracks if the system has converted all the pull info into Vendors and Products
        //LoadManager.Instance.SetLoad(false);
        isDone = true;        
    }   

    static void ConvertAllVendors()
    {
        JArray jsonArray = JArray.Parse(DataManager.instance.GetVendorData());
        CreateVendors(jsonArray);       
    }
    static void ConvertAllProducts(int pageIndex)
    {
        JArray jsonArray = JArray.Parse(DataManager.instance.GetProductPageData(pageIndex));
        CreateProduct(jsonArray);      
    }
    static void CreateVendors(JArray jArray)
    {
        foreach (JObject jObject in jArray)
        {
            JObject data = JObject.Parse(jObject.ToString());
            string description = StringUtilities.StripHTML((string)data["shop"]["description"]);

            Vendor vendor = new Vendor((int)data["id"], data["display_name"].ToString(), description);
            vendors.Add(vendor);
        }
    }
    static void CreateProduct(JArray jArray)
    {
        foreach (JObject jObject in jArray)
        {
            JObject data = JObject.Parse(jObject.ToString());

            string tag = "";
            int vendorId = 0;
            int price = 0;

            var vendorJObj = data["vendor"];

            if (vendorJObj == null || data["tags"].Count() == 0)
                continue; // if no one can sell it, why have it in the location?

            vendorId = (int)vendorJObj;

            //TODO: Change this code to match wherever the price is stored
            var priceObj = (string)data["short_description"];
            string priceString = new string(priceObj.Where(char.IsDigit).ToArray());
            if (priceString.Count() > 0)
                Int32.TryParse(priceString, out price);

            //TODO: Figure out better system for storing what type of item it is
            tag = (string)data["tags"][0]["name"];

            var variationIdList = data["variations"]?.Select(x => (int)x).ToList() ?? new List<int>();
            string description = StringUtilities.StripHTML(data["description"].ToString());

            Product product = new Product(vendorId, (int)data["id"], data["name"].ToString(), description, tag, price); ;

            product.variations = product.CreateBaseVariations(variationIdList);

            products.Add(product);
        }
    }

    #region Pulling Files (Currently Unused)
    static IEnumerator GetAllVendors()
    {
        Debug.Log("GetAllVendors");

        var request = WebsiteInfo.CreateGetRequest(WebsiteInfo.vendorsApiString, new List<string> {"per_page=100"});
        request.SendWebRequest();

        while (!request.isDone)
        {
            float progress = request.downloadProgress * 100;
            downloadProgress = progress / 100;
            yield return null;
        }

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log(request.error);
            Debug.Log("Retrying..");
            yield return new WaitForSeconds(3);
            DataManager.instance.StartCoroutine(GetAllVendors());
        }
        else
        {
            string toCopy = request.downloadHandler.text;
            Debug.Log($"Vendors: {toCopy}");
            //JArray jsonArray = JArray.Parse(request.downloadHandler.text);
            //CreateArtists(jsonArray);
            //yield return new WaitForSeconds(3);
        }
    }
    static IEnumerator GetAllProducts(int pageIndex)
    {        
        var request = WebsiteInfo.CreateGetRequest(WebsiteInfo.productsApiString, new List<string> { "per_page=100", "page=" + pageIndex });
        request.SendWebRequest();

        while (!request.isDone)
        {
            float progress = request.downloadProgress * 100;
            downloadProgress = progress / 100;
            yield return null;
        }

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log(request.error);
            Debug.Log($"Retrying Products {pageIndex}");
            yield return new WaitForSeconds(3);
            DataManager.instance.StartCoroutine(GetAllProducts(pageIndex));
        }
        else
        {
            string toCopy = request.downloadHandler.text;
            Debug.Log($"Products: {toCopy}");
            //Debug.Log($"Products: {request.downloadHandler.text}");
           // JArray jsonArray = JArray.Parse(request.downloadHandler.text);
            //CreateArt(jsonArray);
        }
    }
    static IEnumerator GetVendorImage(Vendor vendor, string imagePath)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(imagePath);
        www.SendWebRequest();

        while (!www.isDone)
        {
            float progress = www.downloadProgress * 100;
            downloadProgress = progress / 100;
            yield return null;
        }

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
            Debug.Log($"Retrying vendor {vendor.name}");
            yield return new WaitForSeconds(3);
            DataManager.instance.StartCoroutine(GetVendorImage(vendor, imagePath));
        }
        else
        {
            Texture2D texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            vendor.image = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        }
    }
    static IEnumerator GetProductImage(Product product, string imagePath)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(imagePath);
        www.SendWebRequest();

        while (!www.isDone)
        {
            float progress = www.downloadProgress * 100;
            downloadProgress = progress / 100;
            yield return null;
        }

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log($"Retrying product {product.name} due to {www.error}");
            yield return new WaitForSeconds(3);
            DataManager.instance.StartCoroutine(GetProductImage(product, imagePath));
        }
        else
        {
            Texture2D texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            product.image = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
            piecesCompleted++;
        }
    }

    /*static IEnumerator VariationRequest(Art product)
    {
        UnityWebRequest request = CreateGetRequest($"{productsApiString}/{product.productId}/variations");
        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.LogWarning($"Failed to connect for variations of {product.name} with {request.error}");
            yield return new WaitForSecondsRealtime(.5f);
            DataManager.instance.StartCoroutine(VariationRequest(product));           
        }            
        else
        {
            Debug.Log($"Succesfully connected for {product.name}");
            var jArray = JArray.Parse(request.downloadHandler.text);
            CreateVariations(jArray, product);
            piecesCompleted++;
        }
    }

    static void CreateVariations(JArray jArray, Art product)
    {
        Debug.Log($"Creating Variations for {product.productId}");

        foreach (JObject jObject in jArray)
        {
            JObject data = JObject.Parse(jObject.ToString());

            if (product.productId == 0)
            {
                Debug.LogError("There was no matching product for " + data["name"].ToString());
            }

            if ((bool)data["purchasable"])
            {
                int varId = (int)data["id"];
                float price = (float)data["price"];

                var attributes = data["attributes"]; //need to loop through and get them all

                JArray jsonArray = JArray.Parse(attributes.ToString());

                string attName = "";

                foreach (JObject jobj in jsonArray)
                {
                    attName += $"{jobj["option"]} ";
                }

                Debug.Log($"{attName} for {product.name}");

                var variation = new Variation(varId, attName, price);

                product.variations.Add(variation);
            }
        }        
    }*/
    #endregion

    /*static bool GotAllProductImages()
    {
        currentProductPieces = piecesCompleted;        
        return (piecesCompleted >= products.Count);//arts.Count
    }*/
}
