using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[ExecuteInEditMode]
public class EventEffect : Effect
{
    public UnityEvent unityEvent;

    void Awake()
    {
        methodArray = GetMethodNames((typeof(EventEffect))).ToArray();
    }

    public void TriggerEvent()
    {
        unityEvent?.Invoke();
    }
}
