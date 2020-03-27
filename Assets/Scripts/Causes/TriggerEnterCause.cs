using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerEnterCause : Cause
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) // -> cause
    {
        //if (other.transform.tag.Equals("Finger")) // -> Enum Check
        //{
        //    cause?.Invoke(); // -> effect

        //    source.PlayOneShot(buttonSound); // -> in the inspector
        //    animator.SetTrigger("Pressed");
        //    player.rightHand.TriggerHapticPulse(500);

        //    sceneInterface.SetInfo(logo, description, sceneName); // -> also in the inspector
        //}
    }
}
