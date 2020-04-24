using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
//Based off of this https://natureofcode.com/book/chapter-6-autonomous-agents/
public class BoidBehavior : MonoBehaviour
{
    public GameObject explosion;
    public bool isDead = false;
    private float deathStart;
    public float timeToBoom = 3.0f;
    

    public void Move(Vector3 force){
        if(isDead){
            float currentTime = Time.time;
            if(currentTime - deathStart >= timeToBoom){ //Destroy the boid mid-air if it hasn't hit anything after a few seconds
                
                Instantiate(explosion, this.gameObject.transform.position, Quaternion.identity);
                Destroy(this.gameObject);
            }

        }else{
            this.gameObject.GetComponent<Rigidbody>().AddForce(force); //Apply the result of the flocking rules to the boid
            
        }
            //Rotate the boid so that it looks in the direction that it is going
            Vector3 lookAt = this.gameObject.GetComponent<Rigidbody>().velocity;
            Quaternion rotation = Quaternion.LookRotation(lookAt, Vector3.up);
            this.gameObject.transform.rotation = Quaternion.Slerp(this.gameObject.transform.rotation, rotation, 0.1f);
    }

    // Start is called before the first frame update
    void Start()
    {
        GameObject child = transform.GetChild(0).gameObject;
        child.transform.up = this.transform.forward; //This is a hack to make the cones face the direction that it is flying. We may adjust it if we use a better mesh
    }
    //If the boid has been defeated, blow up the boid when it hits something
    void OnTriggerEnter(Collider otherCollider){
        if(this.gameObject.GetComponent<CapsuleCollider>().isTrigger){
            Instantiate(explosion, this.gameObject.transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
    public void die(){
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
  
}
