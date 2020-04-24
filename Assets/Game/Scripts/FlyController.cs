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
    public float bulletTTL = 20;
    public GameObject missilePrefab;
    public float bulletSpeed = 200.0f;
    public float missileSpeed = 500.0f;

    Vector3[] offsets = new Vector3[2];

    public ParticleSystem burst;

    float lastFire;
    public float cooldown = 0.2f;
    private float timer = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        offsets[0] = new Vector3(-0.2f,0.5f,-0.5f);
        offsets[1] = new Vector3(0.2f,0.5f,-0.5f);
        lastFire = Time.deltaTime;
        yaw=0;
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
            yaw += Mathf.Rad2Deg*yawSpeed * Time.deltaTime;
        } else if(Input.GetKey(KeyCode.Q)){
            yaw -= Mathf.Rad2Deg*yawSpeed * Time.deltaTime;
        }
        if(Input.GetKeyDown(KeyCode.R)){
            rb.angularVelocity = new Vector3(0,0,0);
        }

        if(Input.GetKeyDown(KeyCode.F)){
            yaw += 180f;
        }

        pitch = -Mathf.Clamp(pitch,-30,30);
        // yaw = Mathf.Clamp(yaw,-30,30);
        roll = Mathf.Clamp(roll,-30,30);

        transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.Euler(pitch,yaw,roll),Time.deltaTime);

        offsets[0] = Vector3.Slerp(offsets[0],new Vector3(offsets[0].x,0.5f+-rawRoll/2,offsets[0].z),Time.deltaTime);
        offsets[1] = Vector3.Slerp(offsets[1],new Vector3(offsets[1].x,0.5f+rawRoll/2,offsets[1].z),Time.deltaTime);
       
       // decrement the time for cool down
        if(timer > 0){
            timer -= Time.deltaTime;
        }
        // lock at zero if clear
        if(timer <0){
            timer = 0;
        }

        if(Input.GetButton("Fire1") && timer == 0){
                GameObject bullet1 = Instantiate(bulletsPrefab,transform.position + offsets[0],Quaternion.Inverse(transform.rotation));
                GameObject bullet2 = Instantiate(bulletsPrefab,transform.position + offsets[1],Quaternion.Inverse(transform.rotation));
                bullet1.GetComponent<Rigidbody>().AddForce(Vector3.Scale(transform.rotation*transform.forward*accSpeed*bulletSpeed,rb.velocity));
                bullet1.GetComponent<Rigidbody>().AddForce(Vector3.Scale(transform.rotation*transform.up*liftForce,rb.velocity));
                bullet2.GetComponent<Rigidbody>().AddForce(Vector3.Scale(transform.rotation*transform.forward*accSpeed*bulletSpeed,rb.velocity));
                bullet2.GetComponent<Rigidbody>().AddForce(Vector3.Scale(transform.rotation*transform.up*liftForce,rb.velocity));

                Destroy(bullet1,bulletTTL);
                Destroy(bullet2,bulletTTL);
                // reset timer
                timer = cooldown;
        }

        if(Input.GetButton("Fire2")){
            if(!GameObject.FindWithTag("Missile")){
                Vector3 offset = new Vector3(0.2f,0.1f,0);
                GameObject missile = Instantiate(missilePrefab,transform.position + offset,Quaternion.Inverse(transform.rotation));
                missile.GetComponent<Rigidbody>().AddForce(Vector3.Scale(transform.rotation*transform.forward*accSpeed*missileSpeed,rb.velocity),ForceMode.Acceleration);
                missile.GetComponent<Rigidbody>().AddForce(Vector3.Scale(transform.rotation*transform.up*liftForce,rb.velocity),ForceMode.Acceleration);

                Destroy(missile,5.0f);
            }
        }

        if(Input.GetKey(KeyCode.LeftShift)){
            rb.angularVelocity = Vector3.Slerp(rb.angularVelocity,new Vector3(0,0,0),Time.deltaTime);
            rb.AddForce(-transform.forward*accSpeed,ForceMode.Acceleration);
            rb.AddForce(transform.rotation*transform.up*liftForce,ForceMode.Acceleration);
            burst.Play();
        }else{
            if(burst.isPlaying)
                burst.Stop();
        }
        rb.velocity = Vector3.ClampMagnitude(rb.velocity,maxVelocity);

        text_attitude.text = "Altitude: " + transform.position.y.ToString();
    }
}
