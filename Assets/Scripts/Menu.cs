using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Foresight.Utilities;

public class Menu : MonoBehaviour
{
    public GameObject[] menus;

    public static int index;

    private void Start()
    {
        foreach (GameObject menu in menus)
            menu.SetActive(false);

        menus[index].SetActive(true);
    }

    public void NextScene()
    {
        menus[index].SetActive(false);
        index = index.IncrementClamped(menus.Length - 1);
        menus[index].SetActive(true);
    }

    public void PreviousScene()
    {
        menus[index].SetActive(false);
        index = index.DecrementClamped();
        menus[index].SetActive(true);
    }
}
