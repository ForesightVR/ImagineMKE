using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Foresight;
using System.Linq;

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

    public List<Renderer> renderers = new List<Renderer>();

    private void Start()
    {
        player = Player.LocalPlayerInstance.transform;
    }

    public void LateUpdate()
    {
        //character.SetActive(thirdPersonActive ? true : false);

        if(renderers.Count <= 0)
            renderers = Player.LocalPlayerInstance.characterSelected.GetComponentsInChildren<Renderer>().ToList();

        if (renderers.Count <= 0) return;

        if (thirdPersonActive)
        {
            if(renderers[0].gameObject.layer == LayerMask.NameToLayer("FirstPerson"))
            {
                foreach (Renderer rend in renderers)
                    rend.gameObject.layer = LayerMask.NameToLayer("Default");
            }

            thirdPerson = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * turnSpeed, Vector3.up) * thirdPerson;
            transform.position = Vector3.SmoothDamp(transform.position, player.position + thirdPerson, ref velocity,  transitionSpeed);
            transform.LookAt(player.position + Vector3.up * lookAtHeight);
        }
        else
        {
            if (renderers[0].gameObject.layer == LayerMask.NameToLayer("Default"))
            {
                foreach (Renderer rend in renderers)
                    rend.gameObject.layer = LayerMask.NameToLayer("FirstPerson");
            }

            transform.position = Vector3.SmoothDamp(transform.position, player.position + firstPerson, ref velocity, transitionSpeed);
        }
    }
}
