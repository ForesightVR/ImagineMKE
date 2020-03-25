using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAvatar : MonoBehaviour
{
    public Animator targetAnimator;

    public GameObject avatar1GameObject;
    public Avatar avatar1;

    public GameObject avatar2GameObject;
    public Avatar avatar2;

    GameObject currentAvatarGameObject;
    Avatar currentAvatater;

    private void Start()
    {
        currentAvatarGameObject = avatar1GameObject;
        currentAvatater = avatar1;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            SwitchAvatar();
    }

    void SwitchAvatar()
    {
        currentAvatarGameObject.SetActive(false);
        currentAvatarGameObject = currentAvatarGameObject == avatar1GameObject ? avatar2GameObject : avatar1GameObject;

        currentAvatater = targetAnimator.avatar == avatar1 ? avatar2 : avatar1;

        targetAnimator.avatar = currentAvatater;
        currentAvatarGameObject.SetActive(true);
        targetAnimator.Rebind();
    }
}
