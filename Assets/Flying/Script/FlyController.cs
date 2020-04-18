using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyController : MonoBehaviour
{
    Rigidbody rb;
    public float maxVelocity = 10f;
    public float accSpeed = 3f;
    public float liftForce = 2f;
    public float pitchSpeed = 300f;
    public float yawSpeed = 300f;
    private float yaw = 0f;
    public float rollingSpeed = 300f;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void Update()
    {
        
        float pitch = Mathf.Rad2Deg*Input.GetAxis("Vertical") * pitchSpeed * Time.deltaTime;
        float roll  = Mathf.Rad2Deg*Input.GetAxis("Horizontal") * rollingSpeed * Time.deltaTime;
        if(Input.GetKey(KeyCode.E)){
            yaw = Mathf.Rad2Deg*yawSpeed * Time.deltaTime;
        } else if(Input.GetKey(KeyCode.Q)){
            yaw = Mathf.Rad2Deg*-yawSpeed * Time.deltaTime;
        }

        transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.Euler(-pitch,yaw,roll),Time.deltaTime);
        if(Input.GetKey(KeyCode.LeftShift)){
            liftForce = Mathf.Clamp(Mathf.Abs(pitch),1,3);
            rb.AddForce(-transform.forward*accSpeed+transform.rotation*transform.up*liftForce,ForceMode.Acceleration);
        }
        rb.velocity = Vector3.ClampMagnitude(rb.velocity,maxVelocity);
    }
}
