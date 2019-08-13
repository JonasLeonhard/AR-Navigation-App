using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SearchListController : MonoBehaviour {

    public rooms rooms;
    public GameObject pfButton;
    private List<GameObject> buttons;
    public GameObject Container;
    public GameObject menuController;
    public int maxDisplay;

    // Use this for initialization
    void Awake () {
        buttons = new List<GameObject>();
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void inputUpdated(string str)
    {
        Debug.Log("UPDATED");
        List<int> roomIds = new List<int>();

        for (int n = 0; n < rooms.roomNames.Length; n++)
        {
            if (str.Length == 0 || rooms.roomNames[n].ToLower().Contains(str.ToLower()))
            {
                roomIds.Add(n);
            }
        }

        Debug.Log(roomIds.Count);

        for (int n = 0; n < Mathf.Min(maxDisplay,roomIds.Count); n++)
        {
            if (n < buttons.Count)
            {
                GameObject b = buttons[n];
                ButtonRoomSelect bt = b.GetComponent<ButtonRoomSelect>();
                bt.setRoom(rooms, roomIds[n]);
            }
            else
            {
                GameObject bt=GameObject.Instantiate(pfButton,Container.transform);

               

                ButtonRoomSelect rs =bt.GetComponent<ButtonRoomSelect>();
                rs.setRoom(rooms, roomIds[n]);
                rs.menuController = menuController;


                buttons.Add(bt);
            }
        }

        for (int n = roomIds.Count; n < buttons.Count; n=n)
        {
            GameObject b = buttons[n];
            buttons.Remove(b);
            GameObject.Destroy(b);
        }




        roomIds.Clear();
    }


}
