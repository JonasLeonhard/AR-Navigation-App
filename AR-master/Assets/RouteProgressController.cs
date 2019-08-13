using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouteProgressController : MonoBehaviour
{

    public GameObject viewpoint;
    public Vector3[] route;


    public float routeLength;

    /// <summary>
    /// relative route progress per route vertex (range 0-1)
    /// </summary>
    float[] relativeProgress;
    float[] segmentSize;
    public int currentSegment = -1;
    public GameObject progressIndicator;
    public float progress=0;
    public Vector3 progressPosition;

    // Use this for initialization
    void Start()
    {
        route = new Vector3[0];
    }

    // Update is called once per frame
    void Update()
    {

        /*
        if (nextPoint != -1)
        {
            for (int n = nextPoint; n < route.Length; n++)
            {
                if ((viewpoint.transform.position - route[n]).magnitude < 1.87f)
                {
                    nextPoint += 1;
                }

                if (nextPoint >= route.Length)
                {
                    nextPoint = -1;
                    routeFinish();
                    break;
                }
            }
        }
        */
        if (route.Length > 0 && GetComponent<TrackingController>().trackingMode == TrackingController.TrackingMode.tracking)
        {
            routeProgressUpdate();

        }

    }



    /// <summary>
    /// Sets the route
    /// </summary>
    public void setRoute(Vector3[] route)
    {
        this.route = route;
        this.relativeProgress = new float[route.Length];
        segmentSize = new float[route.Length - 1];
        relativeProgress[0] = 0;
        currentSegment = 0;


        routeLength = 0;
        //get absolute length of route
        for (int n = 1; n < route.Length; n++)
        {
            segmentSize[n-1]= (route[n] - route[n - 1]).magnitude;
            routeLength += segmentSize[n - 1];
            relativeProgress[n] = routeLength;
        }

        for (int n = 1; n < route.Length; n++)
        {
            relativeProgress[n]/= routeLength;
        }

    }


    public void routeProgressUpdate()
    {
        float device_height = 1.3f; //better way: raycast for floor
        Vector3 userPos = new Vector3(viewpoint.transform.position.x, viewpoint.transform.position.y-device_height, viewpoint.transform.position.z);
        
        float distMin = 10000f;
        Vector3 position = Vector3.zero;
        int segment = -1; //segment ~ p[segment] to p[segement+1]
        float currentDot = 0f;
        //http://answers.unity.com/answers/344656/view.html


        for(int n=0; n< route.Length-1; n++)
        {
            // Just subtract your ray origin from the point.
            Vector3 origin = route[n] - userPos;
            Vector3 direction = (route[n+1] - route[n]).normalized;
            float dot =Mathf.Clamp(-(Vector3.Dot(origin, direction)),0f, (route[n + 1] - route[n]).magnitude);
            Vector3 pointOnLine = dot * direction + route[n];
            float dist = (pointOnLine - userPos).magnitude;

            if (segment==-1 || dist<distMin)
            {
                segment = n;
                distMin = dist;
                position = pointOnLine;
                currentDot = dot;

            }
        }

        currentSegment = segment;
        progress = 
            
            relativeProgress[segment] + 
            
            (relativeProgress[segment + 1] - 
            
            relativeProgress[segment]) * 
            
            (currentDot / 
            
            segmentSize[segment]);

        progressPosition = position;

        if (progressIndicator!=null)
        {
            progressIndicator.transform.position = position;
        }

    }

    public Vector3 getRoutePosition(float progress)
    {
        progress = Mathf.Clamp01(progress);

        int segment = 0;
        while(relativeProgress[segment+1]<progress)
        {
            segment += 1;
        }

        return route[segment] + (route[segment + 1] - route[segment]) * ((progress - relativeProgress[segment]) / (relativeProgress[segment + 1] - relativeProgress[segment]));



    }

}
