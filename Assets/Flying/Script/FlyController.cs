using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyController : MonoBehaviour
{
    Rigidbody rb;

    public float accSpeed = 3f;
    public float liftForce = 2f;
    public float pitchSpeed = 300f;
    public float yawSpeed = 300f;
    public float rollingSpeed = 300f;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void Update()
    {
        
        float pitch = Input.GetAxis("Vertical") * pitchSpeed * Time.deltaTime;
        float roll = Input.GetAxis("Horizontal") * rollingSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.Euler(-pitch,0,roll),Time.deltaTime);
        if(Input.GetKey(KeyCode.LeftShift)){
            rb.AddForce(-transform.forward*accSpeed+transform.rotation*transform.up*liftForce,ForceMode.Acceleration);
        }else{

        }
    }
}
