using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// inspired by Original code https://forum.unity.com/threads/world-space-to-canvas-space.460185/
public class ObjectiveTrace : MonoBehaviour
{
    // Start is called before the first frame update
    public Camera cameraObj;
    public Transform obj;

    public RectTransform canvas;
    public RectTransform objText;

    void Start()
    {
        
    }
    void Update()
    {
        //Calculate the position of the UI element
        //0,0 for the canvas is at the center of the screen, whereas 
        //WorldToViewPortPoint treats the lower left corner as 0,0. 
        //Because of this, you need to subtract the height / width of the 
        //canvas * 0.5f to get the correct position.
 
        Vector2 viewPos=Camera.main.WorldToViewportPoint(obj.position);
        Vector2 targetPos=new Vector2(
            ((viewPos.x*canvas.sizeDelta.x)-(canvas.sizeDelta.x*0.5f)),
            ((viewPos.y*canvas.sizeDelta.y)-(canvas.sizeDelta.y*0.5f)));
 
        //now you can set the position of the ui element
        objText.anchoredPosition=targetPos;
    }
}
