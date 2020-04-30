using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverheadCanvas : MonoBehaviour
{
    public GameObject target;

    void Update()
    {
        // Offset position above object bbox (in world space)
        float offsetPosY = target.transform.position.y + 1.5f;

        // Final position of marker above GO in world space
        Vector3 offsetPos = new Vector3(target.transform.position.x, offsetPosY, target.transform.position.z);

        // Calculate *screen* position (note, not a canvas/recttransform position)
        Vector2 canvasPos;
        Vector2 screenPoint = Camera.main.WorldToScreenPoint(offsetPos);

        // Convert screen position to Canvas / RectTransform space <- leave camera null if Screen Space Overlay
        RectTransformUtility.ScreenPointToLocalPointInRectangle(new RectTransform(), screenPoint, null, out canvasPos);

        //// Set
        //markerRtra.localPosition = canvasPos;

    }
}
