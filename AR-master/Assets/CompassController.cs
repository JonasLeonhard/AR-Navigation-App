using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Compass controller.
/// </summary>
public class CompassController : MonoBehaviour {

    /// <summary>
    /// The current unity-viewpoint. Uses it's position as source position for the Compass direction
    /// </summary>
    public GameObject viewpoint;
    public RouteProgressController routeProgress;

    public GameObject ARrow;
    public GameObject Shadow;
    /// <summary>
    /// The position the Compass should point to. Generally only x and z coordinates are required.
    /// </summary>
    public Vector3 pointTarget;

    public int state = 0;
    public Sprite imCompass;
    public Sprite imButton;


    RectTransform rt;

	// Use this for initialization
	void Start () {
        rt = gameObject.GetComponent<RectTransform>();
        

    }
	
	// Update is called once per frame
	void Update () {
        if (routeProgress.currentSegment >= 0)
            setTarget(routeProgress.getRoutePosition(routeProgress.progress + (1.0f / routeProgress.routeLength)));

        Vector3 uPos=viewpoint.transform.position;
        float angle = Quaternion.LookRotation(new Vector3((pointTarget.x - uPos.x), 0f, (pointTarget.z - uPos.z))).eulerAngles.y -viewpoint.transform.rotation.eulerAngles.y;

        ARrow.GetComponent<RectTransform>().rotation = Quaternion.Euler(0f, 0f, -angle);
        Shadow.GetComponent<RectTransform>().rotation=Quaternion.Euler(0f,0f,-angle);
    }

    /// <summary>
    /// Sets the target Position of the Compass.
    /// </summary>
    /// <param name="target">Target.</param>
    public void setTarget(Vector3 target)
    {
        pointTarget = target;
    }


    /// <summary>
    /// 1=button, 0=comapass
    /// </summary>
    public void setState(int state)
    {
        this.state = state;


        switch (state)
        {
            case 0:
                GetComponent<Image>().sprite = imCompass;
                ARrow.SetActive(true);
                Shadow.SetActive(true);

                break;

            case 1:
                GetComponent<Image>().sprite = imButton;
                ARrow.SetActive(false);
                Shadow.SetActive(false);


                break;
        }

        GetComponent<UIWobble>().wobble();

    }

    


}
