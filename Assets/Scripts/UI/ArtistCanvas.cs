using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ArtistCanvas : MonoBehaviour
{
    public GameObject artInfoUI;
    public Image artPiece;
    public TextMeshProUGUI artistName;
    public TextMeshProUGUI extendedArtistName;
    public Image artistProfilePicture;
    public TextMeshProUGUI artistBio;
    public TextMeshProUGUI extendedartistBio;
    public Transform artistPiecesList;
    public Image artPieceDisplay;

    Vendor currentArtist;
    Product currentArtPiece;

    bool nearby;

    public void Nearby(bool state)
    {
        nearby = state;

        if (!nearby)
            artInfoUI.SetActive(false);
    }

    public void Interacting()
    {
        if (nearby)
            artInfoUI.SetActive(true);
        else
            artInfoUI.SetActive(false);
    }

    public void SetArtistCanvasInfo(Vendor artist, Product artPiece, string artistName, Sprite artistProfilePicture, string artistBio, List<Product> extraArt)
    {
        SetArtist(artist);
        SetArt(artPiece);
        this.artistName.text = artistName;
        this.extendedArtistName.text = artistName;
        this.artistProfilePicture.sprite = artistProfilePicture;
        this.artistBio.text = artistBio;
        this.extendedartistBio.text = artistBio;

        foreach (Product art in extraArt)
        {
            Image displayImage = Instantiate(artPieceDisplay, artistPiecesList);
            displayImage.GetComponent<ChangeArtEffect>().SetInfo(this, art);
            displayImage.sprite = art.image;
        }
    }

    public void SetArtist(Vendor artist)
    {
        currentArtist = artist;
    }

    public void SetArt(Product art)
    {
        currentArtPiece = art;
        artPiece.sprite = currentArtPiece.image;
    }

    public void AddToCart()
    {
        Cart.Instance.AddToCart(currentArtPiece);
    }
}
