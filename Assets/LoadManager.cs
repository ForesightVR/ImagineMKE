﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadManager : MonoBehaviour
{
    public static LoadManager Instance;
    public GameObject loadScreen;
    public Image loadbar;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (!loadScreen.activeInHierarchy) return;

        loadbar.fillAmount = (float)MuseumBank.currentArtPieces / MuseumBank.maxArtPieces;
    }

    public void SetLoad(bool state)
    {
        loadScreen.SetActive(state);
    }
}
