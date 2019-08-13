using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRendererTexScroll : MonoBehaviour {

    public LineRenderer lineRenderer;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        Material m=lineRenderer.material;
        Vector2 vecTmp = m.GetTextureOffset("_MainTex");
        Vector2 vec = new Vector2(vecTmp.x - 0.01f, vecTmp.y);
        m.SetTextureOffset("_MainTex", vec);
	}
}
