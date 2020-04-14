using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        if (!isPaused)
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