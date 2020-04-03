using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

public class TriggerEnterCause : Cause
{
    [TagSelector]
    public string causeTag;

    private void OnTriggerEnter(Collider other)
    {
        if (isLocal && !PhotonNetwork.LocalPlayer.IsLocal) return;

        if (other.transform.tag.Equals(causeTag))
            cause?.Invoke();
    }
}
