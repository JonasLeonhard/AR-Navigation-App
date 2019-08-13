using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrFloor : MonoBehaviour {

    public GameObject pfMarking;
    public int floorID;
    public int roomID;
    public BuildingGraph building=null;

	// Use this for initialization
	void Start () {
        if (building == null)
        {
            building = GameObject.Find("Building").GetComponent<BuildingGraph>();
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider collider)
    {
        Vector3 pos=collider.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
        //GameObject o = GameObject.Instantiate(pfMarking, pos, Quaternion.identity);
        //gameObject.GetComponent<Collider>().enabled = false;

        int floorIDother = collider.gameObject.GetComponent<ScrFloor>().floorID;

        building.addVertex(new int[] {floorID,floorIDother},pos);

    }
}
