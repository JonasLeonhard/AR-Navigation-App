using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGraph : MonoBehaviour {

    private List<BuildingVertex> verticesList;
    public int[] floorsRoomIds;
    public BuildingVertex[] vertices;
    private int continueState=0;
    public GameObject pfMarking;

	// Use this for initialization
	void Start () {
        buildingInit1();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        switch (continueState)
        {
            default:
                break;
            case 1:
                continueState = 0;
                buildingInit2();
                break;
            case 2:
                continueState = 1;

                break;
        }
	}

    void buildingInit1()
    {
        verticesList = new List<BuildingVertex>();
        continueState = 0;
        floorIDsgenerate();
        verticesGenerate();
    }

    void buildingInit2()
    {
        //Now lots of vertices should have been generated maybe
        //floorCollidersDisable();
        vertices = verticesProcess(verticesList);
        verticesLink(vertices);

    }

    public void addVertex(int[] floorIDs,Vector3 position) //usually a vertex is within exactly two floors.
    {
        verticesList.Add(new BuildingVertex(floorIDs, position));
    }

    public void floorIDsgenerate()
    {
        GameObject floors = GameObject.Find("Floors");
        int floorsAmount = floors.transform.childCount;
        floorsRoomIds = new int[floorsAmount];
        for (int n = 0; n < floorsAmount; n++)
        {
            ScrFloor f = floors.transform.GetChild(n).gameObject.GetComponent<ScrFloor>();
            if (f != null)
            {
                f.floorID = n;
                f.building = this;
                floorsRoomIds[n] = f.roomID;
            }
        }
    }

    public void verticesGenerate()
    {
        GameObject floors = GameObject.Find("Floors");
        int floorsAmount = floors.transform.childCount;
        for (int n = 0; n < floorsAmount; n++)
        {
            GameObject g = floors.transform.GetChild(n).gameObject;
            g.GetComponent<BoxCollider>().enabled = true;

        }

        //What comes next is a bit stupid: We have to wait for Collisions to play out, and THEN continue with the Graph Generation...
        continueState=2;
    }

    void floorCollidersDisable()
    {
        GameObject floors = GameObject.Find("Floors");
        int floorsAmount = floors.transform.childCount;
        for (int n = 0; n < floorsAmount; n++)
        {
            GameObject g = floors.transform.GetChild(n).gameObject;
            g.GetComponent<BoxCollider>().enabled = false;


        }
    }

    BuildingVertex[] verticesProcess(List<BuildingVertex> vertsList)
    {
        Debug.Log("YOWZA, size of list: "+vertsList.Count.ToString());
        List<BuildingVertex> vNew = new List<BuildingVertex>();

        for (int n = 0; n < vertsList.Count; n++)
        {
            BuildingVertex v = vertsList[n];
            BuildingVertex opp = null;
            //find the vertex's opposite "partner"
            if (v != null)
            {
                for (var n2= 0; n2 < vertsList.Count; n2++)
                {
                    BuildingVertex v2 = vertsList[n2];
                    if (v2 != null && n!=n2)
                    {
                        
                        if (v.floorIDs[0] == v2.floorIDs[1] && v.floorIDs[1] == v2.floorIDs[0])
                        {
                            opp = v2;
                            vNew.Add(new BuildingVertex(v.floorIDs,(v.position+v2.position)*0.5f));
                            int index = vNew.Count - 1;



                            vertsList[n2] = null;
                           
                        }
                    }
                }
            }
        }

        return vNew.ToArray();

    }

    void verticesLink(BuildingVertex[] verts)
    {
        List<int> linksList;
        for (int n1 = 0; n1 < verts.Length; n1++)
        {
            
            BuildingVertex v1 = verts[n1];
            v1.id = n1;
            linksList = new List<int>();

            for (int n2 = 0; n2 < verts.Length; n2++)
            {
                if (n1 != n2)
                {
                    BuildingVertex v2 = verts[n2];  
                    if (v2.floorIDs[0] == v1.floorIDs[0] || v2.floorIDs[1] == v1.floorIDs[0] || v2.floorIDs[0] == v1.floorIDs[1] || v2.floorIDs[1] == v1.floorIDs[1])
                    {
                        linksList.Add(n2);
                    }
                    }
            }
            Debug.Log("Found Connections: " + linksList.Count);
            v1.accessibleVertices = linksList.ToArray();
            linksList.Clear();

            /*GameObject o = GameObject.Instantiate(pfMarking, v1.position, Quaternion.identity);
            o.GetComponent<DebugInfo>().integer = n1;
            o.GetComponent<DebugInfo>().listOfInts = v1.accessibleVertices;*/
        }
    }


}

public class BuildingVertex
{
    public int[] floorIDs;
    public Vector3 position;
    public int[] accessibleVertices; //vertices this vertex has direct connection to.
    public int id;

    public BuildingVertex(int[] f,Vector3 p)
    {
        floorIDs = f;
        position = p;
        accessibleVertices = new int[] { };
    }
        
}


