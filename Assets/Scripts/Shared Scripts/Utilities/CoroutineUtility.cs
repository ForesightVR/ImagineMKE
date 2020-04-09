using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineUtility : MonoBehaviour
{
    public static CoroutineUtility instance;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        StartCoroutine(MuseumBank.CallGet());
    }
}
