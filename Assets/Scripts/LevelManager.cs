using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private bool plate1Active, plate2Active;
    [SerializeField]
    private GameObject[] platforms;
    


    private void Awake()
    {
        plate1Active = false;
        plate2Active = false;

        foreach (var item in platforms)
        {
            item.SetActive(false);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (plate1Active && plate2Active)
        {
            foreach (var item in platforms)
            {
                item.SetActive(true);
            }
        }
    }

    public void EnablePlates(string name)
    {
        if (name == "Plate1")
        {
            plate1Active = true;
        }
        if (name == "Plate2")
        {
            plate2Active = true;
        }
    }
}
