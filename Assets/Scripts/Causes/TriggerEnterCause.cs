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
        if (isLocal)
        {
            if(other.tag.Equals(causeTag))
            {
                PhotonView photonView = causeTag == "Player" ? other.GetComponent<PhotonView>() : other.GetComponentInParent<PhotonView>();
                if (!photonView.IsMine)
                    return;
            }
        }

        if (other.transform.tag.Equals(causeTag))
            cause?.Invoke();
    }
}
