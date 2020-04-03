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
        if (isLocal && causeTag == "Player")
        {
            if(other.tag.Equals(causeTag))
            {
                if (!other.GetComponent<PhotonView>().IsMine)
                    return;
            }
        }

        if (other.transform.tag.Equals(causeTag))
            cause?.Invoke();
    }
}
