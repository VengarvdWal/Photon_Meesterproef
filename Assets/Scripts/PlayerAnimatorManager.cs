using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


namespace Com.MyCompany.MyGame
{
	public class PlayerAnimatorManager : MonoBehaviourPun
	{
		#region Private Fields	

		[SerializeField]
		private Animator animator;
		private float directionDampTime = 0.25f;
		private PlayerControls inputActions;
		private CharacterController controller;

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
			inputActions = new PlayerControls();
			
        }
        // Start is called before the first frame update
        void Start()
		{
			controller = GetComponent<CharacterController>();
			animator = GetComponent<Animator>();
			if (!animator)
			{
				Debug.LogError("PlayerAnimatorManager is Missing Animator Component", this);
			}
		}

		// Update is called once per frame
		void Update()
		{
			if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
			{
				return;
			}

			if (!animator)
			{
				return;
			}

			AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

			if (stateInfo.IsName("Base Layer.Run"))
			{
				if (inputActions.PlayerMain.Jump.triggered)
				{
					animator.SetTrigger("Jump");
				}
			}

			Vector2 movement = inputActions.PlayerMain.Move.ReadValue<Vector2>();
			Vector3 move = new Vector3(movement.x, 0, movement.y);
			controller.Move(move * Time.deltaTime * 5);

			float h = inputActions.PlayerMain.Move.ReadValue<Vector2>().x;
			float v = inputActions.PlayerMain.Move.ReadValue<Vector2>().y;


			//float h = Input.GetAxis("Horizontal");
			//float v = Input.GetAxis("Vertical");

			Debug.Log(movement);

			animator.SetFloat("Speed", h * h + v * v);
			animator.SetFloat("Direction", h, directionDampTime, Time.deltaTime);
		}

		#endregion
	}
}
