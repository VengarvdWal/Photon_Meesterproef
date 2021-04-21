using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;

namespace Com.MyCompany.MyGame
{

	public class GameManager : MonoBehaviourPunCallbacks
	{

#region Public Fields
		public static GameManager Instance;
		public GameObject playerPrefab;

#endregion

#region MonoBehaviour Callbacks

		private void Start()
		{
			Instance = this;
			if (playerPrefab == null)
			{
				Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
			}
			else
			{
				if (PlayerManager.LocalPlayerInstance == null)
				{
					Debug.LogFormat("We are Instantiating LocalPlayer from {0}", Application.loadedLevelName);

					PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
				}
				else
				{
					Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
				}
#if UNITY_5_4_OR_NEWER
				UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
#endif

			}
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
				transform.position = new Vector3(0f, 5f, 0f);
			}
		}


#if UNITY_5_4_OR_NEWER
		public override void OnDisable()
		{
			base.OnDisable();
			UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
		}
#endif

#endregion



#region Photon Callbacks

		public override void OnLeftRoom()
		{
			SceneManager.LoadScene(0);
		}

		public override void OnPlayerEnteredRoom(Player other)
		{
			Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName);

			if (PhotonNetwork.IsMasterClient)
			{
				Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient);

				LoadArena();
			}
		}
	

		public override void OnPlayerLeftRoom(Player other)
		{
			Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName);


			if (PhotonNetwork.IsMasterClient)
			{
				Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient);

				LoadArena();
			}
		}

#endregion

#region Public Methods

		public void LeaveRoom()
		{
			PhotonNetwork.LeaveRoom();
		}

#endregion

#region Private Methods
		void LoadArena()
		{
			if (!PhotonNetwork.IsMasterClient)
			{
				Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
			}
			Debug.LogFormat("PhotonNetwork : Loading Level : {0}", PhotonNetwork.CurrentRoom.PlayerCount);
			PhotonNetwork.LoadLevel("Room for " + PhotonNetwork.CurrentRoom.PlayerCount);

		}

#if UNITY_5_4_OR_NEWER
		void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode LoadingMode)
		{
			this.CalledOnLevelWasLoaded(scene.buildIndex);
		}
#endif

#endregion


	}

}
