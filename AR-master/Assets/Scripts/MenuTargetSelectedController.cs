using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuTargetSelectedController : MonoBehaviour {

    public int displayTime;
    private float t;
	// Use this for initialization
	void Start () {
        t = 0;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        t += Time.deltaTime;

        if (t > displayTime)
        {
            Destroy(gameObject);
        }
	}


   
    public void set(string roomName,bool success)
    {
        Text t = gameObject.GetComponentInChildren<Text>();
        if (success)
        {
            t.text = "Navigation zu Raum " + roomName + " gestartet!";
        }
        else
        {
            t.text = "Navigation zu Raum " + roomName + " konnte nicht gestartet werden! Es wurde kein möglicher Pfad gefunden.";
        }
    }

}
