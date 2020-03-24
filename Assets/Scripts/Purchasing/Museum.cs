using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Museum : MonoBehaviour
{
    public CauseTag causeTag;
    public List<Image> placements;
    public List<Artist> artists;
}

public enum CauseTag
{
    IslandsOfBrilliance,
    TrueSkool,
    Oneida,
    Military
}
