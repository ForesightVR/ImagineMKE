using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Foresight.Utilities;

public class Gallery : MonoBehaviour
{
    public CauseTag causeTag;
    public List<ArtistCanvas> artistCanvases;
    List<Vendor> artists = new List<Vendor>();

    private void Start()
    {
        artists = PullStorage.vendors.Where(x => x.GetCause() == causeTag.ToString()).ToList();

        int artistIndex = 0;

        foreach (ArtistCanvas artistCanvas in artistCanvases)
        {
            Vendor artist = artists[artistIndex];
            artistCanvas.SetArtistCanvasInfo(artist, artist.GetNextProduct(), artist.name, artist.image, artist.description, artist.products);
            artistIndex = MathUtilities.IncrementLoop(artistIndex, artists.Count - 1);
        }        
    }
}

public enum CauseTag
{
    IslandsOfBrilliance,
    TrueSkool,
    Oneida,
    Military,
    GalleryNight
}
