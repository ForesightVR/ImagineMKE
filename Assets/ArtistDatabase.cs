using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ArtistDatabase")]
public class ArtistDatabase : ScriptableObject
{
    [System.Serializable]
    public struct ArtData
    {
        public Sprite artImage;
        public int productID;
    }

    [System.Serializable]
    public struct ArtistData
    {
        public string aristName;
        public ArtData[] artData;
    }

    public ArtistData[] artistData;
}
