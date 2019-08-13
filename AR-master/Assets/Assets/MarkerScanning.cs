
namespace GoogleARCore.Examples.AugmentedImage
{
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using GoogleARCore;

    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.Events;

    public class MarkerScanning : MonoBehaviour
    {

        public GameObject SourceObject;
        public Vector3 PositionOffset;
        public Quaternion AngleOffset;
        public GameObject TargetObject;

        public AugmentedImageVisualizer AugmentedImageVisualizerPrefab;
        public GameObject FitToScanOverlay;

        private Dictionary<int, int> imTrackStatus
            = new Dictionary<int, int>();
        private List<AugmentedImage> m_TempAugmentedImages = new List<AugmentedImage>();

        public UnityEvent realityPositionSet;
        public bool isTracking;

        Anchor anchor;

        public MenuController controller;

        // Use this for initialization
        void Start()
        {
            AngleOffset = Quaternion.LookRotation(Vector3.forward);
            realityPositionSet = new UnityEvent();
            isTracking = false;
        }

        /// <summary>
        /// Controller for AugmentedImage example.
        /// </summary>
        /// <summary>
        /// A prefab for visualizing an AugmentedImage.
        /// </summary>




        /// <summary>
        /// The Unity Update method.
        /// </summary>
        public void Update()
        {

            // Check that motion tracking is tracking.
            if (Session.Status != SessionStatus.Tracking)
            {
                isTracking = true;
                return;
            }
            else
            {
                isTracking = false;
            }

            // Get updated augmented images for this frame.
            Session.GetTrackables<AugmentedImage>(m_TempAugmentedImages, TrackableQueryFilter.Updated);

            // Create visualizers and anchors for updated augmented images that are tracking and do not previously
            // have a visualizer. Remove visualizers for stopped images.
            foreach (var image in m_TempAugmentedImages)
            {
                int status = 0;
                imTrackStatus.TryGetValue(image.DatabaseIndex, out status);
                if (image.TrackingState == TrackingState.Tracking && status == 0)
                {
                    // Create an anchor to ensure that ARCore keeps tracking this augmented image.
                    // TODO: Is this anchor automatically removed when no longer in use?

                    anchor = image.CreateAnchor(image.CenterPose);


                    //Find associated Markerobject in Gameworld

                    foreach (scrMarker marker in FindObjectsOfType<scrMarker>())
                    {
                        if (marker.augmentedImageDbIndex == image.DatabaseIndex)
                        {
                            RelativeOrientation r = gameObject.GetComponent<RelativeOrientation>();
                            r.AnchorSource = anchor.gameObject;
                            r.AnchorTarget = marker.gameObject;


                            if (controller.gameObject.GetComponent<RouteProgressController>().route.Length == 0)
                                controller.setMenuPreset(1);

                            GetComponent<RelativeOrientation>().worldOffsetPostion = Vector3.zero;
                            FindObjectOfType<messageController>().message("Marker gescannt.", 3.7f, "markerScanned");

                            break;
                        }
                    }




                    imTrackStatus.Add(image.DatabaseIndex, 1);
                }
                else
                {
                    if (image.TrackingState == TrackingState.Tracking)
                    {
                        foreach (scrMarker marker in FindObjectsOfType<scrMarker>())
                        {
                            if (marker.augmentedImageDbIndex == image.DatabaseIndex)
                            {
                                RelativeOrientation r = gameObject.GetComponent<RelativeOrientation>();
                                r.AnchorSource = anchor.gameObject;
                                r.AnchorTarget = marker.gameObject;


                                if(controller.gameObject.GetComponent<RouteProgressController>().route.Length==0)
                                controller.setMenuPreset(1);

                                GetComponent<RelativeOrientation>().worldOffsetPostion = Vector3.zero;
                                FindObjectOfType<messageController>().message("Marker gescannt.", 3.7f, "markerScanned");

                                break;
                            }
                        }
                    }
                    else if (image.TrackingState == TrackingState.Stopped && status != 0)
                    {
                        imTrackStatus.Remove(image.DatabaseIndex);

                    }
                    }
                }
            }

        }

    }


