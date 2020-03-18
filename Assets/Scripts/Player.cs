using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Player : MonoBehaviourPun, IPunObservable
{
    public float speed = 5;
    public float rotationSpeed = 100;
    public bool hideCursor = true;

    Camera cam;
    Animator animator;

    float horizontal;
    float vertical;
    float mouseX;
    float mouseY;

    void Awake()
    {
        cam = GetComponentInChildren<Camera>();
        cam.gameObject.SetActive(false);

        if (!photonView.IsMine)
            enabled = false;
        else
            cam.gameObject.SetActive(true);
    }

    private void Start()
    {
        animator = GetComponent<Animator>();

        if(hideCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private void Update()
    {
        Move();
        Rotate();
    }

    void Move()
    {
        if (photonView.IsMine)
        {
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");
        }

        animator.SetFloat("Horizontal", horizontal);
        animator.SetFloat("Vertical", vertical);

        Vector3 move = new Vector3(horizontal, 0, vertical);

        move = transform.TransformDirection(move);
        move.y = 0;


        transform.position += move.normalized * speed * Time.deltaTime;

        
    }

    void Rotate()
    {

        if (photonView.IsMine)
        {
            mouseX += Input.GetAxis("Mouse X");
            mouseY += Input.GetAxis("Mouse Y");
        }

        mouseX *= rotationSpeed;
        mouseY *= rotationSpeed;

        mouseY = Mathf.Clamp(mouseY, -90f, 90f);

        transform.rotation = Quaternion.Euler(0, mouseX, 0);
        cam.transform.localRotation = Quaternion.Euler(-mouseY, 0, 0);
    }

    public static void RefereshInstance(ref Player player, Player prefab)
    {
        Vector3 position = Vector3.zero;
        Quaternion rotation = Quaternion.identity;

        if (player != null)
        {
            position = player.transform.position;
            rotation = player.transform.rotation;
            PhotonNetwork.Destroy(player.gameObject);
        }

        player = PhotonNetwork.Instantiate(prefab.gameObject.name, position, rotation).GetComponent<Player>();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(horizontal);
            stream.SendNext(vertical);
            stream.SendNext(mouseX);
            stream.SendNext(mouseY);
        }
        else
        {
            horizontal = (float)stream.ReceiveNext();
            vertical = (float)stream.ReceiveNext();
            mouseX = (float)stream.ReceiveNext();
            mouseY = (float)stream.ReceiveNext();
        }
    }
}
