using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Transform target;
    public float smoothSpeed = 0.005f;
    public Vector3 velocity;

    Vector3 initPos;
    // Start is called before the first frame update
    void Start()
    {
        initPos = transform.position - target.position;
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion lookRotate = Quaternion.LookRotation(target.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation,lookRotate,0.5f);

        Vector3 targetPos = Vector3.SmoothDamp( transform.position, target.position + initPos, ref velocity, smoothSpeed );

        transform.position =  new Vector3(targetPos.x,target.position.y+0.7f,targetPos.z);
    }
}
