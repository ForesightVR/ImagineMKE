using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WebsiteInfo : MonoBehaviour
{
    public static string oauth_consumerKey = "ck_42bc844ad5f5679fa919f8104137bd276daefbe2";
    public static string oauth_consumerSecret = "cs_e65a26fd8ece512d02363359388e863f2eedc598";

    public static string vendorsApiString = "/wp-json/wcmp/v1/vendors";
    public static string productsApiString = "/wp-json/wc/v2/products";

    public static string websiteRoot = "https://outsidersvr.com";

    public static UnityWebRequest CreateGetRequest(string apiString)
    {
        string url = GenerateRequestURL(WebsiteInfo.websiteRoot + apiString);
        var request = UnityWebRequest.Get(url);
        return request;
    }
    public static UnityWebRequest CreateGetRequest(string apiString, List<string> parameters)
    {
        string url = GenerateRequestURL(WebsiteInfo.websiteRoot + apiString, parameters);
        var request = UnityWebRequest.Get(url);
        return request;
    }
    public static string GenerateRequestURL(string in_url, string HTTP_Method = "GET")
    {
        OAuth_CSharp oauth = new OAuth_CSharp(oauth_consumerKey, oauth_consumerSecret);
        string requestURL = oauth.GenerateRequestURL(in_url, HTTP_Method);

        return requestURL;
    }
    public static string GenerateRequestURL(string in_url, List<string> parameters, string HTTP_Method = "GET")
    {
        OAuth_CSharp oauth = new OAuth_CSharp(oauth_consumerKey, oauth_consumerSecret);
        string requestURL = oauth.GenerateRequestURL(in_url, HTTP_Method, parameters);

        return requestURL;
    }
}
