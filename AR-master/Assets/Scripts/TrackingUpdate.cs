using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;
using UnityEngine.Events;

public class TrackingUpdate : MonoBehaviour {

    public GameObject realityViewpoint;

    public RelativeOrientation relativeOrientation;
    Quaternion lastTrackstate;
    Quaternion gyro, arcore;
    

    bool hasTracked = false;
    enum state {tracking, nontracking};

    public UnityEvent trackingGained;
    public UnityEvent trackingLost;

    // Use this for initialization
    void Start () {


    }
	
	// Update is called once per frame
	void Update () {
        
        if (!hasTracked)
        {
            if (Session.Status == SessionStatus.Tracking)
            {
                hasTracked = true;
                trackingGained.Invoke();
            }
        }
        else if (Session.Status != SessionStatus.Tracking)
        {
            Debug.Log("not tracking!");
            hasTracked = false;
            trackingLost.Invoke();
        };
	}




    public static Quaternion GyroQuaternion()
    {
        return GyroToUnity(Input.gyro.attitude);
    }

    private static Quaternion GyroToUnity(Quaternion q)
    {
        //convert to unity left-handed sytem:
        return new Quaternion(q.x, q.y, -q.z, -q.w);
    }

}
