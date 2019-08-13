using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SpatialTracking;

/// <summary>
/// Autoplays the NavMeshPath for demo purposes.
/// </summary>
public class PathAutoplay : MonoBehaviour
{

    public NavMeshPath path;
    public float starttimer;
    float starttimerCurrent;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (starttimerCurrent > 0 && starttimerCurrent > -100)
        {
            starttimerCurrent -= Time.deltaTime;

            if (starttimerCurrent <= 0)
            {
                starttimerCurrent = -300;



                NavMeshAgent ago = GetComponent<NavMeshAgent>();
                ago.enabled = true;
                ago.updateRotation = false;
                ago.SetPath(path);
                ago.baseOffset = 1.5f;

                transform.parent.GetComponentInChildren<TrackedPoseDriver>().trackingType = TrackedPoseDriver.TrackingType.RotationOnly;
                //transform.parent.GetComponentInChildren<TrackedPoseDriver>().enabled = false;
                GameObject.FindObjectOfType<RelativeOrientation>().trackingType = RelativeOrientation.TrackingType.RotationOnly;

            }

        }
    }

    public void activate(NavMeshPath p)
    {
        
            starttimerCurrent = starttimer;
            path = p;
        
    }
}
