using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class message : MonoBehaviour {


    public float timeToLive;
    int state;
    public string singletonTag;

    // Use this for initialization
    void Start () {
        state = 0;

    }
	
	// Update is called once per frame
	void Update () {
        timeToLive -= Time.deltaTime;

        if(timeToLive<=0)
        {
            Destroy(gameObject);
        }

    }
}
