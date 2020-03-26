using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    public static GameObject Instance;
    private void Start()
    {
        DontDestroyOnLoad(this);

        if (Instance == null)
            Instance = gameObject;
        else
            Destroy(gameObject);
    }
}
