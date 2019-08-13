using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ScrButtonClosePopUp : MonoBehaviour {

    /// <summary>
    /// The GameObject that is gonna be closed
    /// </summary>
    public GameObject parentMenu;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void close()
    {
        Destroy(parentMenu);
    }
}
