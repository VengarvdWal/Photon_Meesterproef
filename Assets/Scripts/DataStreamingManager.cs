using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class DataStreamingManager : MonoBehaviourPunCallbacks
{
	public static DataStreamingManager Instance;

	[SerializeField]
    private GameObject playerUI;

    private void Awake()
    {
		Instance = this;
	}
    // Start is called before the first frame update
    void Start()
    {
		if (!PhotonNetwork.IsConnected)
		{
			SceneManager.LoadScene("Launcher");

			return;
		}
		if (playerUI == null)
		{
			Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
		}
		else
		{
			if (DataManager.LocalPlayerInstance == null)
			{
				Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);
				               
				GameObject go = PhotonNetwork.Instantiate(this.playerUI.name, new Vector3(0f, 0f, 0f), Quaternion.identity, 0);
				go.transform.SetParent(GameObject.Find("Canvas").GetComponent<Transform>(), false);
			}
			else
			{
				Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
			}
		}
	}

	public void LeaveRoom()
	{
		PhotonNetwork.LeaveRoom();
	}

	public override void OnLeftRoom()
	{
		SceneManager.LoadScene("Launcher");
	}


	// Update is called once per frame
	void Update()
    {
        
    }
}
