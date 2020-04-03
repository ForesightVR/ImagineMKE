using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerExitCuase : Cause
{
    [TagSelector]
    public string causeTag;

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag.Equals(causeTag))
            cause?.Invoke();
    }
}
