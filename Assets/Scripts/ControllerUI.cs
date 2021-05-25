using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Com.MyCompany.MyGame
{
    public class ControllerUI : MonoBehaviour
    {
        private void Awake()
        {
            this.transform.SetParent(GameObject.Find("MainCanvas").GetComponent<Transform>(), false);
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}
