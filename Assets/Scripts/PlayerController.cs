using Photon.Pun;
using UnityEngine;

namespace Com.MyCompany.MyGame
{
    public class PlayerController : MonoBehaviourPun
    {
        private PlayerControls playerInput;
        private Transform child;
        private CharacterController controller;
        private Transform cameraMain;
        private Vector3 playerVelocity;
        private bool groundedPlayer;
        private float playerSpeed = 2.0f;
        private float jumpHeight = 1.0f;
        private float gravityValue = -9.81f;
        private float rotationSpeed = 4f;

        private void Awake()
        {
            playerInput = new PlayerControls();
            controller = GetComponent<CharacterController>();
            cameraMain = Camera.main.transform;
        }

        private void OnEnable()
        {
            playerInput.Enable();
        }

        private void OnDisable()
        {
            playerInput.Disable();
        }

        private void Start()
        {
            cameraMain = Camera.main.transform;
            child = transform.GetChild(0).transform;
        }

        private void Update()
        {
            cameraMain = Camera.main.transform;
            if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
            {
                return;
            }

            groundedPlayer = controller.isGrounded;
            if (groundedPlayer && playerVelocity.y < 0)
            {
                playerVelocity.y = 0f;
            }

            Vector2 movementInput = playerInput.PlayerMain.Move.ReadValue<Vector2>();
            Vector3 move = (cameraMain.forward * movementInput.y + cameraMain.right * movementInput.x);

            move.y = 0f;
            controller.Move(move * Time.deltaTime * playerSpeed);

            // Changes the height position of the player..
            if (playerInput.PlayerMain.Jump.triggered && groundedPlayer)
            {
                playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            }
            playerVelocity.y += gravityValue * Time.deltaTime;
            controller.Move(playerVelocity * Time.deltaTime);

            if (movementInput != Vector2.zero)
            {
                Quaternion rotation = Quaternion.Euler(new Vector3(child.localEulerAngles.x, cameraMain.localEulerAngles.y, child.localEulerAngles.z));
                child.rotation = Quaternion.Lerp(child.rotation, rotation, Time.deltaTime * rotationSpeed);
            }
        }
    }
}