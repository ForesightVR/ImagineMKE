using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

namespace Foresight
{
    public class Player : MonoBehaviourPun//, IPunObservable
    {
        [Header("Movement")]
        public float speed = 1;
        public float runSpeed = 3;
        public float backwardSpeed = .5f;
        public float rotationSpeed = 100;

        [Space, Header("Animation")]
        public float animationSmoothTime = .1f;
        public float baseAnimatorSpeed = 1f;
        public float sprintAnimatorSpeed = 1.5f;

        [Space]
        public TextMeshProUGUI nickName;

        public bool hideCursor = true;

        public static GameObject LocalPlayerInstance;

        public CameraFacingBillboard cameraFacingBillboard;
        public List<GameObject> characterOptions;

        [HideInInspector]
        public Camera cam;
        public CameraControl cameraControl;

        public Animator Animator { get; private set; }
        CharacterController cc;

        float horizontal;
        float vertical;
        float mouseX;
        float mouseY;
        bool leftShift;

        void Awake()
        {
            DontDestroyOnLoad(this.gameObject);

            cc = GetComponent<CharacterController>();
            cam = GetComponentInChildren<Camera>();
            cam.gameObject.SetActive(false);
            nickName.text = photonView.Owner.NickName;

            if (!photonView.IsMine)
            {
                enabled = false;
            }
            else
            {
                photonView.RPC("RPCSelectCharacter", RpcTarget.AllBuffered, NetworkConnectionManager.Instance.CharacterSelected);
                LocalPlayerInstance = this.gameObject;
                cam.gameObject.SetActive(true);
            }

            float height = characterOptions[NetworkConnectionManager.Instance.CharacterSelected].GetComponent<CharacterInfo>().cameraHeight;
            cam.transform.localPosition = new Vector3(cam.transform.localPosition.x, height, cam.transform.localPosition.z);
        }

        private void Start()
        {
            if (hideCursor)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        [PunRPC]
        void RPCSelectCharacter(byte index)
        {
            characterOptions[index].SetActive(true);
            Animator = GetComponentInChildren<Animator>();
            Animator.Rebind();
            GetComponent<PhotonAnimatorView>().SetAnimator(Animator);
        }

        private void Update()
        {
            if (GameManager.Instance && GameManager.Instance.MenuOpen) return;
            Move();
            Rotate();
        }

        void Move()
        {
            leftShift = false;

            if (photonView.IsMine)
            {
                horizontal = Input.GetAxisRaw("Horizontal");
                vertical = Input.GetAxisRaw("Vertical");
                leftShift = Input.GetKey(KeyCode.LeftShift);
            }

            Vector3 move = new Vector3(horizontal, 0, vertical);
            float currentSpeed = 0;

            if (move.magnitude > 0)
                cameraControl.thirdPersonActive = false;

            Animator.speed = leftShift ? sprintAnimatorSpeed : baseAnimatorSpeed;
            Animator.SetFloat("Horizontal", move.x, animationSmoothTime, Time.deltaTime);
            Animator.SetFloat("Vertical", move.z, animationSmoothTime, Time.deltaTime);

            Animator.SetBool("Move", move == Vector3.zero ? false : true);
            Animator.SetBool("Run", leftShift ? true : false);

            if (leftShift) //IsSprinting
                currentSpeed = runSpeed;
            else if (move.z < 0)    //IsMovingBackwards
                currentSpeed = backwardSpeed;
            else
                currentSpeed = speed;

            move = transform.TransformDirection(move);
            move.y = 0;

            cc.SimpleMove(move.normalized * currentSpeed);
        }

        void Rotate()
        {
            //if (Input.GetKeyDown(KeyCode.Tab))
            //    cameraControl.thirdPersonActive = !cameraControl.thirdPersonActive;

            if (cameraControl.thirdPersonActive) return;
            //if (cameraControl.thirdPersonActive)
            //{
            //    if(cc.velocity != Vector3.zero)
            //        transform.rotation = Quaternion.RotateTowards(transform.rotation,  Quaternion.LookRotation(cam.transform.forward), 250 * Time.deltaTime);
            //}
            //else
            //{
                if (photonView.IsMine)
                {
                    mouseX += Input.GetAxis("Mouse X");
                    mouseY += Input.GetAxis("Mouse Y");
                }

                Animator.SetFloat("MouseX", Input.GetAxisRaw("Mouse X"), .1f, Time.deltaTime);

                mouseX *= rotationSpeed;
                mouseY *= rotationSpeed;

                mouseY = Mathf.Clamp(mouseY, -60f, 90f);

                transform.rotation = Quaternion.Euler(0, mouseX, 0);
                cam.transform.localRotation = Quaternion.Euler(-mouseY, 0, 0);
          //  }
        }
    }
}

