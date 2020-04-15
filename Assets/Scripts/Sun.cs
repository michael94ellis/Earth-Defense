using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : MonoBehaviour
{
    public float XWobble = 0f;
    public float YWobble = 0f;
    public float ZWobble = 0f;
    public bool increaseX = true;
    public bool increaseY = true;
    public bool increaseZ = true;
    public float maxXWobble = 18f;
    public float maxYWobble = 13f;
    public float maxZWobble = 14;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float x, y, z;
        if (increaseX)
        {
            if (XWobble >= maxXWobble)
            {
                increaseX = false;
            }
            XWobble++;
        }
        else
        {

            if (XWobble <= 0)
            {
                increaseX = true;
            }
            XWobble--;
        }
        if (increaseY)
        {
            if (YWobble >= maxYWobble)
            {
                increaseY = false;
            }
            YWobble++;
        }
        else
        {

            if (YWobble <= 0)
            {
                increaseY = true;
            }
            YWobble--;
        }
        if (increaseZ)
        {
            if (ZWobble >= maxZWobble)
            {
                increaseZ = false;
            }
            ZWobble++;
        }
        else
        {

            if (ZWobble <= 0)
            {
                increaseZ = true;
            }
            ZWobble--;
        }
        x = XWobble * Time.deltaTime * 0.25f;
        y = YWobble * Time.deltaTime * 0.25f;
        z = ZWobble * Time.deltaTime * 0.25f;
        // Orbit the Sun
        transform.Rotate(x, y, z);
    }
}
