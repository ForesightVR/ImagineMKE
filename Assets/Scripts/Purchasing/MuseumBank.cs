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
    static string productsApiString = "/wp-json/wc/v2/products";

    public static string websiteRoot = "https://outsidersvr.com";

    public static List<Artist> artists = new List<Artist>();
    static List<Art> arts = new List<Art>();

    static int piecesCompleted = 0;

    public static int currentArtPieces;
    public static int maxArtPieces;
    public static bool isDone;

    public static float downloadProgress;

    static bool retrying;
    static bool isTesting = true;

    public static IEnumerator CallGet()
    {
        if (!isTesting)
        {
            Debug.Log(Time.time);
            yield return GetAllVendors();
            yield return GetAllProducts();

            foreach (Artist artist in artists)
            {
                var pieces = arts.Where(x => x.vendorId == artist.vendorId).ToList();

                foreach (Art art in pieces)
                {
                    artist.artPieces.Add(art);
                    art.artist = artist;
                    //CoroutineUtility.instance.StartCoroutine(VariationRequest(art));
                }
            }

            maxArtPieces = arts.Count;
            yield return new WaitUntil(GotAllArtImages);
        }

        isDone = true;
        LoadManager.Instance.SetLoad(false);
        Debug.Log("FINISHED!");
        Debug.Log(Time.time);
    }

    static bool GotAllArtImages()
    {
        currentArtPieces = piecesCompleted;
        Debug.Log($"We're at {piecesCompleted} out of {arts.Count}");
        return (piecesCompleted == arts.Count);
    }

    static IEnumerator GetAllVendors()
    {
        Debug.Log("GetAllVendors");

        var request = CreateGetRequest(vendorsApiString, new List<string> {"per_page=100"});
        request.SendWebRequest();

        while (!request.isDone)
        {
            float progress = request.downloadProgress * 100;
            downloadProgress = progress / 100;
            yield return null;
        }

        if (request.isNetworkError || request.isHttpError)
        {
            Retry();
            throw new System.Exception(request.error);
        }
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
        for (int i = 1; i < 5; i++)
        {
            var request = CreateGetRequest(productsApiString, new List<string> { "per_page=100", "page=" + i});
            request.SendWebRequest();

            while (!request.isDone)
            {
                float progress = request.downloadProgress * 100;
                downloadProgress = progress / 100;
                yield return null;
            }

            if (request.isNetworkError || request.isHttpError)
            {
                Retry();
                throw new System.Exception(request.error);
            }
            else
            {
                Debug.Log($"Products: {request.downloadHandler.text}");
                JArray jsonArray = JArray.Parse(request.downloadHandler.text);
                CreateArt(jsonArray);
            }
        }
    }

    static void Retry()
    {
        if(!retrying)
        {
            retrying = true;
            Debug.Log("Retrying..");
            CoroutineUtility.instance.StartCoroutine(WaitForRetry());
        }
    }

    static IEnumerator WaitForRetry()
    {
        yield return new WaitForSeconds(3);
        retrying = false;
        CallGet();
    }

    static void CreateArtists(JArray jArray)
    {
        foreach (JObject jObject in jArray)
        {
            JObject data = JObject.Parse(jObject.ToString());
            Artist artist = new Artist((int)data["id"], data["display_name"].ToString(), StripHTML((string)data["shop"]["description"]));

            //Debug.Log($"Image for {data["display_name"].ToString()} possibly {data["shop"]["image"]}");

            string artistImageString = GetImageLink((string)data["shop"]["description"]);

            if (artistImageString.Length > 0)
            {
                CoroutineUtility.instance.StartCoroutine(GetArtistImage(artist, artistImageString));
            }

            Debug.Log($"Adding {artist.name}");
            artists.Add(artist);
        }
    }
   
    static void CreateArt(JArray jArray)
    {
        foreach (JObject jObject in jArray)
        {
            //Debug.Log("Creating art!");
            JObject data = JObject.Parse(jObject.ToString());

            string tag = "";
            int vendorId = 0;
            int price = 0;

            var vendorJObj = data["vendor"];            

            if (vendorJObj == null || data["tags"].Count() == 0)
                continue; // if no one can sell it, why have it in the museum?
            
            vendorId = (int)vendorJObj;

            var priceObj = data["regular_price"];

            if (priceObj != null)
                price = (int)priceObj;

            tag = (string)data["tags"][0]["name"];

            var variationIdList = data["variations"]?.Select(x => (int)x).ToList() ?? new List<int>();

            Art art = new Art(vendorId, (int)data["id"], data["name"].ToString(), StripHTML(data["description"].ToString()), tag, price);

            CoroutineUtility.instance.StartCoroutine(GetArtImage(art, (string)data["images"][0]["src"]));

            art.variations = art.CreateBaseVariations(variationIdList);

            arts.Add(art);                      

            //GetAllVariationByProduct(variationIdList, art);
            //CoroutineUtility.instance.StartCoroutine(GetAllVariationsByProduct(art));
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
            CoroutineUtility.instance.StartCoroutine(VariationRequest(product));           
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

    static IEnumerator GetArtistImage(Artist artist, string imagePath)
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
        }
        else
        {
            Texture2D texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            artist.artistImage = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        }
    }

    static IEnumerator GetArtImage(Art art, string imagePath)
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
        }
        else
        {
            Texture2D texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            art.artImage = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        }
        piecesCompleted++;
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
