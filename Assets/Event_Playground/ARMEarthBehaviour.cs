using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ARMEarthBehaviour : MonoBehaviour
{

    private bool isRotating = false;

    [SerializeField] private VoidEvent OnObjectSelected;
    

    public void TogglePause()
    {
        isRotating = !isRotating;
    }

    private void Update()
    {
        if (isRotating)
        {
            transform.Rotate(Vector3.up, ((360.985f / 100) * Time.deltaTime) * -1);
        }
    }


    [SerializeField] private Texture2D imageMap;
    [SerializeField] private Color[] colors;
    [SerializeField] private string[] texts;

    private int RetrieveIndexFromColor(Color color)
    {
        for (int i = 0; i < colors.Length; i++)
        {
            if (colors[i] == color)
            {
                return i;
            }
        }
        return -1;
    }
    public void GetPixelColorFromRaycastHit(RaycastHit hit)
    {
        Renderer renderer = hit.transform.GetComponent<Renderer>();
        Texture2D texture2D = renderer.material.mainTexture as Texture2D;

        Vector2 pixelUV = hit.textureCoord;
        pixelUV.x *= texture2D.width;
        pixelUV.y *= texture2D.width;

        Vector2 tilling = renderer.material.mainTextureScale;

        Color color = imageMap.GetPixel(Mathf.FloorToInt(pixelUV.x * tilling.x), Mathf.FloorToInt(pixelUV.y * tilling.y));


        int index = RetrieveIndexFromColor(color);
        if (index >= 0)
        {
            Debug.Log(texts[index]);
        }
    }

}
