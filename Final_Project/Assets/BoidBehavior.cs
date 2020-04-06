using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Based off of this https://natureofcode.com/book/chapter-6-autonomous-agents/
public class BoidBehavior : MonoBehaviour
{
    public float sightDistance = 5.0f;
    public float sightAngle = 180.0f; //degrees

    public float sepStrength = 1.0f;
    public float cohesStrength = 1.0f;
    public float aligStrength = 1.0f;

    public float atractStrength = 1.0f;

    private float maxForce = 5.0f;

    public float speed = 1.0f;
    public GameObject attractor;
    private GameObject[] NearbyAgents(){
        Collider[] hitColliders = Physics.OverlapSphere(this.gameObject.transform.position, sightDistance);

        List<GameObject> otherAgents = new List<GameObject>();
        foreach(Collider collider in hitColliders){
            otherAgents.Add(collider.gameObject);
        }

        return otherAgents.ToArray();
    }
    private Vector3 FindSeparation(GameObject[] agents){
        Vector3 avoidance = new Vector3(0.0f, 0.0f, 0.0f);
        foreach(GameObject agent in agents){ //If the two velocities intersect, move the current agent away from the other agent
            var temp = this.transform.position - agent.transform.position;
            avoidance += temp;
        }
        avoidance = avoidance / (agents.Length-1);
        //avoidance.Normalize();
        return avoidance;
    }

    private Vector3 FindAlignment(GameObject[] agents){
        Vector3 avg = new Vector3(0.0f, 0.0f, 0.0f);
        foreach(GameObject agent in agents){
            avg += agent.transform.forward;
        }
        return avg / agents.Length;
    }

    private Vector3 MoveToAttract(){
        Vector3 result = (attractor.transform.position - this.transform.position);
        result.Normalize();
        return result;
    }

    private Vector3 FindCohesion(GameObject[] agents){
        Vector3 centeroid = new Vector3(0.0f, 0.0f, 0.0f);
        foreach(GameObject agent in agents){
            centeroid += agent.transform.position;
        }
        //centeroid += this.transform.position;
        centeroid = centeroid / (agents.Length);
        Vector3 direction =  centeroid - this.transform.position;
        direction.Normalize();
        return direction;
    }
    // Start is called before the first frame update
    void Start()
    {
        GameObject child = transform.GetChild(0).gameObject;
        child.transform.up = this.transform.forward; //This is a hack to make the cones face the direction that it is flying. We may adjust it if we use a better mesh
    }

    void FixedUpdate(){
        GameObject[] agents = NearbyAgents();

        Vector3 steeringForce;
        if(agents.Length > 1){ //NearbyAgents() will always contain the current agent. We need to have the agent Length > 1
            Vector3 sep = FindSeparation(agents);
            steeringForce = sep*speed - this.gameObject.GetComponent<Rigidbody>().velocity;
            //steeringForce = max(steeringForce, maxForce);
            this.gameObject.GetComponent<Rigidbody>().AddForce(steeringForce * sepStrength);
            
            Vector3 alig = FindAlignment(agents);
            steeringForce = alig*speed - this.gameObject.GetComponent<Rigidbody>().velocity;
            //steeringForce = max(steeringForce, maxForce);
            this.gameObject.GetComponent<Rigidbody>().AddForce(steeringForce * aligStrength);

            Vector3 cohes = FindCohesion(agents);
            steeringForce = cohes*speed - this.gameObject.GetComponent<Rigidbody>().velocity;
            //steeringForce = max(steeringForce, maxForce);
            this.gameObject.GetComponent<Rigidbody>().AddForce(steeringForce * cohesStrength);
        }    

        var desiredVelocity = MoveToAttract();
        steeringForce = desiredVelocity*speed - this.gameObject.GetComponent<Rigidbody>().velocity;
        //steeringForce = max(steeringForce, maxForce);
        this.gameObject.GetComponent<Rigidbody>().AddForce(steeringForce *atractStrength);
        Vector3 lookAt = this.gameObject.GetComponent<Rigidbody>().velocity;
        Quaternion rotation = Quaternion.LookRotation(lookAt, Vector3.up);

        GameObject child = transform.GetChild(0).gameObject;
        child.transform.rotation = Quaternion.Slerp(child.transform.rotation, rotation, 0.1f);

        
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
