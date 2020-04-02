using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Foresight.Utilities;

public class Museum : MonoBehaviour
{
    public CauseTag causeTag;
    public List<Image> placements;
    List<Artist> artists;

    private void Start()
    {
        artists = MuseumBank.artists.Where(x => x.GetCause() == causeTag.ToString()).ToList();

        int artistIndex = 0;

        foreach (Image place in placements)
        {
            place.sprite = artists[artistIndex].GetNextArt().artImage;
            artistIndex = MathUtilities.IncrementLoop(artistIndex, artists.Count - 1);
        }        
    }
}

public enum CauseTag
{
    IslandsOfBrilliance,
    TrueSkool,
    Oneida,
    Military
}
