using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Earth : MonoBehaviour
{
    public List<GameObject> Cities = new List<GameObject>();
    public Object cityRef;
    public Vector3 origin = new Vector3(0, 0, 0);
    public int GlobalCurrency = 0;
    public bool isPaused = false;
    GameObject Sun;
    public Vector3 axis = new Vector3(0, 0, 0);
    public float rotationSpeed = 80.0f;

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

    void UpdateCitiesList()
    {
        Cities = new List<GameObject>();
        // Add the city's buildings to the list
        foreach (Transform child in transform)
        {
            if (child.tag == "City")
            {
                Cities.Add(child.gameObject);
            }
        }
    }

    void OnMouseUp()
    {
        if (!isPaused)
        {
            PauseGame();
        }
    }

    void OnGUI()
    {
        if (isPaused)
        {
            GUI.Window(0, new Rect((Screen.width / 2) - 150, (Screen.height / 2) - 75, 300, 250), ShowGUI, "Title Example");

        }
    }

    void ShowGUI(int windowID)
    {
        // You may put a label to show a message to the player

        GUI.Label(new Rect(65, 40, 200, 30), "PUT YOUR MESSAGE HERE");

        // You may put a button to close the pop up too

        if (GUI.Button(new Rect(50, 150, 75, 30), "OK"))
        {
            isPaused = false;
            ContinueGame();
            // you may put other code to run according to your game too
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