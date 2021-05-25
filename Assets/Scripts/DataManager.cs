using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;


public class DataManager : MonoBehaviourPunCallbacks, IPunObservable
{
	public static GameObject LocalPlayerInstance;

	[SerializeField]
	private float Number;

	[SerializeField]
	private Text number;

	private float baseNumber = 0;
    // Start is called before the first frame update

    private void Awake()
    {
		if (photonView.IsMine)
		{
			DataManager.LocalPlayerInstance = this.gameObject;
		}
		DontDestroyOnLoad(this.gameObject);
	}
    void Start()
    {
		number.text = baseNumber.ToString();
    }

    // Update is called once per frame
    void Update()
    {
		number.text = baseNumber.ToString();
	}

	public void IncreaseNumber()
    {
		baseNumber++;

        if (baseNumber >= 99)
        {
			baseNumber = 99;
        }
    }
	public void DecreaseNumber()
    {
		baseNumber--;
		if (baseNumber <= 0)
		{
			baseNumber = 0;
		}
	}


	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.IsWriting)
		{
			stream.SendNext(baseNumber);
		}
		else
		{
			this.baseNumber = (float)stream.ReceiveNext();
		}
	}
}
