using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWobble : MonoBehaviour {

    public float targetScale;
    public float wobbleMax;
    public float force;
    public float friction;

    float scale;
    float speed;

    bool active;

    RectTransform rt;

    // Use this for initialization
    void Start () {
        rt = GetComponent<RectTransform>();
        active = true;

    }
	
	// Update is called once per frame
	void FixedUpdate () {

        if (active)
        {
            speed += (targetScale - scale) * force;
            speed *= (1 - friction);

            scale += speed;
            Debug.Log("SCALE " + scale);

            if (restable())
            {
                active = false;
                speed = 0;
                scale = targetScale;
            }

            rt.localScale = new Vector3(scale, scale, scale);


        }

	}

    bool restable()
    {
        return Mathf.Abs(scale - targetScale) < 0.004f && Mathf.Abs(speed) < 0.002f;
    }

    public void wobble()
    {
        scale = wobbleMax;
        active = true;
    }
}
