using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
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

    public TMP_Text text_attitude; 
    
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
        if(Input.GetKeyDown(KeyCode.R)){
            rb.angularVelocity = new Vector3(0,0,0);
        }

        pitch = Mathf.Clamp(pitch,-30,30);
        // yaw = Mathf.Clamp(yaw,-30,30);
        roll = Mathf.Clamp(roll,-30,30);

        transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.Euler(pitch,yaw,-roll),Time.deltaTime);


        if(Input.GetKey(KeyCode.LeftShift)){
            rb.angularVelocity = Vector3.Slerp(rb.angularVelocity,new Vector3(0,0,0),Time.deltaTime);
            float  apply_liftForce = 0;
            if(pitch < 0){
                apply_liftForce = liftForce * Mathf.Abs(pitch) ;
            }else if (pitch == 0){
                apply_liftForce = liftForce;
            }else{
                apply_liftForce = liftForce * -Mathf.Abs(pitch);
            }
            rb.AddForce(transform.forward*accSpeed,ForceMode.Acceleration);
            rb.AddForce(transform.rotation*transform.up*apply_liftForce,ForceMode.Acceleration);
        }
        rb.velocity = Vector3.ClampMagnitude(rb.velocity,maxVelocity);

        text_attitude.text = transform.position.y.ToString();
    }
}
