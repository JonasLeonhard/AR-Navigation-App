using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonRoomSelect : MonoBehaviour {

    public int roomId;
    public GameObject menuController;
	// Use this for initialization
	void Start () {
        Button btn = gameObject.GetComponent<Button>();
        btn.onClick.AddListener(clicked);
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void clicked()
    {
        //AWFULL!!!!!!!!     CHANGE!!!
        menuController.GetComponent<SearchController>().mainController.GetComponent<NavigationController>().navigationTry(roomId);
        menuController.GetComponent<SearchController>().mainController.GetComponent<MenuController>().setMenuPreset(2);
    }

    public void setRoom(rooms rooms,int roomId)
    {
        this.roomId = roomId;

        gameObject.GetComponentInChildren<Text>().text=rooms.roomNames[roomId];
    }
}
