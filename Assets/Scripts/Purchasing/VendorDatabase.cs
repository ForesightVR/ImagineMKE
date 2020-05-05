using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Valve.Newtonsoft.Json;
using Valve.Newtonsoft.Json.Linq;

[CreateAssetMenu(fileName = "VendorDatabase")]
public class VendorDatabase : ScriptableObject
{
    [InspectorButton("CreateVendorDatabase")]
    public bool areVendorsCreated;

    public List<Sprite> images;

    [System.Serializable]
    public struct ProductData
    {
        public Sprite productImage;
        public int productID;

        public ProductData(Sprite image, int productID)
        {
            productImage = image;
            this.productID = productID;
        }
    }

    [System.Serializable]
    public struct VendorData
    {
        public Sprite vendorImage;
        public int vendorID;

        public VendorData(Sprite image, int vendorID)
        {
            vendorImage = image;
            this.vendorID = vendorID;
        }
    }

    public List<VendorData> vendorDataCollection;
    public List<ProductData> productDataCollection;    

    public void CreateVendorDatabase()
    {
        string vendorJSON = StoredJSONs.instance.vendorJSON.text;
        JArray vendorArray = JArray.Parse(vendorJSON);

        dynamic array = JsonConvert.DeserializeObject(vendorJSON);

        foreach (JObject data in vendorArray)
        {
            //Get URL of the Image
            string imageLink = PullStorage.GetImageLink((string)data["shop"]["description"]);
            if (string.IsNullOrWhiteSpace(imageLink))
                continue;

            //Converting Vendor Link into a Sprite Name
            string filename = System.IO.Path.GetFileName(imageLink);
            int index = filename.LastIndexOf("-");
            string finalImageLink = filename.Substring(0, index);

            Sprite imageSpirte = images.FirstOrDefault(x => x.name == finalImageLink);

            if (imageSpirte != null)
            {
                vendorDataCollection.Add(new VendorData(imageSpirte, (int)data["id"]));
            }
        }

        foreach (TextAsset JSON in StoredJSONs.instance.productJSONs)
        {
            MatchProducts(JArray.Parse(JSON.text));
        }

        /*MatchProducts(JArray.Parse(StoredJSONs.instance.productJSONs[0].text));
        MatchProducts(JArray.Parse(StoredJSONs.instance.productJSONs[1].text));
        MatchProducts(JArray.Parse(StoredJSONs.instance.productJSONs[2].text));*/
    }

    void MatchProducts(JArray jsonArray)
    {
        foreach (JObject data in jsonArray)
        {
            //Converting Vendor Link into a Sprite Name
            string imageLink = (string)data["images"][0]["src"];
            
            string filename = System.IO.Path.GetFileName(imageLink);
            if (string.IsNullOrWhiteSpace(imageLink))
                continue;

            int index = filename.LastIndexOf(".");
            string finalImageLink = filename.Substring(0, index);

            Sprite imageSpirte = images.FirstOrDefault(x => x.name == finalImageLink);

            if (imageSpirte != null)
            {
                productDataCollection.Add(new ProductData(imageSpirte, (int)data["id"]));
            }
        }
    }

        
}
