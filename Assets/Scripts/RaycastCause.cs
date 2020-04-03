using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RaycastCause : Cause
{
    public LayerMask interactionLayer;
    Camera mainCamera;

    private void Start()
    {
        mainCamera = transform.root.GetComponentInChildren<Camera>();
    }

    void Update()
    {
        if (isLocal && !PhotonNetwork.LocalPlayer.IsLocal) return;

        Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f, interactionLayer))
        {
            if(Input.GetKeyDown(KeyCode.Mouse0))
            {
                hit.transform.GetComponent<Effect>().InvokeEffect();
            }
        }
    }
}
