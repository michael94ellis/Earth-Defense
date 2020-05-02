using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClickMenu : MonoBehaviour
{
    // Start is called before the first frame update
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {

        
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //possible third param for max distance
        if(Physics.Raycast(ray, out hit))
        {
            if (hit.transform != null)
            {
                PrintInfo(hit.transform.gameObject);
            }
        }
    }
    }
    private void PrintInfo (GameObject clicked)
    {
        print(clicked.name);
    }
}
