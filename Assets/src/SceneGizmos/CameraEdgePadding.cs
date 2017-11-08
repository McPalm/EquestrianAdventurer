/*
 * Calculates the cameras max and min positions to snap to the bounds of the map using the current zoom level.
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEdgePadding : MonoBehaviour
{
    public Vector2 max;
    public Vector2 min;

    public GameObject focus;
    

    // Update is called once per frame
    void LateUpdate ()
    {
        // update max and min based on player location
        if(focus.transform.position.x > max.x || focus.transform.position.y > max.y || focus.transform.position.y < min.y || focus.transform.position.x < min.x)
        {
            RefreshBounds();
        }
        GetComponent<DragFollow>().max = new Vector2(max.x - Camera.main.orthographicSize * Camera.main.pixelWidth / Camera.main.pixelHeight, max.y - Camera.main.orthographicSize);
        GetComponent<DragFollow>().min = new Vector2(min.x + Camera.main.orthographicSize * Camera.main.pixelWidth / Camera.main.pixelHeight, min.y + Camera.main.orthographicSize);
    }

    /// <summary>
    /// Refresh the bounds to contain the map section the focus is in.
    /// </summary>
    private void RefreshBounds()
    {
        float y = Mathf.Floor(focus.transform.position.y / 41);
        float x = Mathf.Floor(focus.transform.position.x / 41);

        max = new Vector2(x * 41f + 40.5f, y * 41f + 40.5f);
        min = new Vector2(x * 41f - 0.5f, y * 41f - 0.5f);
    }
}
