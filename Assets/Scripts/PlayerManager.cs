using Photon.Pun;
using UnityEngine;

namespace Com.MyCompany.MyGame
{
    public class PlayerManager : MonoBehaviourPunCallbacks, IPunObservable
	{
		#region Public Fields
		[Tooltip("The current Health of our player")]
		public float Health = 1f;

		[Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
		public static GameObject LocalPlayerInstance;



		#endregion

		#region Private Fields

		[Tooltip("The Player's UI GameObject Prefab")]
		[SerializeField]
		public GameObject PlayerUIPrefab;
		

		[Tooltip("The Beams GameObject to control")]
		[SerializeField]		
		private GameObject beams;		
		private PlayerControls inputActions;
		

		bool IsFiring;
		#endregion

		#region MonoBehaviour Callbacks

		private void Awake()
		{
			inputActions = new PlayerControls();

            if (beams == null)
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> Beams Reference.", this);
            }
            else
            {
                beams.SetActive(false);
            }

            if (photonView.IsMine)
			{
				PlayerManager.LocalPlayerInstance = this.gameObject;
			}
			DontDestroyOnLoad(this.gameObject);
		}

		private void Start()
		{
			CameraWork _cameraWork = this.gameObject.GetComponent<CameraWork>();            

			if (_cameraWork != null)
			{
				if (photonView.IsMine)
				{
					_cameraWork.OnStartFollowing();
				}
			}
			else
			{
				Debug.LogError("<Color=Red><a>Missing</a></Color> CameraWork Component on playerPrefab.", this);
			}

            if (this.PlayerUIPrefab != null)
            {
				GameObject _UIGo = Instantiate(this.PlayerUIPrefab);
				_UIGo.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);

            }
            else
            {
				Debug.LogWarning("<Color=Red><a>Missing</a></Color> PlayerUiPrefab reference on player Prefab.", this);
			}            

            

#if UNITY_5_4_OR_NEWER
			UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
#endif
		}



		public override void OnDisable()
		{
			base.OnDisable();
#if UNITY_5_4_OR_NEWER
			UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
#endif
		}


		private void Update()
		{

            if (this.transform.position.y <= -10f)
            {
				this.transform.position = new Vector3(0f, 5f, 7.5f);
            }
			// we only process Inputs and check health if we are the local player
			if (photonView.IsMine)
			{
				this.ProcessInputs();

				if (this.Health <= 0f)
				{
					GameManager.Instance.LeaveRoom();
				}
			}

            if (this.beams != null && this.IsFiring != this.beams.activeInHierarchy)
            {
                this.beams.SetActive(this.IsFiring);
            }
        }		

        private void OnTriggerEnter(Collider other)
		{
			if (!photonView.IsMine)
			{
				return;
			}
			if (!other.name.Contains("Beam"))
			{
				return;
			}

			this.Health -= 0.1f;
		}

		private void OnTriggerStay(Collider other)
		{
			if (!photonView.IsMine)
			{
				return;
			}
			if (!other.name.Contains("Beam"))
			{
				return;
			}

			Health -= 0.01f * Time.deltaTime;
		}


#if !UNITY_5_4_OR_NEWER

		void OnLevelWasLoaded(int level)
		{
			this.CalledOnLevelWasLoaded(level);
		}
#endif
		void CalledOnLevelWasLoaded(int level)
		{
			if (!Physics.Raycast(transform.position, -Vector3.up, 5f))
			{
				transform.position = new Vector3(0f, 5f, 7.5f);
			}
			GameObject _UIGo = Instantiate(this.PlayerUIPrefab);
			_UIGo.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);     
			

		}
		#endregion

		#region IPunObservable implementation

		public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
		{
			if (stream.IsWriting)
			{
				stream.SendNext(IsFiring);
				stream.SendNext(Health);
			}
			else
			{
				this.IsFiring = (bool)stream.ReceiveNext();
				this.Health = (float)stream.ReceiveNext();
			}			
		}

		#endregion

		#region Private Methods


#if UNITY_5_4_OR_NEWER
		void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode LoadingMode)
		{
			this.CalledOnLevelWasLoaded(scene.buildIndex);
		}
#endif

		void ProcessInputs()
		{
            if(inputActions.PlayerMain.Jump.triggered)
            {
                if (!IsFiring)
                {
                    IsFiring = true;
                }
            }
            //if (inputActions.PlayerMain.Jump.triggered)
            //{
            //    if (IsFiring)
            //    {
            //        IsFiring = false;
            //    }
            //}
        }

        #endregion
    }

}

