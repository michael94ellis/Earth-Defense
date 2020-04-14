using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Earth : MonoBehaviour
{
    public List<GameObject> cities = new List<GameObject>();
    public Object cityRef;
    public Vector3 origin = new Vector3(0, 0, 0);
    public int count = 0;
    public bool isPaused = false;

    void Awake()
    {
        cityRef = Resources.Load("City");
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    void Update()
    {

    }

    void OnMouseUp()
    {
        if (isPaused)
        {
            ContinueGame();
        }
        else
        {
            PauseGame();
        }
        // If your mouse hovers over the GameObject with the script attached, output this message
        Vector3 clickedPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray))
        {
        }
    }


    private void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0;
        //Disable scripts that still work while timescale is set to 0
    }
    private void ContinueGame()
    {
        isPaused = false;
        Time.timeScale = 1;
        //enable the scripts again
    }
}