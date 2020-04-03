using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TriggerExitCuase : Cause
{
    [TagSelector]
    public string causeTag;

    private void OnTriggerExit(Collider other)
    {
        if (isLocal && !PhotonNetwork.LocalPlayer.IsLocal) return;

        if (other.transform.tag.Equals(causeTag))
            cause?.Invoke();
    }
}
