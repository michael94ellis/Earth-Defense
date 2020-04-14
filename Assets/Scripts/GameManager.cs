using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public static List<GameObject> Cities { get; private set; }
    public static List<GameObject> Aliens { get; private set; }

    void initGameManager()
    {
        if (Instance == null)
        {
            Debug.Log("Instantiating GameManager");
            GameObject go = new GameObject();
            Instance = go.AddComponent<GameManager>();
            go.name = "StaticGameManager";
        }
    }

    void Awake()
    {
        Debug.Log("Game Manager Awakened");
        initGameManager();
        DontDestroyOnLoad(this);
    }


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Game Manager Started");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
