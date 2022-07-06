using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraResolution1 : MonoBehaviour
{
    Camera m_camera;
    // Start is called before the first frame update
    void Start()
    {
        m_camera = GetComponent<Camera>();
        Rect viewRect = m_camera.rect;
        var scaleHeight = ((float)Screen.width / Screen.height) / ((float)2 / 3);
        var scaleWidth = 1f / scaleHeight;
        if(scaleWidth > 1f)
        {
            viewRect.height = scaleHeight;
            viewRect.y = (1f - scaleHeight) / 2f;
        }
        else
        {
            viewRect.width = scaleWidth;
            viewRect.x = (1f - scaleHeight) / 2f;
        }
        m_camera.rect = viewRect;
    }
    void OnPreCull()
    {
        GL.Clear(true, true, Color.black);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
