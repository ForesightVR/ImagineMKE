using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class CameraFacingBillboard : MonoBehaviourPunCallbacks
{
    public Camera m_Camera;

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        m_Camera = Player.LocalPlayerInstance.GetComponent<Player>().cam;
        base.OnPlayerEnteredRoom(newPlayer);
    }

    void LateUpdate()
    {
        if (!m_Camera) return;
        transform.LookAt(transform.position + m_Camera.transform.rotation * Vector3.forward,
            m_Camera.transform.rotation * Vector3.up);
    }
}
