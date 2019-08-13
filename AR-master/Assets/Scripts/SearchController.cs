using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SearchController : MonoBehaviour {

    public GameObject searchField;
    public GameObject scrollView;
    public GameObject mainController;

    public string roomTarget;

	// Use this for initialization
	void Start () {
        searchField.GetComponent<InputField>().onValueChanged.AddListener(scrollView.GetComponent<SearchListController>().inputUpdated);

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void reset()
    {
        Debug.Log("RESET");
        searchField.GetComponent<InputField>().text = "";
        scrollView.GetComponent<SearchListController>().inputUpdated("");
    }
}
