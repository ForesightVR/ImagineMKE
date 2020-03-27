using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class HoloInteractable : Interactable
{
    public GameObject hologram;
    public GameObject hologramProjector;
    public Animator animator;

    public AudioClip onSound;
    public AudioClip offSound;

    AudioSource source;
    Valve.VR.InteractionSystem.Player player;

    private void Start()
    {
        source = GetComponent<AudioSource>();
        player = Valve.VR.InteractionSystem.Player.instance;
    }

    public override void Interacting()
    {
        if (!hologram.activeInHierarchy)
        {
            player.leftHand.TriggerHapticPulse(1000);
            source.PlayOneShot(onSound);
            hologram.gameObject.SetActive(true);
            hologramProjector.SetActive(true);
            animator.SetTrigger("ScaleUp");
        }
    }

    public override void StoppedInteracting()
    {
        if (hologram.activeInHierarchy)
        {
            player.leftHand.TriggerHapticPulse(1000);
            source.PlayOneShot(offSound);
            animator.SetTrigger("ScaleDown");
            hologramProjector.SetActive(false);
        }
    }
}
