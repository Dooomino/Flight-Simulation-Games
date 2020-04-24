using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveTrace : MonoBehaviour
{
    // Start is called before the first frame update
    public Camera cameraObj;

    public Transform obj;

    public RectTransform objText;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 viewportPos = cameraObj.WorldToViewportPoint(obj.position);
 
         //Calculate position considering percentage, using text size
         
         viewportPos.x *= objText.sizeDelta.x;
         viewportPos.y *= objText.sizeDelta.y;
 
         // Remove the 0.5 considering cavnas rectransform pivot.
         viewportPos.x -= objText.sizeDelta.x * objText.pivot.x;
         viewportPos.y -= objText.sizeDelta.y * objText.pivot.y;

         objText.localPosition = viewportPos;
    }
}
