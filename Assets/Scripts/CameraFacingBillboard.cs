using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFacingBillboard : MonoBehaviour
{
    public Camera m_Camera;

    void LateUpdate()
    {
        if(Player.LocalPlayerInstance && !m_Camera)
            m_Camera = Player.LocalPlayerInstance.GetComponent<Player>().cam;

        if (!m_Camera) return;
        transform.LookAt(transform.position + m_Camera.transform.rotation * Vector3.forward,
            m_Camera.transform.rotation * Vector3.up);
    }
}
