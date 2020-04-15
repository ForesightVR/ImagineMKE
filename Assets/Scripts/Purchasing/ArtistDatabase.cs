using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Valve.Newtonsoft.Json.Linq;

[CreateAssetMenu(fileName = "ArtistDatabase")]
public class ArtistDatabase : ScriptableObject
{
    [InspectorButton("CreateArtistDatabase")]
    public bool areArtistsCreated;

    public List<Sprite> images;

    [System.Serializable]
    public struct ArtData
    {
        public Sprite artImage;
        public int productID;

        public ArtData(Sprite image, int productID)
        {
            artImage = image;
            this.productID = productID;
        }
    }

    [System.Serializable]
    public struct ArtistData
    {
        public Sprite artistImage;
        public int vendorID;

        public ArtistData(Sprite image, int vendorID)
        {
            artistImage = image;
            this.vendorID = vendorID;
        }
    }

    public List<ArtistData> artistData;
    public List<ArtData> artData;    

    public void CreateArtistDatabase()
    {
        string vendorJSON = MuseumJSONs.instance.vendorJSON;
        JArray jsonArray = JArray.Parse(vendorJSON);

        foreach (JObject jObject in jsonArray)
        {
            //Debug.Log("Creating art!");
            JObject data = JObject.Parse(jObject.ToString());

            //Converting Vendor Link into a Sprite Name
            //string imageLink = (string)data["images"][0]["src"];
            string imageLink = MuseumBank.GetImageLink((string)data["shop"]["description"]);
            string filename = System.IO.Path.GetFileName(imageLink);
            int index = filename.IndexOf(".");
            string finalImageLink = filename.Substring(0, index);

            Sprite imageSpirte = images.FirstOrDefault(x => x.name == finalImageLink);

            if (imageSpirte != null)
            {
                artistData.Add(new ArtistData(imageSpirte, (int)data["id"]));
            }
        }

        MatchProducts(JArray.Parse(MuseumJSONs.instance.productJSONs[0]));
        MatchProducts(JArray.Parse(MuseumJSONs.instance.productJSONs[1]));
        MatchProducts(JArray.Parse(MuseumJSONs.instance.productJSONs[2]));
    }

    void MatchProducts(JArray jsonArray)
    {
        foreach (JObject jObject in jsonArray)
        {
            //Debug.Log("Creating art!");
            JObject data = JObject.Parse(jObject.ToString());

            //Converting Vendor Link into a Sprite Name
            string imageLink = (string)data["images"][0]["src"];
            //string imageLink = MuseumBank.GetImageLink((string)data["shop"]["description"]);
            string filename = System.IO.Path.GetFileName(imageLink);
            int index = filename.IndexOf(".");
            string finalImageLink = filename.Substring(0, index);

            Sprite imageSpirte = images.FirstOrDefault(x => x.name == finalImageLink);

            if (imageSpirte != null)
            {
                artData.Add(new ArtData(imageSpirte, (int)data["id"]));
            }
        }
    }

        
}
