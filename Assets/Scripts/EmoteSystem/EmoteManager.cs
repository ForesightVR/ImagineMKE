using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Foresight;
using Photon.Pun;

public class EmoteManager : MonoBehaviour
{
    public RectTransform emoteObject;
    public Transform categoryParent;
    public EmoteCategory[] emoteCategories;
    public float speed = 2000f;

    bool isOpen = false;

    Dictionary<int, float> emoteBarLength = new Dictionary<int, float>();

    private void Start()
    {
        if (!transform.root.GetComponent<PhotonView>().IsMine)
        {
            enabled = false;
            return;
        }

        Initialize();

        foreach (EmoteCategory category in emoteCategories)
            category.Initialize(this);
    }

    void Initialize()
    {
        emoteBarLength.Add(0, 0);
        emoteBarLength.Add(1, 250);
        emoteBarLength.Add(2, 450);
        emoteBarLength.Add(3, 650);
        emoteBarLength.Add(4, 850);
        emoteBarLength.Add(5, 1050);
        emoteBarLength.Add(6, 1250);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            if(isOpen)
            {
                isOpen = false;
                TriggerTransition(categoryParent.gameObject, null, 0);

                foreach (EmoteCategory category in emoteCategories)
                {
                    category.ResetEmoteCategory();
                    category.SetActive(false);
                }
            }
            else
            {
                isOpen = true;
                TriggerTransition(null, categoryParent.gameObject, 3);

                foreach (EmoteCategory category in emoteCategories)
                    category.SetActive(true);
            }
        }
    }

    public void SelectCategory(EmoteCategory emoteCategory)
    {

    }

    public void PlayEmote(Emote emote)
    {

    }

    public void TriggerTransition(GameObject objectToDisable, GameObject objectToEnable, int emoteCount)
    {
        StartCoroutine(Transition(objectToDisable, objectToEnable, emoteCount));
    }

    IEnumerator Transition(GameObject objectToDisable, GameObject objectToEnable, int emoteCount)
    {
        yield return new WaitUntil(() => InPosition(0));

        if(objectToDisable)
            objectToDisable.SetActive(false);

        if(objectToEnable)
            objectToEnable.SetActive(true);

        float targetWidth = GetBarLength(emoteCount);
        yield return new WaitUntil(() => InPosition(targetWidth));
    }

    bool InPosition(float targetWidth)
    {
        emoteObject.sizeDelta = new Vector2(Mathf.MoveTowards(emoteObject.sizeDelta.x, targetWidth, speed * Time.deltaTime), emoteObject.sizeDelta.y);
        return emoteObject.sizeDelta.x == targetWidth;
    }

    float GetBarLength(int emoteCount)
    {
        return emoteBarLength[emoteCount];
    }
}

