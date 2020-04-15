using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Earth : MonoBehaviour
{
    public List<GameObject> Cities = new List<GameObject>();
    public Object cityRef;
    public Vector3 origin = new Vector3(0, 0, 0);
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
        UpdateCitiesList();
    }

    void Update()
    {
        Cities.RemoveAll(item => item == null);
    }

    void UpdateCitiesList()
    {
        Cities = new List<GameObject>();
        // Add the city's buildings to the list
        foreach (Transform child in transform)
        {
            Debug.Log(child.tag + " !!!!!");
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
            int width = 500;
            int height = 500;
            int x = (Screen.width / 2) - (width / 2);
            int y = (Screen.height / 2) - (height / 2);
            GUI.Window(0, new Rect(x, y, width, height), ShowGUI, "Earth Defense Shop");

        }
    }

    void ShowGUI(int windowID)
    {
        // You may put a label to show a message to the player
        UpdateCitiesList();
        Debug.Log(Cities.Count);
        int x = 65, y = 40;
        for (int index = 0; index < Cities.Count; index++)
        {
            List<GameObject> LaserTurrets = new List<GameObject>();
            List<GameObject> Buildings = new List<GameObject>();
            foreach (Transform child in Cities[index].transform)
            {
                switch (child.tag)
                {
                    case "Turret":
                        LaserTurrets.Add(child.gameObject);
                        break;
                    case "Building":
                        Buildings.Add(child.gameObject);
                        break;
                }
            }
            GUI.Label(new Rect(x, y, 150, 30), "City " + (index + 1) + "");
            if (GUI.Button(new Rect(x + 175, y, 100, 30), "Add Turret"))
            {

            }
            y += 30 + 10;
            GUI.Label(new Rect(x, y, x + 10, 30), "  Turrets: " + LaserTurrets.Count);
            foreach(GameObject turret in LaserTurrets)
            {
                y += 30 + 10;
                GUI.Label(new Rect(65, y, 300, 30), "  Turret: " + turret.transform.localPosition);
            }

        }
        // You may put a button to close the pop up too

        y += 30 + 10;
        if (GUI.Button(new Rect(x, y, 75, 30), "OK"))
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