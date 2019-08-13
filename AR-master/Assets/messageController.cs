using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class messageController : MonoBehaviour {

    public GameObject pfMessage;
    

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public GameObject message(string s,float displayTime)
    {
        GameObject m = Instantiate(pfMessage, gameObject.transform);
        m.GetComponentInChildren<Text>().text = s;
        m.GetComponent<message>().timeToLive = displayTime;


        return m;
    }

    public GameObject message(string s, float displayTime, string singletonTag)
    {
        GameObject m = message(s, displayTime);

        foreach(message mess in FindObjectsOfType<message>())
        {
            if(mess.singletonTag.Equals(singletonTag))
            {
                mess.timeToLive = -1f;
            }
        }

        m.GetComponent<message>().singletonTag = singletonTag;


        return m;
    }
    }
