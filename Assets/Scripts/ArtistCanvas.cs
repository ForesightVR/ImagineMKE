using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ArtistCanvas : MonoBehaviour
{
    public Image artPiece;
    public TextMeshProUGUI artistName;
    public TextMeshProUGUI extendedArtistName;
    public Image artistProfilePicture;
    public TextMeshProUGUI artistBio;
    public TextMeshProUGUI extendedartistBio;
    public Transform artistPiecesList;
    public Image artPieceDisplay;

    public void SetArtistCanvasInfo(Art artPiece, string artistName, Sprite artistProfilePicture, string artistBio, List<Art> extraArt)
    {
        this.artPiece.sprite = artPiece.artImage;
        this.artistName.text = artistName;
        this.extendedArtistName.text = artistName;
        this.artistProfilePicture.sprite = artistProfilePicture;
        this.artistBio.text = artistBio;
        this.extendedartistBio.text = artistBio;

        foreach (Art art in extraArt)
        {
            Image displayImage = Instantiate(artPieceDisplay, artistPiecesList);
            displayImage.GetComponent<ChangeArtEffect>().SetArtistCanvas(this);
            displayImage.sprite = art.artImage;
        }
    }
}
