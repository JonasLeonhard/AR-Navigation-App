using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavigationController : MonoBehaviour
{

    public GameObject user;
    public GameObject building;
    public GameObject visualizer;
    public NavMeshPath path;
    public NavMeshPathStatus pathStatusPrev;
    public int roomTargetId=-1;
    // Use this for initialization
    void Start()
    {
        pathStatusPrev = NavMeshPathStatus.PathInvalid;
    }
	
    // Update is called once per frame
    void Update()
    {
        if (roomTargetId >= 0)
        {
            ScrFloor floor= FindFloor(user.transform.position);
            if(floor!=null)
            if (floor.roomID == roomTargetId)
            {
                
                //destination reached
                GetComponent<MenuController>().destinationReachedMessageShow(building.GetComponent<rooms>().roomNames[roomTargetId]);
                GetComponent<MenuController>().resetNavigation();


                    roomTargetId = -1;
                }
        }

    }

    public bool navigate(int roomTo)
    {
        return navigateNavMesh(user.transform.position, roomTo);
    }

    private bool navigateNavMesh(Vector3 position, int roomTo)
    {


        roomTargetId = roomTo;
        ScrFloor target=null;
        //find a floor gameobject with fitting floor tag
        //Debug.Log("room TO: " + roomTo);
        foreach (ScrFloor floor in building.GetComponentsInChildren<ScrFloor>())
        {
            //Debug.Log("room TO: " + floor.roomID);
            if (floor.roomID==roomTo)
            {
                target = floor;
                Debug.Log("target IS: " + floor);
                break;
            }
        }

        if(target==null)
        {
            return false;
        }
        else
        {
            path = new NavMeshPath();
            NavMeshAgent agent = user.GetComponent<NavMeshAgent>();
            agent.enabled = true;
            agent.CalculatePath(target.gameObject.transform.position, path);

            agent.enabled = false;

            if (path.status==NavMeshPathStatus.PathComplete)
            {
                visualizerActivate(path.corners);
                gameObject.GetComponent<MenuController>().Compass.GetComponent<CompassController>().setTarget(path.corners[0]);
                gameObject.GetComponent<RouteProgressController>().setRoute(path.corners);

                PathAutoplay autoplay = user.GetComponent<PathAutoplay>() ;

                if(autoplay!=null)
                {
                    if(autoplay.enabled)
                    {
                        autoplay.activate(path);
                    }
                }


                return true;
            }
            else
            {
                visualizer.GetComponent<LineRenderer>().SetPositions(null);
                return false;

            }
            

        }

        

        

    }

    public bool navigate(Vector3 posFrom, int roomTo)
    {

        throw new NotImplementedException();
        int floorFrom = 0; /*FindFloors(posFrom);*/
        int vertexFrom = findVertex(floorFrom);

        RaycastHit hit;
        int layerMask = 1 << 9;
        float floorY= posFrom.y;

        if (Physics.Raycast(posFrom, Vector3.down, out hit, 5, layerMask, QueryTriggerInteraction.Collide))
        {
            floorY=hit.transform.position.y;
        }

        posFrom = new Vector3(posFrom.x, floorY, posFrom.z);

        if (vertexFrom >= 0)
        {
            int[] route = findRoute(vertexFrom, roomTo);

            if (route != null)
            {
                Vector3[] routeVec = routeToPositions(route, posFrom);
                visualizerActivate(routeVec);



                gameObject.GetComponent<RouteProgressController>().setRoute(routeVec);

                //set initial target to first waypoint
                gameObject.GetComponent<MenuController>().Compass.GetComponent<CompassController>().setTarget(building.GetComponent<BuildingGraph>().vertices[route[0]].position);
                return true;
            }
        }

        return false;
    }



    int findVertex(int floor)
    {
        BuildingGraph graph = building.GetComponent<BuildingGraph>();

        for (int n = 0; n < graph.vertices.Length; n++)
        {
            BuildingVertex v = graph.vertices[n];

            for (int n2 = 0; n2 < v.floorIDs.Length; n2++)
            {
                if (v.floorIDs[n2] == floor)
                {
                    return n;
                }
            }
        }
        return -1;
    }

    int findRoom(Vector3 posFrom)
    {
        RaycastHit hit;
        int layerMask = 1 << 9;
        if (Physics.Raycast(posFrom, Vector3.down, out hit, 5, layerMask, QueryTriggerInteraction.Collide))
        {
            GameObject gHit = hit.collider.gameObject;
            ScrFloor floor = gHit.GetComponent<ScrFloor>();
            rooms r = building.GetComponent<rooms>();
            Debug.Log(r.ToString());
            Debug.Log(floor.ToString());

            Debug.Log("Object is in Room: " + r.roomNames[floor.roomID]);
            return floor.roomID;
        }
        return -1;

            
    }

    public static ScrFloor FindFloor(Vector3 posFrom)
    {
        RaycastHit hit;
        int layerMask = 1 << 9;
        if (Physics.Raycast(posFrom, Vector3.down, out hit, 2.8f, layerMask, QueryTriggerInteraction.Collide))
        {
            GameObject gHit = hit.collider.gameObject;
            ScrFloor floor = gHit.GetComponent<ScrFloor>();
            return floor;
        }
        return null;


    }

    int[] findRoute(int roomFrom, int roomTo)
    {
        BuildingGraph graph = building.GetComponent<BuildingGraph>();
        int[] vertShortestAccess = new int[graph.vertices.Length];
        bool[] vertPotentiallyViable= new bool[graph.vertices.Length]; //is set to false when all connecting verts return an unsuccessful path
        for (int n = graph.vertices.Length - 1; n >= 0; n--)
        {
            vertShortestAccess[n] = int.MaxValue; //Still calculated in Vertex-passes, not actual physical distance
            vertPotentiallyViable[n]=true;
        }

        List<int> l = new List<int>();
        l.Add(roomFrom);

        List<int> routeList = routeRun(graph, l, roomTo, vertShortestAccess,vertPotentiallyViable);
        string s = arrayToString(routeList.ToArray());
        Debug.Log("ROute is: " + s);

        if (routeList != null)
        {
            int[] routeListArray = routeList.ToArray();
            return routeListArray;
        }
        else
            return null;
    }

    List<int> routeRun(BuildingGraph graph, List<int> route, int roomTo, int[] vertShortestAccess,bool[] viability)
    {
        
        BuildingVertex v = graph.vertices[route[route.Count - 1]];
        vertShortestAccess[v.id] = route.Count;
        for (int n = 0; n < v.floorIDs.Length; n++)
        {
            if (graph.floorsRoomIds[v.floorIDs[n]] == roomTo) //If in target room
            {
                return route;
            }                   
        }

        List<int>[] lists = new List<int>[v.accessibleVertices.Length];

        for (int n = 0; n < v.accessibleVertices.Length; n++)
        {
            if (vertShortestAccess[v.accessibleVertices[n]] > route.Count - 1 && viability[v.accessibleVertices[n]])
            {
                List<int> copy = listCopy(route);
                copy.Add(v.accessibleVertices[n]);
                lists[n] = routeRun(graph, copy, roomTo, vertShortestAccess,viability);
            }
            else
            {
                lists[n] = null;
            }
        }

        int shortestLength = int.MaxValue;
        int shortestIndex = -1;

        for (int n = 0; n < lists.Length; n++)
        {
            List<int> rCurrent = lists[n];
            if (rCurrent != null)
            {
                if (rCurrent.Count < shortestLength)
                {
                    shortestIndex = n;
                    shortestLength = rCurrent.Count;
                }
            }
        }
        if (shortestIndex >= 0)
            return lists[shortestIndex];
        else
        {
            viability[v.id] = false; //DONT GO HERE EVER AGAIN!
            return null;
        }
        
    }

    List<int> listCopy(List<int> l)
    {
        List<int> ret = new List<int>();
        for (int n = 0; n < l.Count; n++)
        {
            ret.Add(l[n]);
        }

        return ret;
    }

    string arrayToString(int[] arr1)
    {
        string[] arr2 = new string[arr1.Length];

        for (int n = 0; n < arr2.Length; n++)
        {
            arr2[n] = arr1[n].ToString();
        }

        return string.Join(" - ", arr2);
    }

    void visualizerActivate(Vector3[] route)
    {
        ///Elevation of the route
        float pathElevation = 0.1f;
        Vector3[] routeElevated = new Vector3[route.Length];

        for(int n=0; n<route.Length;n++)
        {
            routeElevated[n] = new Vector3(route[n].x, route[n].y + pathElevation, route[n].z);
        }

        visualizer.SetActive(true);
        LineRenderer lr = visualizer.GetComponent<LineRenderer>();
        lr.positionCount = route.Length;
        lr.SetPositions(routeElevated);
    }

    Vector3[] routeToPositions(int[] route, Vector3 start)
    {
        Vector3[] positions=new Vector3[route.Length+1];
        positions[0] = start;

        BuildingGraph graph = building.GetComponent<BuildingGraph>();


        for (int n = 0; n < route.Length; n++)
        {
            positions[n + 1] = graph.vertices[route[n]].position;
        }

        return positions;
    }

    public void navigationTry(int roomId)
    {
        bool success = navigate(roomId);

        if(!success)
        {
            GetComponent<MenuController>().resetNavigation();
        }

        MenuController m = gameObject.GetComponent<MenuController>();
        m.navMessageShow(GameObject.Find("Building").GetComponent<rooms>().roomNames[roomId], success);


    }

    public string roomGetName(int roomId)
    {
        if (roomId >= 0)
            return building.GetComponent<rooms>().roomNames[roomId];
        else
            return null;
    }
}
