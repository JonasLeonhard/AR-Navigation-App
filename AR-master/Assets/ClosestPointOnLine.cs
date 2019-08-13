using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosestPointOnLine : MonoBehaviour {

    public Transform lineStart;
    public Transform lineEnd;
    public Transform point;
    public Transform pointOnLine;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        //http://answers.unity.com/answers/344656/view.html

        // Just subtract your ray origin from the point.
        Vector3 origin = lineStart.position - point.position;

        //dot that vector with the normalized ray vector

        Vector3 direction = (lineEnd.position - lineStart.position);
        float dot = Vector3.Dot(origin, direction.normalized);

        //That gives you the magnitude of the point projected onto the ray. 
        //Multiply that magnitude to scale the normalized ray and add back the ray world origin back to that point.

        Vector3 finalPosition = -dot * direction.normalized + lineStart.position;

        pointOnLine.position = finalPosition;

    }
}
