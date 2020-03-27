using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
public class HoloSceneLoader : MonoBehaviour
{
    public HoloSceneInterface sceneInterface;

    [Space]
    public AudioSource source;
    public AudioClip buttonSound;

    Animator animator;
    Valve.VR.InteractionSystem.Player player;

    private void Start()
    {
        animator = GetComponent<Animator>();
        player = Valve.VR.InteractionSystem.Player.instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Finger"))
        {
            source.PlayOneShot(buttonSound);
            animator.SetTrigger("Pressed");
            player.rightHand.TriggerHapticPulse(500);
            sceneInterface.LoadScene();
        }
    }
}
