using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Foresight;

public class CameraControl : MonoBehaviour
{
    public Vector3 thirdPerson;
    public Vector3 firstPerson;
    public GameObject character;

    public float transitionSpeed;
    public float turnSpeed = 4.0f;
    public float lookAtHeight = 3;

    public bool thirdPersonActive;

    Vector3 velocity;
    Transform player;

    private void Start()
    {
        player = Player.LocalPlayerInstance.transform;
    }

    public void LateUpdate()
    {
        //character.SetActive(thirdPersonActive ? true : false);

        if (thirdPersonActive)
        {
            thirdPerson = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * turnSpeed, Vector3.up) * thirdPerson;
            transform.position = Vector3.SmoothDamp(transform.position, player.position + thirdPerson, ref velocity,  transitionSpeed);
            transform.LookAt(player.position + Vector3.up * lookAtHeight);
        }
        else
            transform.position = Vector3.SmoothDamp(transform.position, player.position + firstPerson, ref velocity, transitionSpeed);
    }
}
