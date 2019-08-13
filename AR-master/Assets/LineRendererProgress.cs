using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRendererProgress : MonoBehaviour {

    public RouteProgressController progress;
    LineRenderer lr;
    // Use this for initialization
    void Start () {
        lr =gameObject.GetComponent<LineRenderer>();
	}

    // Update is called once per frame
    void Update() {

        /*
        AnimationCurve curve = lr.widthCurve;

        int prevPoint = Mathf.Max(0, progress.nextPoint - 2);
        int currentPoint = Mathf.Max(0, progress.nextPoint - 1);
        int endPoint = Mathf.Min(progress.route.Length, progress.nextPoint + 2);

        float distToCurrent = 0;
        float distToPrev = 0;
        float distToEnd = 0;
        float distTotal = 0;
        float dist = 0;

        for (int n = 0; n < progress.route.Length - 1; n++)
        {

            if (n == prevPoint)
            {
                distToPrev = dist;
            }
            if (n == currentPoint)
            {
                distToCurrent = dist;
            }

            if (n == endPoint)
            {
                distToEnd = dist;
            }

            dist += (progress.route[n + 1] - progress.route[n]).magnitude;
        }
        distTotal = dist;

        float ratioPerv = distToCurrent / distTotal;
        float ratioCurrent = distToCurrent / distTotal;
        float ratioEnd = distToEnd / distTotal;

        lr.widthCurve = new AnimationCurve(new Keyframe[] { new Keyframe(ratioPerv, 0f),new Keyframe(ratioCurrent,1f),new Keyframe(ratioEnd,0f) });

        Debug.Log("etgsfutgrzezzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzz: " + ratioCurrent);


        //curve.MoveKey
        */

    }
}
