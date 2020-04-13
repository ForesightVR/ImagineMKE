using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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

    public static float vendorDownloadProgress;
    public static float artDownloadProgress;
    public static float imageDownloadProgress;
    public static int stateProgress;

    static bool retrying;

    public static IEnumerator CallGet()
    {
        Debug.Log(Time.realtimeSinceStartup);
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

        isDone = true;
        LoadManager.Instance.SetLoad(false);
        Debug.Log("FINISHED!");
        Debug.Log(Time.realtimeSinceStartup);
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

        if (artists.Count == 0) //if we have gotten artists, there's no need to call this again
        {
            var request = CreateGetRequest(vendorsApiString, new List<string> { "per_page=100" });
            request.SendWebRequest();

            while (!request.isDone)
            {
                float progress = request.downloadProgress * 100;
                vendorDownloadProgress = progress / 100;
                yield return null;
            }

            if (request.isNetworkError || request.isHttpError)
            {
                Retry();
                Debug.LogWarning($"Retrieved error {request.error}. Attempting to retry.");
            }
            else
            {
                Debug.Log($"Vendors: {request.downloadHandler.text}");
                JArray jsonArray = JArray.Parse(request.downloadHandler.text);
                CreateArtists(jsonArray);
            }

            stateProgress = 1;
        }

        
    }

    static IEnumerator GetAllProducts()
    {
        Debug.Log("GetAllProducts");
        for (int i = 1; i < 5; i++)
        {
            var request = CreateGetRequest(productsApiString, new List<string> { "per_page=100", "page=" + i });
            request.SendWebRequest();

            while (!request.isDone)
            {
                float progress = (request.downloadProgress * 100) + ((i - 1) * 100);
                artDownloadProgress = progress / 100 * i;
                yield return null;
            }

            if (request.isNetworkError || request.isHttpError)
            {
                //Retry();
                Debug.LogWarning($"Retrieved error {request.error}. Attempting to retry.");
            }
            else
            {
                Debug.Log($"Products: {request.downloadHandler.text}");
                JArray jsonArray = JArray.Parse(request.downloadHandler.text);
                CreateArt(jsonArray);
            }
        }

        stateProgress = 2;
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

            var vendorJObj = data["vendor"];            

            if (vendorJObj == null || data["tags"].Count() == 0)
                continue; // if no one can sell it, why have it in the museum?
            
            vendorId = (int)vendorJObj;
            tag = (string)data["tags"][0]["name"];

            var variationIdList = data["variations"]?.Select(x => (int)x).ToList() ?? new List<int>();

            Art art = new Art(vendorId, (int)data["id"], data["name"].ToString(), StripHTML(data["description"].ToString()), tag);

            CoroutineUtility.instance.StartCoroutine(GetArtImage(art, (string)data["images"][0]["src"]));

            art.variations = art.CreateBaseVariations(variationIdList);

            arts.Add(art);                      

            //GetAllVariationByProduct(variationIdList, art);
            //CoroutineUtility.instance.StartCoroutine(GetAllVariationsByProduct(art));
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

    static IEnumerator GetArtistImage(Artist artist, string imagePath)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(imagePath);
        www.SendWebRequest();

        while (!www.isDone)
        {
            float progress = www.downloadProgress * 100;
            vendorDownloadProgress = progress / 100;
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
            float progress = www.downloadProgress;
            imageDownloadProgress = (progress + piecesCompleted) / arts.Count;
            Debug.Log($"DownloadProcess: {imageDownloadProgress} at number {piecesCompleted}");
            yield return null;
        }

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
            yield return new WaitForSeconds(1f);
            CoroutineUtility.instance.StartCoroutine(GetArtImage(art, imagePath));
        }
        else
        {
            art.artImage = ((DownloadHandlerTexture)www.downloadHandler).texture;
            //art.artImage = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        }
        Debug.Log($"{art.name} finished at {Time.realtimeSinceStartup} for number {piecesCompleted}");
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
