using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatePlate : MonoBehaviour
{

    public LevelManager levelManager;

    private void OnTriggerEnter(Collider other)
    {
        


        levelManager.SendMessage("EnablePlates", this.gameObject.name, SendMessageOptions.RequireReceiver);
    }


}
