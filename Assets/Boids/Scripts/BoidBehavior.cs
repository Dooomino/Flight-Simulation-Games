using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
//Based off of this https://natureofcode.com/book/chapter-6-autonomous-agents/
public class BoidBehavior : MonoBehaviour
{
    public float sightDistance = 5.0f;
    [Range(-1.0f, 1.0f)]
    public float sightAngle = 0.0f; //dot product Increasing this number will decrease the boid's POV. A sight angle of 0.0 gives it 90 deg of view from the forward vector
//https://chortle.ccsu.edu/VectorLessons/vch09/vch09_6.html
    public float sepStrength = 1.0f;

    public float avdTerrnStrength = 1.0f;
    public float cohesStrength = 1.0f;
    public float aligStrength = 1.0f;

    public float atractStrength = 1.0f;

    private float maxForce = 5.0f;

    public float speed = 1.0f;
    public int numPoints = 100;
    public GameObject attractor;
    public Vector3[] sightRays;
    public GameObject explosion;
    public bool isDead = false;
    private float deathStart;
    public float timeToBoom = 3.0f;
    //https://stackoverflow.com/questions/9600801/evenly-distributing-n-points-on-a-sphere/44164075#44164075

    public void Move(Vector3 force){
        if(isDead){
            float currentTime = Time.time;
            if(currentTime - deathStart >= timeToBoom){
                
                Instantiate(explosion, this.gameObject.transform.position, Quaternion.identity);
                Destroy(this.gameObject);
            }

            Vector3 lookAt = this.gameObject.GetComponent<Rigidbody>().velocity;
            Quaternion rotation = Quaternion.LookRotation(lookAt, Vector3.up);
            this.gameObject.transform.rotation = Quaternion.Slerp(this.gameObject.transform.rotation, rotation, 0.1f);
        }else{
            this.gameObject.GetComponent<Rigidbody>().AddForce(force);
            Vector3 lookAt = this.gameObject.GetComponent<Rigidbody>().velocity;
            Quaternion rotation = Quaternion.LookRotation(lookAt, Vector3.up);
            this.gameObject.transform.rotation = Quaternion.Slerp(this.gameObject.transform.rotation, rotation, 0.1f);
        }
        
    }
    private Vector3 AvoidTerrian(){
        int count = 0;
        Vector3 avg = new Vector3(0, 0, 0);
        bool collisionHappened = false;
        foreach(Vector3 ray in sightRays){
            if(Vector3.Dot(ray, this.gameObject.GetComponent<Rigidbody>().velocity) >= sightAngle){
                bool didHit = Physics.Raycast(this.gameObject.transform.position, ray, sightDistance, LayerMask.NameToLayer("Terrain"));
                if(didHit){
                    collisionHappened = true;
                }else{
                    avg += ray;
                    count ++;
                }
            }
        }
        if(collisionHappened){
            return (avg / count).normalized;
        }
        return new Vector3(0, 0, 0);
    }


    // Start is called before the first frame update
    void Start()
    {
        GameObject child = transform.GetChild(0).gameObject;
        child.transform.up = this.transform.forward; //This is a hack to make the cones face the direction that it is flying. We may adjust it if we use a better mesh
    }
    void OnTriggerEnter(Collider otherCollider){
        if(this.gameObject.GetComponent<CapsuleCollider>().isTrigger){
            Instantiate(explosion, this.gameObject.transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
    public void die(){
        Debug.Log("test");
        this.gameObject.GetComponent<Rigidbody>().useGravity = true;
        this.gameObject.GetComponent<CapsuleCollider>().isTrigger = true;
        
        deathStart = Time.time;
        isDead = true;
        
    }
    void FixedUpdate(){

        
    }


    // Update is called once per frame
    void Update()
    {
        

    }

    private Vector3 max(Vector3 force, float maxVal){
        for(int i = 0; i < 3; i ++){
            force[i] = force[i] > maxVal ? maxVal: force[i];
        }
        return force;
    }
  
}
