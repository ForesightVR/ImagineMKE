using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;
using Valve.Newtonsoft.Json.Linq;

public static class MuseumBank
{
    static string oauth_consumerKey = "ck_42bc844ad5f5679fa919f8104137bd276daefbe2";
    static string oauth_consumerSecret = "cs_e65a26fd8ece512d02363359388e863f2eedc598";

    static string vendorsApiString = "/wp-json/wcmp/v1/vendors";
    static string productsApiString = "/wp-json/wc/v3/products";

    public static string websiteRoot = "https://outsidersvr.com";

    public static List<Artist> artists = new List<Artist>();
    static List<Art> arts = new List<Art>();

    public static IEnumerator CallGet()
    {
        yield return GetAllVendors();
        yield return GetAllProducts();

        foreach (Art art in arts)
        {
            art.artist = artists.FirstOrDefault(x => x.vendorId == art.vendorId);
            CoroutineUtility.instance.StartCoroutine(VariationRequest(art));
        }
    }
       
    static IEnumerator GetAllVendors()
    {
        Debug.Log("GetAllVendors");

        var request = CreateGetRequest(vendorsApiString);

        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
            throw new System.Exception(request.error);
        else
        {
            Debug.Log($"Vendors: {request.downloadHandler.text}");
            JArray jsonArray = JArray.Parse(request.downloadHandler.text);
            CreateArtists(jsonArray);
        }
    }

    static IEnumerator GetAllProducts()
    {
        Debug.Log("GetAllProducts");

        var request = CreateGetRequest("/wp-json/wc/v2/products", new List<string> {"vendor=20"});

        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
            throw new System.Exception(request.error);
        else
        {
            Debug.Log($"Products: {request.downloadHandler.text}");
            JArray jsonArray = JArray.Parse(request.downloadHandler.text);
            CreateArt(jsonArray);   
        }
    }

    static void CreateArtists(JArray jArray)
    {
        foreach (JObject jObject in jArray)
        {
            JObject data = JObject.Parse(jObject.ToString());
            Artist artist = new Artist((int)data["id"], data["display_name"].ToString(), StripHTML((string)data["shop"]["description"]));

            string artistImageString = GetImageLink((string)data["shop"]["description"]);

            if (artistImageString.Length > 0)
            {
                CoroutineUtility.instance.StartCoroutine(GetImage(artist, artistImageString));
            }

            Debug.Log($"Adding {artist.name}");
            artists.Add(artist);

            //if (artist.artPieces.Count > 0)
            //GetAllProductsByVendor(artist);
            //yield return new WaitForSecondsRealtime(1f);
        }

        //CoroutineUtility.instance.StartCoroutine(GetAllProductsByVendor());
        //GetAllProductsByVendor();
    }
    static void CreateArt(JArray jArray)
    {
        foreach (JObject jObject in jArray)
        {
            JObject data = JObject.Parse(jObject.ToString());

            string tag = (string)data["tags"][0]["name"];
            Debug.Log("Cause: " + tag);

            var vendorId = data["vendor"];

            Debug.Log(vendorId);

            Art art = new Art(0, (int)data["id"], data["name"].ToString(), StripHTML(data["description"].ToString()), tag);

            CoroutineUtility.instance.StartCoroutine(GetImage(art, (string)data["images"][0]["src"]));

            arts.Add(art);

            //var variationIdList = data["variations"]?.Select(x => (int)x).ToList() ?? new List<int>();

            //GetAllVariationByProduct(variationIdList, art);
            //CoroutineUtility.instance.StartCoroutine(GetAllVariationsByProduct(art));
        }
    }

    /*static void GetAllProductsByVendor(Artist artist)
    {       
        CoroutineUtility.instance.StartCoroutine(ArtRequest(artist));       
    }*/

    /*static IEnumerator ArtRequest(Artist artist)
    {
        var request = CreateGetRequest(productsApiString);
        Debug.Log($"Request URL for {artist.name} is {request.url}");
        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {            
            Debug.LogWarning($"Failed to connect for {artist.name} with {request.error}");
            yield return new WaitForSecondsRealtime(2f);
            //CoroutineUtility.instance.StartCoroutine(ArtRequest(artist));
        }
            
        else
        {
            Debug.Log($"Succesfully connected for {artist.name}");
            Debug.Log($"{artist.name}'s products: {request.downloadHandler.text}");
            JArray jsonArray = JArray.Parse(request.downloadHandler.text);
            
            if (jsonArray.Count > 0)
                CreateArt(jsonArray, artist);
        }
    }*/    

    /*static IEnumerator GetAllVariationsByProduct(Art product)
    {
        CoroutineUtility.instance.StartCoroutine(VariationRequest(product));
    }*/

    static IEnumerator VariationRequest(Art product)
    {
        UnityWebRequest request = CreateGetRequest($"{productsApiString}/{product.productId}/variations");
        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.LogWarning($"Failed to connect for {product.name} with {request.error}");            
            //CoroutineUtility.instance.StartCoroutine(VariationRequest(product));
        }
            
        else
        {
            Debug.Log($"Succesfully connected for {product.name}");
            var jArray = JArray.Parse(request.downloadHandler.text);
            CreateVariations(jArray, product);
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
    }

    static UnityWebRequest CreateGetRequest(string apiString)
    {
        string url = GenerateRequestURL(websiteRoot + apiString);
        var request = UnityWebRequest.Get(url);
        return request;
    }

    static UnityWebRequest CreateGetRequest(string apiString, List<string> parameters)
    {
        string url = GenerateRequestURL(websiteRoot + apiString, parameters);
        var request = UnityWebRequest.Get(url);
        return request;
    }

    static string GenerateRequestURL(string in_url, string HTTP_Method = "GET")
    {
        OAuth_CSharp oauth = new OAuth_CSharp(oauth_consumerKey, oauth_consumerSecret);
        string requestURL = oauth.GenerateRequestURL(in_url, HTTP_Method);

        return requestURL;
    }

    static string GenerateRequestURL(string in_url, List<string> parameters, string HTTP_Method = "GET")
    {
        OAuth_CSharp oauth = new OAuth_CSharp(oauth_consumerKey, oauth_consumerSecret);
        string requestURL = oauth.GenerateRequestURL(in_url, HTTP_Method, parameters);

        return requestURL;
    }

    static IEnumerator GetImage(Artist artist, string imagePath)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(imagePath);

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Texture2D texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            artist.artistImage = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        }
    }

    static IEnumerator GetImage(Art art, string imagePath)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(imagePath);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Texture2D texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            art.artImage = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        }
    }

    static string GetImageLink(string data)
    {
        string searchCharacters = "src=\"";

        var searchCharIndex = data.IndexOf(searchCharacters);

        string imageLink = "";

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
    static string StripHTML(string input)
    {
        return Regex.Replace(input, "<.*?>", String.Empty);
    }
}
