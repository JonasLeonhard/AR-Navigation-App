using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelativeOrientation : MonoBehaviour
{

    public GameObject AnchorSource;
    public GameObject MoverSource;
    public GameObject AnchorTarget;
    public GameObject MoverTarget;
    Quaternion deltaQuaternion;


    public int rotOffX;
    public int rotOffY;
    public int rotOffZ;

    public Vector3 worldOffsetPostion;

    public TrackingType trackingType;
    // Use this for initialization
    void Start()
    {
        //!!!!! Benutzt in Update- einrechnen!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        deltaQuaternion = Quaternion.identity;
    }

    public void OffXChanged(float n)
    {
        rotOffX = (int)Mathf.Round(n);
    }
    public void OffYChanged(float n)
    {
        rotOffY = (int)Mathf.Round(n);
    }
    public void OffZChanged(float n)
    {
        rotOffZ = (int)Mathf.Round(n);
    }


    // Update is called once per framez
    void Update()
    {
        apply();
    }

    public void apply()
    {
        if (AnchorSource != null && AnchorTarget != null)
        {

            Quaternion sourceRot = AnchorSource.transform.rotation;
            Vector3 sourceEuler = sourceRot.eulerAngles;
            Vector3 eulerSnapped = new Vector3(Mathf.Round(sourceEuler.x / 90f) * 90f, sourceEuler.y, sourceEuler.z);
            sourceRot = Quaternion.Euler(eulerSnapped);

            Quaternion qDelta = Quaternion.Inverse(sourceRot) * Quaternion.LookRotation(AnchorSource.transform.position - MoverSource.transform.position, Vector3.down);
            Quaternion qFromUMarker = Quaternion.Euler(180f, 0f, 0f) * AnchorTarget.transform.rotation * qDelta;


            Quaternion rotationDelta = Quaternion.Inverse(sourceRot) * MoverSource.transform.rotation;

            switch (trackingType)
            {
                case TrackingType.RotationAndPosition:
                    MoverTarget.transform.position = AnchorTarget.transform.position + (qFromUMarker * Vector3.back * Vector3.Distance(AnchorSource.transform.position, MoverSource.transform.position)) + worldOffsetPostion;
                    MoverTarget.transform.SetPositionAndRotation(MoverTarget.transform.position, Quaternion.Euler(180f, 0f, 0f) * AnchorTarget.transform.rotation * (rotationDelta));
                    break;

                case TrackingType.PositionOnly:
                    MoverTarget.transform.position = AnchorTarget.transform.position + (qFromUMarker * Vector3.back * Vector3.Distance(AnchorSource.transform.position, MoverSource.transform.position)) + worldOffsetPostion;

                    break;

                case TrackingType.RotationOnly:
                    MoverTarget.transform.rotation = Quaternion.Euler(180f, 0f, 0f) * AnchorTarget.transform.rotation * (rotationDelta);
                    break;
            }

        }
    }

    //
    // Zusammenfassung:
    //     This enum is used to indiciate which parts of the pose will be applied to the
    //     parent transform
    public enum TrackingType
    {
        //
        // Zusammenfassung:
        //     With this setting, both the pose's rotation and position will be applied to the
        //     parent transform
        RotationAndPosition = 0,
        //
        // Zusammenfassung:
        //     With this setting, only the pose's rotation will be applied to the parent transform
        RotationOnly = 1,
        //
        // Zusammenfassung:
        //     With this setting, only the pose's position will be applied to the parent transform
        PositionOnly = 2,

        /// <summary>
        /// No tracking
        /// </summary>
        none=-1
    }
}
