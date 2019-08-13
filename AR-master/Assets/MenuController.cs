using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{

    public GameObject FitToScanOverlay;
    public GameObject TargetSelectMenu;
    public GameObject NavigationStartedMenu;
    public GameObject DestinationReachedMenu;
    //public GameObject MenuButton;
    public CompassController Compass;

    public int state;


    float lastBackpress;

    // Use this for initialization
    void Start()
    {
        lastBackpress = 0;
    }
	
    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.Escape))
        {
            if (Time.time - lastBackpress > 1.2f)
            {
                lastBackpress = Time.time;
                switch (state)
                {

                    case 0:
                    case 2:
                        Application.Quit();
                        break;

                    default:
                        setMenuPreset(2);
                        break;
                }
            }
        }
    }

    public void resetNavigation()
    {
        Compass.setState(1);

        

        RouteProgressController rp = GetComponent<RouteProgressController>();
        rp.progress = 0;
        rp.route = new Vector3[0];
        rp.routeLength = 0;
        

        NavigationController n = GetComponent<NavigationController>();
        n.visualizer.SetActive(false);
        n.roomTargetId = -1;
      

    }


    /// <summary>
    /// Sets the whole GUI to one of numerous presets.
    /// Just the way good ol' grandma always used to.
    /// </summary>
    /// <param name="menuId">Menu Preset identifier.</param>
    public void setMenuPreset(int menuId)
    {
        if (state != menuId)
        {
            state = menuId;
            switch (menuId)
            {
                //Scanoverlay only
                default:
                case 0:
                    scanOverlayToggle(true);
                    targetSelectMenuToggle(false);
                    //menuButtonToggle(false);
                    compassToggle(false);
                    break;

                //Target select menu
                case 1:
                    scanOverlayToggle(false);
                    targetSelectMenuToggle(true);
                    //menuButtonToggle(false);
                    compassToggle(false);
                    break;

                //Normal navigation gui
                case 2:
                    scanOverlayToggle(false);
                    targetSelectMenuToggle(false);
                    //menuButtonToggle(true);
                    compassToggle(true);
                    break;

            }
        }
    }

    /// <summary>
    /// Toggles Scan-Overlay on or off.
    /// </summary>
    /// <param name="active">If set to <c>true</c> active.</param>
    public void scanOverlayToggle(bool active)
    {
        FitToScanOverlay.SetActive(active);
    }

    /// <summary>
    /// Toggles the Button to open up the target select menu on or off.
    /// </summary>
    /// <param name="active">If set to <c>true</c> active.</param>
    public void menuButtonToggle(bool active)
    {
        //MenuButton.SetActive(active);
    }

    /// <summary>
    /// Toggles Compass on or off.
    /// I wont blaim you for keeping it enabled all the way, but, you know; sometimes one loves themself some adventure!
    /// </summary>
    /// <param name="active">If set to <c>true</c> active.</param>
    public void compassToggle(bool active)
    {
        Compass.gameObject.SetActive(active);

        if(GetComponent<NavigationController>().roomTargetId>=0)
        {
            Compass.setState(0);

        }
        else
        {
            Compass.setState(1);

        }
    }

    /// <summary>
    /// Toggles the target select menu on or off.
    /// That's a hefty fella'. All the functionality, but all the screen width aswell.
    /// </summary>
    /// <param name="active">If set to <c>true</c> active.</param>
    public void targetSelectMenuToggle(bool active)
    {
        TargetSelectMenu.SetActive(active);
        if (active)
        {
            TargetSelectMenu.GetComponent<SearchController>().reset();

            string s = GetComponent<NavigationController>().roomGetName(GetComponent<NavigationController>().roomTargetId);

            TargetSelectMenu.GetComponent<SearchController>().scrollView.GetComponent<ScrollViewOffsetTop>().setState(s);

        }
    }

    /// <summary>
    /// Displays a message that the navigation has been started. So they know. Once they started it. That they started it. The Navigation.. Once it's going, there's no holding back no more..
    /// </summary>
    /// <param name="room">Room Name.</param>
    public void navMessageShow(string room,bool success)
    {
        //GameObject g = GameObject.Instantiate(NavigationStartedMenu, GameObject.Find("Canvas").transform);
        //MenuTargetSelectedController c = g.GetComponent<MenuTargetSelectedController>();
        //c.set(room,success);
        targetSelectMenuToggle(false);

        if (success)
        FindObjectOfType<messageController>().message("Navigation zu Raum \n" + room + " gestartet.",4.2f);
        else
            FindObjectOfType<messageController>().message("Navigation zu Raum \n" + room + " konnte nicht gestartet werden.", 4.2f);

    }

    public void destinationReachedMessageShow(string room)
    {
        FindObjectOfType<messageController>().message("Du hast dein Ziel\n"+ room+" erreicht.", 4.2f);

    }
}
