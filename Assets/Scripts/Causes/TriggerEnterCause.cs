using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerEnterCause : Cause
{
    [TagSelector]
    public string causeTag;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag.Equals(causeTag))
            cause?.Invoke();
    }
}
