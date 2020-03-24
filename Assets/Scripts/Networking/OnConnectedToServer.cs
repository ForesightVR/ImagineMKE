using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class OnConnectedToServer : MonoBehaviourPunCallbacks
{
    public UnityEvent unityEvent;

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        unityEvent?.Invoke();
    }

    public override void OnEnable()
    {
        base.OnEnable();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (PhotonNetwork.IsConnectedAndReady)
            gameObject.SetActive(false);
    }
}
