using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Foresight.Utilities;

public class Museum : MonoBehaviour
{
    public CauseTag causeTag;
    public List<ArtistCanvas> artistCanvases;
    List<Artist> artists = new List<Artist>();

    private void Start()
    {
        artists = MuseumBank.artists.Where(x => x.GetCause() == causeTag.ToString()).ToList();

        Debug.Log("Artist Count: "  + artists.Count);
        int artistIndex = 0;

        foreach (ArtistCanvas artistCanvas in artistCanvases)
        {
            Artist artist = artists[artistIndex];
            artistCanvas.SetArtistCanvasInfo(artist, artist.GetNextArt(), artist.name, artist.artistImage, artist.description, artist.artPieces);
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
