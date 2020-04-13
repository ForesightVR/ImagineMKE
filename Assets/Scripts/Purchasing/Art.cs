using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class Art
{
    public int productId;
    public List<Variation> variations;
    public string name;
    public string description;
    public string cause;
    public Texture2D artImage;
    public Sprite artSprite {
        get
        {
            if (artSprite == null)
            {
                artSprite = Sprite.Create(artImage, new Rect(0, 0, artImage.width, artImage.height), Vector2.zero);
                return artSprite;
            }
            else
                return artSprite;
        } set { }
    }
    public Artist artist;
    public int vendorId;

    public static int artPiecesLoaded;

    public Art(int vendorId, int productID, string name, string description, string cause)
    {
        this.vendorId = vendorId;
        this.productId = productID;
        this.variations = new List<Variation>();
        this.name = name;
        this.description = description;
        this.cause = cause;
        //GetRemoteTexture(imageURL);
    }


    public Variation GetDefaultVariation()
    {
        return variations[0];
    }

    public List<Variation> CreateBaseVariations(List<int> variationIds)
    {
        float start = Time.time;
        var variations = new List<Variation>();

        var categories = new string[] { "Archival Gloss", "Archival Matte", "Regular Gloss", "Regular Matte" };
        var sizes = new string[] { "5 x 7", "8 x 10", "11 x 14", "18 x 24" };
        var prices = new int[] { 20, 80, 120, 300 };

        int variationIndex = 0;// we always start with the orginial

        variations.Add(new Variation(variationIds[variationIndex], "Original", 0));

        foreach (string cat in categories)
        {
            for(int index = 0; index < sizes.Length; index++)
            {
                variationIndex++;
                if (variationIndex < variationIds.Count)
                    variations.Add(new Variation(variationIds[variationIndex], $"{cat} {sizes[index]}", cat.Contains("Archival") ? prices[index] * 2 : prices[index]));                
            }
        }

        return variations;
    }

    async Task<Texture2D> GetRemoteTexture(string url)
    {
        //using UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(url))
        {
            //begin requenst:
            var asyncOp = www.SendWebRequest();

            //await until it's done: 
            while (asyncOp.isDone == false)
            {
                await Task.Delay(1000 / 30);//30 hertz
            }

            //read results:
            if (www.isNetworkError || www.isHttpError)
            {
                //log error:
                Debug.Log($"{ www.error }, URL:{ www.url }");
                //nothing to return on error:
                return null;
            }
            else
            {
                //return valid results:
                artPiecesLoaded++;
                artImage = DownloadHandlerTexture.GetContent(www);
                return null;
                //return DownloadHandlerTexture.GetContent(www);
            }
        }
    }
}
