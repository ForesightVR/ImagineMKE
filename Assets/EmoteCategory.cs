using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EmoteCategory : MonoBehaviour
{
    public Transform emoteBar;
    public List<Emote> emotes;

    public TextMeshProUGUI emoteCategoryName;
    public TextMeshProUGUI emoteInputKey;
    public string inputKey;

    EmoteManager emoteManager;

    public bool Active { get; private set; }

    private void Start()
    {
        emoteInputKey.text = "(" + inputKey + ")";
        emoteCategoryName.text = transform.name;
    }

    private void Update()
    {
        if (!Active) return;

        Debug.Log("Waiting for Input");
        if (Input.GetKeyDown(inputKey))
        {
            Debug.Log("Trigger Animation");
            //Trigger Animation
            OpenEmoteCategory();
        }
    }

    public void SetActive(bool state)
    {
        Active = state;
    }

    public void Initialize(EmoteManager emoteManager)
    {
        this.emoteManager = emoteManager;

        foreach (Emote emote in emotes)
            emote.Initialize(this);
    }

    public void OpenEmoteCategory()
    {
        emoteManager.TriggerTransition(emoteManager.categoryParent.gameObject, emoteBar.gameObject, emotes.Count);

        foreach (Emote emote in emotes)
            emote.SetActive(true);
    }

    public void CloseEmoteCategory()
    {
        emoteManager.TriggerTransition(emoteBar.gameObject, emoteManager.categoryParent.gameObject, 3);

        foreach (Emote emote in emotes)
            emote.SetActive(false);
    }

    public void ResetEmoteCategory()
    {
        emoteManager.TriggerTransition(emoteBar.gameObject, null, 0);

        foreach (Emote emote in emotes)
            emote.SetActive(false);
    }
}
