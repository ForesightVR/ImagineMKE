using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class Player : MonoBehaviourPun, IPunObservable
{
    public float speed = 1;
    public float rotationSpeed = 100;
    public float runSpeed = 3;
    public TextMeshProUGUI nickName;

    public bool hideCursor = true;

    public static GameObject LocalPlayerInstance;

    [HideInInspector]
    public Camera cam;
    Animator animator;

    float horizontal;
    float vertical;
    float mouseX;
    float mouseY;
    bool leftShift;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        cam = GetComponentInChildren<Camera>();
        cam.gameObject.SetActive(false);
        nickName.text = photonView.Owner.NickName;

        if (!photonView.IsMine)
        {
            enabled = false;
        }
        else
        {
            LocalPlayerInstance = this.gameObject;
            cam.gameObject.SetActive(true);
        }
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
        leftShift = false;
        if (photonView.IsMine)
        {
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");
            leftShift = Input.GetKey(KeyCode.LeftShift);
        }

        animator.speed = leftShift ? 1.5f : 1;
        animator.SetFloat("Horizontal", horizontal, .1f, Time.deltaTime);
        animator.SetFloat("Vertical", vertical, .1f, Time.deltaTime);

        Vector3 move = new Vector3(horizontal, 0, vertical);

        animator.SetBool("Move", move == Vector3.zero ? false : true);

        move = transform.TransformDirection(move);
        move.y = 0;

        transform.position += move.normalized * (leftShift ? runSpeed :  speed) * Time.deltaTime;
    }

    void Rotate()
    {

        if (photonView.IsMine)
        {
            mouseX += Input.GetAxis("Mouse X");
            mouseY += Input.GetAxis("Mouse Y");
        }
        
        animator.SetFloat("MouseX", Input.GetAxisRaw("Mouse X"), .1f, Time.deltaTime);

        mouseX *= rotationSpeed;
        mouseY *= rotationSpeed;

        mouseY = Mathf.Clamp(mouseY, -60f, 90f);

        transform.rotation = Quaternion.Euler(0, mouseX, 0);
        cam.transform.localRotation = Quaternion.Euler(-mouseY, 0, 0);
    }

    //public static void RefereshInstance(ref Player player, Player prefab)
    //{
    //    Vector3 position = Vector3.zero;
    //    Quaternion rotation = Quaternion.identity;

    //    if (player != null)
    //    {
    //        position = player.transform.position;
    //        rotation = player.transform.rotation;
    //       // PhotonNetwork.Destroy(player.gameObject);
    //    }

    //    if(player == null)
    //        player = PhotonNetwork.Instantiate(prefab.gameObject.name, position, rotation).GetComponent<Player>();
    //}

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
