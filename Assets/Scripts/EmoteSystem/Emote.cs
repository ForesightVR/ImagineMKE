using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Foresight;

public class Emote : MonoBehaviour
{
    public TextMeshProUGUI emoteName;
    public TextMeshProUGUI emoteInputKey;
    public string inputKey;

    public bool Active { get; private set; }

    EmoteCategory emoteCategory;
    Animator animator;

    public void Initialize(EmoteCategory emoteCategory)
    {
        this.emoteCategory = emoteCategory;
        emoteInputKey.text = "(" + inputKey + ")";
        emoteName.text = transform.name;

        animator = Player.LocalPlayerInstance.GetComponent<Player>().Animator;
    }

    public void SetActive(bool state)
    {
        Active = state;
    }

    private void Update()
    {
        if (!Active) return;

        if(Input.GetKeyDown(inputKey))
            animator.SetTrigger(emoteName.text);
        else if(Input.GetKeyUp(inputKey))
            animator.ResetTrigger(emoteName.text);


        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Active = false;
            emoteCategory.CloseEmoteCategory();
        }
    }
}
