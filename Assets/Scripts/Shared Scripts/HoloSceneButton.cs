using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR.InteractionSystem;
public class HoloSceneButton : MonoBehaviour
{
    public UnityEvent holoClick;
    public HoloSceneInterface sceneInterface;
    public Sprite logo;
    [TextArea]
    public string description;
    public string sceneName;

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

    private void OnTriggerEnter(Collider other) // -> cause
    {
        if (other.transform.tag.Equals("Finger")) // -> Enum Check
        {
            holoClick?.Invoke(); // -> effect

            source.PlayOneShot(buttonSound); // -> in the inspector
            animator.SetTrigger("Pressed");
            player.rightHand.TriggerHapticPulse(500);

            sceneInterface.SetInfo(logo, description, sceneName); // -> also in the inspector
        }
    }
}
