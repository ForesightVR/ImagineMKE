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

    Artist currentArtist;
    Art currentArtPiece;

    public void SetArtistCanvasInfo(Artist artist, Art artPiece, string artistName, Sprite artistProfilePicture, string artistBio, List<Art> extraArt)
    {
        SetArtist(artist);
        SetArt(artPiece);
        this.artistName.text = artistName;
        this.extendedArtistName.text = artistName;
        this.artistProfilePicture.sprite = artistProfilePicture;
        this.artistBio.text = artistBio;
        this.extendedartistBio.text = artistBio;

        foreach (Art art in extraArt)
        {
            Image displayImage = Instantiate(artPieceDisplay, artistPiecesList);
            displayImage.GetComponent<ChangeArtEffect>().SetInfo(this, art);
            displayImage.sprite = art.artImage;
        }
    }

    public void SetArtist(Artist artist)
    {
        currentArtist = artist;
    }

    public void SetArt(Art art)
    {
        currentArtPiece = art;
        artPiece.sprite = currentArtPiece.artImage;
    }

    public void AddToCart()
    {
        Cart.Instance.AddToCart(currentArtPiece);
    }
}
