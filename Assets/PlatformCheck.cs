using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.iOS;

public class PlatformCheck : MonoBehaviour
{
#if UNITY_STANDALONE_OSX
    IEnumerator Start()
    {
        yield return Application.RequestUserAuthorization(UserAuthorization.Microphone);

        if (Application.HasUserAuthorization(UserAuthorization.Microphone))
            Debug.Log("Microphone Found");
        else
            Debug.Log("Could Not Find Microphone!");
    }
#endif
}
