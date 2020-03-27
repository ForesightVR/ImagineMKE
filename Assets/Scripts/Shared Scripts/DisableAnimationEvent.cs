using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableAnimationEvent : Interactable
{
    public void Disable()
    {
        gameObject.SetActive(false);
    }
}
