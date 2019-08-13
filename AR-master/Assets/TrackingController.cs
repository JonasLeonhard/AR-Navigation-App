using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingController : MonoBehaviour {

    public TrackingMode trackingMode;

    public GameObject ARCoreDevice;
    public GameObject unityViewpoint;


    /// <summary>
    /// how many "samples" for each stat should be tracked. Higher numbers mean more data and more processing
    /// </summary>
    public int statSampleSize;


    /// <summary>
    /// over what timeperiod stats should be tracked. Lower values mean denser data.
    /// </summary>
    public int statSampleTime;

    float statDelta;
    float statDeltaCurrent;

    int statSampleCurrent;

    float statProgress; //per second
    float statDeviceHeight;

    float[] statTimeArray;
    float[] statProgressArray;
    float[] statDeviceHeightArray;

    Vector3 unityViewpointPosPrev;

    float ghostingProgressStart;
    float ghostingProgressSpeed;
    float ghostingTimeStart;

    RouteProgressController routeProgress;


    // Use this for initialization
    void Start () {
        statDelta = (float)statSampleTime / (float)statSampleSize;
        statDeltaCurrent = 0f;

        statTimeArray = new float[statSampleSize];
        statProgressArray = new float[statSampleSize];
        statDeviceHeightArray = new float[statSampleSize];

        unityViewpointPosPrev = Vector3.zero;

        routeProgress = GetComponent<RouteProgressController>();


    }
	
	// Update is called once per frame
	void Update () {

        statDeltaCurrent += Time.deltaTime;
        if(statDeltaCurrent>statDelta)
        {
            
            trackStats(Time.time);
            statDeltaCurrent = 0;
        }

        

        switch(trackingMode)
        {
            case TrackingMode.tracking:

                if(checkForWallPass())
                {
                    Debug.Log("Wall pass");

                    //reset unity viewpoint to path.

                    Vector3 deltaMove = GetComponent<RouteProgressController>().progressPosition - unityViewpoint.transform.position + new Vector3(0f,statDeviceHeight,0f);

                    RelativeOrientation r = ARCoreDevice.GetComponent<RelativeOrientation>();

                    r.worldOffsetPostion = new Vector3(r.worldOffsetPostion.x + deltaMove.x, r.worldOffsetPostion.y + deltaMove.y, r.worldOffsetPostion.z + deltaMove.z);

                   
                }


                break;

            case TrackingMode.ghosting:

                routeProgress.progress = ghostingProgressStart + ghostingProgressSpeed * (Time.time - ghostingTimeStart);
                //unityViewpoint.transform.position = routeProgress.getRoutePosition(routeProgress.progress) + new Vector3(0f, 1.4f, 0f);

                unityViewpoint.transform.position = routeProgress.getRoutePosition(routeProgress.progress) + new Vector3(0f, 1.4f, 0f);

                //get orientation from gyro instead arcore
                /*
                RelativeOrientation ro= ARCoreDevice.GetComponent<RelativeOrientation>()
                unityViewpoint.transform.rotation = TrackingUpdate.GyroQuaternion()*Quaternion.Loo(ro.rotOffX,ro.rotOffY,ro.rotOffZ);

                FindObjectOfType<messageController>().message("Gyro:\n"+ TrackingUpdate.GyroQuaternion().ToString(), 4.0f, "gyro");
                FindObjectOfType<messageController>().message("Gyro active:\n" + Input.gyro.enabled, 1.0f, "gyro2");
                */
                break;
        }
		
	}

    private bool checkForWallPass()
    {
        if (unityViewpointPosPrev != Vector3.zero)
        {

            ///check multiple times if our dude moves further than 5cm. lower values ~ more precision and lower risk, but more processing
            float segmentDist = 0.05f;

            Vector3 moveDist = (unityViewpoint.transform.position - unityViewpointPosPrev);

            int segments = Mathf.CeilToInt(moveDist.magnitude / segmentDist);


            for(int n=0; n<segments;n++)
            {
                float progress = (n + 1) / (segments + 1);

                Vector3 from = unityViewpointPosPrev + moveDist * progress;

                if(NavigationController.FindFloor(from)==null)
                {
                    unityViewpointPosPrev = unityViewpoint.transform.position;     
                    return true;
                }
            }
        }
        unityViewpointPosPrev = unityViewpoint.transform.position;
        return false;
    }

    private void trackStats(float time)
    {
        int statSamplePrev = statSampleCurrent;
        statSampleCurrent= (statSampleCurrent+1)% statSampleSize;
        int statSampleOldest = (statSampleCurrent + 1) % statSampleSize;

        statTimeArray[statSampleCurrent] = time;
        float deltaTime = statTimeArray[statSampleCurrent] - statTimeArray[statSampleOldest];

        statProgressArray[statSampleCurrent] = GetComponent<RouteProgressController>().progress;

        //device height
        RaycastHit hit;
        float height = statDeviceHeight;
        int layerMask = 1 << 9;
        if (Physics.Raycast(unityViewpoint.transform.position, Vector3.down, out hit, 2.8f, layerMask, QueryTriggerInteraction.Collide))
        {
            height = unityViewpoint.transform.position.y - hit.point.y;
        }

        statDeviceHeightArray[statSampleCurrent] = height;


        //now crunch those values up!
        statProgress = ((statProgressArray[statSampleCurrent] - statProgressArray[statSampleOldest])) / deltaTime;

        statDeviceHeight=0;
        for(int n=0; n<statSampleSize;n++)
        {
            statDeviceHeight += statDeviceHeightArray[n];
        }
        statDeviceHeight /= statSampleSize;


    }

    public enum TrackingMode
    {
        tracking=0,
        ghosting=1,
    }

    public void trackingGained()
    {

        
        if (trackingMode == TrackingMode.ghosting)
        {
            FindObjectOfType<messageController>().message("Tracking wiederhergestellt.", 4.0f, "ghostingStatus");
            trackingMode =TrackingMode.tracking;

            

            Vector3 desiredPosition = unityViewpoint.transform.position;

            RelativeOrientation r = ARCoreDevice.GetComponent<RelativeOrientation>();
            r.enabled = true;
            r.trackingType = RelativeOrientation.TrackingType.RotationAndPosition;
            r.apply();
            r.apply();


            r.worldOffsetPostion = desiredPosition - unityViewpoint.transform.position;

            Input.gyro.enabled = false;

        }
    }

    /// <summary>
    /// Start ghosting along the route if tracking is lost. restore arcore tracking as soon as available.
    /// </summary>
    public void trackingLost()
    {



        if (routeProgress.route.Length > 0 && trackingMode==TrackingMode.tracking)
        {

            FindObjectOfType<messageController>().message("Ghosting aktiviert...", 8.0f,"ghostingStatus");
            //start ghosting
            trackingMode = TrackingMode.ghosting;

            ghostingProgressStart = routeProgress.progress;
            ghostingProgressSpeed = statProgress;
            ghostingTimeStart = Time.time;

            ARCoreDevice.GetComponent<RelativeOrientation>().trackingType = RelativeOrientation.TrackingType.none;
            ARCoreDevice.GetComponent<RelativeOrientation>().enabled = false;

            Input.gyro.enabled = true;
        }
    }
}
