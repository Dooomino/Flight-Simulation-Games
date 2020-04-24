using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
public class FlockCreator : MonoBehaviour
{

    public GameObject scoreText;
    public int numAgents = 100;
    public int spawnRadius = 20;
    public GameObject attractor;
    public GameObject agent;

    public float sepStrength = 1.0f;
    public float cohesStrength = 1.0f;
    public float aligStrength = 1.0f;

    public float atractStrength = 1.0f;

    public float speed = 1.0f;
    public int numPoints = 100;

    public float sightDistance = 5.0f;
    [Range(-1.0f, 1.0f)]
    public float sightAngle = 0.0f; //dot product Increasing this number will decrease the boid's POV. A sight angle of 0.0 gives it 90 deg of view from the forward vector

    private Vector3[] sightRays;
    private GameObject[] agents;

    //GPU stuff
    private ComputeBuffer posBuffer;
    private ComputeBuffer velBuffer;
    private ComputeBuffer resultBuffer;

    private Vector3[] resultData;
    [SerializeField] ComputeShader computeShader;
    private int kernelHandle;
    public float sightRadius = 5.0f;
    public LayerMask terrianLayer;
    //https://stackoverflow.com/questions/9600801/evenly-distributing-n-points-on-a-sphere/44164075#44164075
    private Vector3[] DrawSphere(){
        
        IEnumerable<float> indicies = Enumerable.Range(0, numPoints).Select(x => (float)x + 0.5f);

        IEnumerable<float> phis = indicies.Select(x => (float)Math.Acos(1.0f - 2 * x / numPoints));
        IEnumerable<float> thetas = indicies.Select(x => (float)(Math.PI * (1 + Math.Pow(5.0, 0.5) * x)));

        
        Vector3[] points = new Vector3[numPoints];
        for(int i = 0; i < numPoints; i ++){
            //x, y, z = cos(theta) * sin(phi), sin(theta) * sin(phi), cos(phi);
            float x = (float)(Math.Cos(thetas.ElementAt(i)) * Math.Sin(phis.ElementAt(i)));
            float y = (float)(Math.Sin(thetas.ElementAt(i)) * Math.Sin(phis.ElementAt(i)));
            float z = (float)(Math.Cos(phis.ElementAt(i)));
            points[i] = new Vector3(x, y, z);
        }

        return points;
    }

    // Start is called before the first frame update
    void Start()
    {
        agents = new GameObject[numAgents];
        agent.layer = LayerMask.NameToLayer("Agents");
        


        for(int i = 0; i < numAgents; i ++){
            Vector3 pos = UnityEngine.Random.insideUnitCircle;
            pos += this.gameObject.transform.position;
            pos.x *= UnityEngine.Random.value * spawnRadius;
            pos.y *= UnityEngine.Random.value * spawnRadius;
            pos.z *= UnityEngine.Random.value * spawnRadius;
            GameObject currentAgent = Instantiate(agent, this.transform.position, Quaternion.identity);
            currentAgent.GetComponent<AgentStats>().scoreText = scoreText;
            currentAgent.transform.parent = this.transform;
            
        }


        kernelHandle = computeShader.FindKernel("CSMain");

        sightRays = DrawSphere();

        
    }
    //Think of thses as the uniform variables in OpenGL.
    private void setUniforms(){
        
        computeShader.SetFloat("sepStrength", sepStrength);
        computeShader.SetFloat("cohesStrength", cohesStrength);
        computeShader.SetFloat("aligStrength", aligStrength);
        computeShader.SetFloat("atractStrength", atractStrength);
        computeShader.SetFloat("sightDistance", sightDistance);
        computeShader.SetFloat("speed", speed);
        computeShader.SetInt("numAgents", numAgents);

        float[] attractorPos = new float[3];
        attractorPos[0] = attractor.gameObject.transform.position.x;
        attractorPos[1] = attractor.gameObject.transform.position.y;
        attractorPos[2] = attractor.gameObject.transform.position.z;
        computeShader.SetFloats("attractorPos", attractorPos);
    }

    //Send the Flock's positions and velocities to the GPU
    private void setBuffer(){

        //Initalization
        posBuffer = new ComputeBuffer(numAgents, sizeof(float)*3);
        velBuffer = new ComputeBuffer(numAgents, sizeof(float)*3);
        resultBuffer = new ComputeBuffer(numAgents, sizeof(float)*3);
        resultData = new Vector3[numAgents];

        //Set the agent's position and velocity to the buffer
        posBuffer.SetData(agents.Select(x => x.transform.position).ToArray()); 
        velBuffer.SetData(agents.Select(x => x.GetComponent<Rigidbody>().velocity).ToArray());

        //Send the buffers to the GPU
        computeShader.SetBuffer(kernelHandle, "posBuffer", posBuffer);
        computeShader.SetBuffer(kernelHandle, "velBuffer", velBuffer);
        computeShader.SetBuffer(kernelHandle, "resultBuffer", resultBuffer);
    } 
    void runShader(){
        computeShader.Dispatch(kernelHandle, numAgents, 1, 1); //Launch the compute shader

        resultBuffer.GetData(resultData);

        //Clean up
        posBuffer.Dispose();
        velBuffer.Dispose();
        resultBuffer.Dispose();

        
    }
    //Taken from https://youtu.be/bqtqltqcQhw?t=384
    /*Ideally, we should find a way to put this into the GPU since this is a _very_ expensive computation
    One possible solution is take to dot product check and put it onto the GPU and return the valid rays. That way, we will
    reduce the majority of the rays that we are actually checking
     */
    private Vector3 AvoidTerrian(GameObject agent){
        Vector3 bestDir = agent.GetComponent<Rigidbody>().velocity.normalized;
        float furthestDistance = 0;
        RaycastHit hit;
        //Find the ray that is farthest away from the collider and return that ray
        foreach(Vector3 ray in sightRays){
            if(Vector3.Dot(ray, agent.GetComponent<Rigidbody>().velocity) >= sightAngle){
                if(Physics.SphereCast(agent.gameObject.transform.position, sightRadius, ray, out hit, sightDistance, terrianLayer)){
                    
                    if(hit.distance > furthestDistance){
                        bestDir = ray;
                        furthestDistance = hit.distance;
                    }
                }else{
                    return bestDir;
                }

            }
        }
        return bestDir;
    }
    private void moveAgents(){
        for(int i = 0; i < numAgents; i ++){
            Vector3 temp = AvoidTerrian(agents[i]);
            Vector3 avoidForce = temp * speed - agents[i].GetComponent<Rigidbody>().velocity;
            agents[i].GetComponent<BoidBehavior>().Move(resultData[i]+avoidForce);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate(){
        numAgents = this.gameObject.transform.childCount;
        agents = new GameObject[numAgents];
        for(int i = 0; i < numAgents; i ++){
            agents[i] = this.gameObject.transform.GetChild(i).gameObject;
        }
        setUniforms();
        setBuffer();
        runShader();
        moveAgents();
    }
}
