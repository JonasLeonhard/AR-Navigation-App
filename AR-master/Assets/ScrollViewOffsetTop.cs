using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollViewOffsetTop : MonoBehaviour {


    public float[] offsets;
    RectTransform rt;
    public Text roomText;
    public GameObject routePanel;
    int state = 0; // 0=new roud button 1=roud button
	// Use this for initialization
	void Start () {
        rt = GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void Update () {
        

    }

    public void setState(string s)
    {

        if (s == null)
        {
            state = 0;
            routePanel.SetActive(false);
        }
        else
        {
            state = 1;
            roomText.text = s;
            routePanel.SetActive(true);
        }


        rt.offsetMax = new Vector2(rt.offsetMax.x, -offsets[state]);
    }

    public void setState()
    {
        setState(null);
    }


}
