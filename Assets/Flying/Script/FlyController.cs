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
    public Animator plane;
    public TMP_Text text_attitude; 
    
    public GameObject bulletsPrefab;
    public float bulletSpeed = 200.0f;

    Vector3[] offsets = new Vector3[2];
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        offsets[0] = new Vector3(-0.2f,0.5f,-0.5f);
        offsets[1] = new Vector3(0.2f,0.5f,-0.5f);

    }
    // Update is called once per frame
    void Update()
    {
        
        float pitch = Mathf.Rad2Deg*Input.GetAxis("Vertical") * pitchSpeed * Time.deltaTime;
        float rawRoll = Input.GetAxis("Horizontal");
        float roll  = Mathf.Rad2Deg* rawRoll * rollingSpeed * Time.deltaTime;

        if(pitch > 0){
            plane.SetFloat("pitch_speed",1.0f);
            plane.SetFloat("pitch_direction",1.0f);
        }else if(pitch == 0){
             plane.SetFloat("pitch_speed",-1.0f);
        }else{ 
            plane.SetFloat("pitch_speed",1.0f);
            plane.SetFloat("pitch_direction",-1.0f);
        }

         if(roll > 0){
            plane.SetFloat("roll_speed",1.0f);
            plane.SetFloat("roll_direction",1.0f);
        }else if(roll == 0){
             plane.SetFloat("roll_speed",-1.0f);
        }else{ 
            plane.SetFloat("roll_speed",1.0f);
            plane.SetFloat("roll_direction",-1.0f);
        }

        if(Input.GetKey(KeyCode.E)){
            yaw = Mathf.Rad2Deg*yawSpeed * Time.deltaTime;
        } else if(Input.GetKey(KeyCode.Q)){
            yaw = Mathf.Rad2Deg*-yawSpeed * Time.deltaTime;
        }
        if(Input.GetKeyDown(KeyCode.R)){
            rb.angularVelocity = new Vector3(0,0,0);
        }

        pitch = -Mathf.Clamp(pitch,-30,30);
        // yaw = Mathf.Clamp(yaw,-30,30);
        roll = Mathf.Clamp(roll,-30,30);

        transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.Euler(pitch,yaw,roll),Time.deltaTime);

        offsets[0] = Vector3.Slerp(offsets[0],new Vector3(offsets[0].x,0.5f+-rawRoll/2,offsets[0].z),Time.deltaTime);
        offsets[1] = Vector3.Slerp(offsets[1],new Vector3(offsets[1].x,0.5f+rawRoll/2,offsets[1].z),Time.deltaTime);
        if(Input.GetButton("Fire1")){
            GameObject bullet1 = Instantiate(bulletsPrefab,transform.position + offsets[0],transform.rotation);
            GameObject bullet2 = Instantiate(bulletsPrefab,transform.position + offsets[1],transform.rotation);
            bullet1.GetComponent<Rigidbody>().AddForce(Vector3.Scale(transform.forward*accSpeed*bulletSpeed,rb.velocity));
            bullet1.GetComponent<Rigidbody>().AddForce(Vector3.Scale(transform.rotation*-transform.up*liftForce,rb.velocity));
            bullet2.GetComponent<Rigidbody>().AddForce(Vector3.Scale(transform.forward*accSpeed*bulletSpeed,rb.velocity));
            bullet2.GetComponent<Rigidbody>().AddForce(Vector3.Scale(transform.rotation*-transform.up*liftForce,rb.velocity));

            Destroy(bullet1,3.0f);
            Destroy(bullet2,3.0f);
        }

        if(Input.GetKey(KeyCode.LeftShift)){
            rb.angularVelocity = Vector3.Slerp(rb.angularVelocity,new Vector3(0,0,0),Time.deltaTime);
            rb.AddForce(-transform.forward*accSpeed,ForceMode.Acceleration);
            rb.AddForce(transform.rotation*transform.up*liftForce,ForceMode.Acceleration);
        }
        rb.velocity = Vector3.ClampMagnitude(rb.velocity,maxVelocity);

        text_attitude.text = transform.position.y.ToString();
    }
}
