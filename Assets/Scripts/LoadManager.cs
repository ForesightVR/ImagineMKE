using System.Collections;
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

    private void Start()
    {
        if (MuseumBank.isDone)
            SetLoad(false);
    }

    private void Update()
    {
        if (!loadScreen.activeInHierarchy) return;

        if (MuseumBank.stateProgress == 0)
            loadbar.fillAmount = MuseumBank.vendorDownloadProgress;
        else if (MuseumBank.stateProgress == 1)
            loadbar.fillAmount = MuseumBank.artDownloadProgress;
        else
            loadbar.fillAmount = MuseumBank.imageDownloadProgress;

        Debug.Log($"Fill amount {loadbar.fillAmount} at {Time.time} for {MuseumBank.stateProgress}");
    }

    public void SetLoad(bool state)
    {
        loadScreen.SetActive(state);
    }
}
