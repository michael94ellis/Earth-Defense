using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class is instantiated by the Earth class
// It holds important vars for the menu separate from the earth object
public class MenuManager
{
    public bool Paused;
    public bool isPickingLocation;
    public int windowWidth = Screen.width * 4 / 6;
    public int windowHeight = Screen.height * 4 / 6;
    public int windowOriginX = Screen.width / 6;
    public int windowOriginY = Screen.height / 6;
    // This holds an item that was just purchased so it can be placed on the earth
    public Object NewObject;

    public enum MenuScreen
    {
        NewGame,
        MainMenu,
        GameOver
    }
    public MenuScreen CurrentScreen;

    /// MARK: - Pause/Resume

    public void Pause()
    {
        Paused = true;
        Time.timeScale = 0;
        //Disable scripts that still work while timescale is set to 0
    }
    public void Resume()
    {
        Paused = false;
        Time.timeScale = 1;
        //enable the scripts again
    }


    /// MARK: - UI Elements

    private GUIStyle _HeaderStyle;
    public GUIStyle HeaderStyle
    {
        get
        {
            if (_HeaderStyle == null)
            {
                GUIStyle newStyle = new GUIStyle
                {
                    alignment = TextAnchor.MiddleCenter,
                    fontSize = 24
                };
                newStyle.normal.textColor = Color.white;
                _HeaderStyle = newStyle;
            }
            return _HeaderStyle;
        }
    }
    private GUIStyle _Header2Style;
    public GUIStyle Header2Style
    {
        get
        {
            if (_Header2Style == null)
            {
                GUIStyle newStyle = new GUIStyle
                {
                    alignment = TextAnchor.MiddleCenter,
                    fontSize = 22
                };
                newStyle.normal.textColor = Color.white;
                _Header2Style = newStyle;
            }
            return _Header2Style;
        }
    }

    private GUIStyle _BodyStyle;
    public GUIStyle BodyStyle
    {
        get
        {
            if (_BodyStyle == null)
            {
                GUIStyle newStyle = new GUIStyle
                {
                    alignment = TextAnchor.MiddleLeft,
                    fontSize = 18,
                    fixedWidth = 300,
                    wordWrap = true
                };
                newStyle.normal.textColor = Color.white;
                _BodyStyle = newStyle;
            }
            return _BodyStyle;
        }
    }

    private GUIStyle _InfoStyle;
    public GUIStyle InfoStyle
    {
        get
        {
            if (_InfoStyle == null)
            {
                GUIStyle newStyle = new GUIStyle
                {
                    alignment = TextAnchor.MiddleLeft,
                    fontSize = 26,
                };
                newStyle.normal.textColor = Color.white;
                _InfoStyle = newStyle;
            }
            return _InfoStyle;
        }
    }

    private GUIStyle _ButtonStyle;
    public GUIStyle ButtonStyle
    {
        get
        {
            if (_ButtonStyle == null)
            {
                GUIStyle newStyle = new GUIStyle(GUI.skin.button)
                {
                    alignment = TextAnchor.MiddleCenter,
                    fontSize = 22
                };
                newStyle.normal.textColor = Color.white;
                _ButtonStyle = newStyle;
            }
            return _ButtonStyle;
        }
    }
}