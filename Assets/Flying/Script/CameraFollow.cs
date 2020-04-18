using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Transform target;
    public float smoothSpeed = 0.05f;
    public Vector3 velocity;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion lookRotate = Quaternion.LookRotation(target.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation,lookRotate,Time.deltaTime);

        Vector3 targetPos = Vector3.SmoothDamp( transform.position, target.position + new Vector3(1,0,2), ref velocity, smoothSpeed );

        transform.position =  new Vector3(targetPos.x,target.position.y+1,targetPos.z);
    }
}
