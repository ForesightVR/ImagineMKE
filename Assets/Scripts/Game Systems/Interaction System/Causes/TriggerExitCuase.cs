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
        if (isLocal && causeTag == "Art Canvas Trigger")
        {
            if (other.tag.Equals(causeTag))
            {
                if (!other.GetComponentInParent<PhotonView>().IsMine)
                    return;
            }
        }

        if (other.transform.tag.Equals(causeTag))
            cause?.Invoke();
    }
}
