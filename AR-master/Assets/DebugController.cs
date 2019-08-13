using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugController : MonoBehaviour {
    public bool active=false;
    public GameObject floors;
    public GameObject markers;
    public GameObject sprites;

	// Use this for initialization
	void Start () {
        setActive(active);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.D))
        {
            ActiveToggle();

        }
	}

    public void ActiveToggle()
    {
        setActive(!active);
    }

    void setActive(bool a)
    {
        active = a;

        foreach (MeshRenderer m in floors.GetComponentsInChildren<MeshRenderer>())
        {
            m.enabled = active;
        }

        foreach (MeshRenderer m in markers.GetComponentsInChildren<MeshRenderer>())
        {
            m.enabled = active;
        }


        foreach (SpriteRenderer m in sprites.GetComponentsInChildren<SpriteRenderer>())
            {
                m.enabled = active;
            }


    }
}
