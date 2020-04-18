using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
public class FlockCreator : MonoBehaviour
{
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

    private GameObject[] agents;

    //GPU stuff
    private ComputeBuffer posBuffer;
    private ComputeBuffer velBuffer;
    private ComputeBuffer resultBuffer;
    [SerializeField] ComputeShader computeShader;
    private int kernelHandle;
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

        agent.GetComponent<BoidBehavior>().attractor = attractor;
        agent.GetComponent<BoidBehavior>().sepStrength = sepStrength;
        agent.GetComponent<BoidBehavior>().cohesStrength = cohesStrength;
        agent.GetComponent<BoidBehavior>().aligStrength = aligStrength;
        agent.GetComponent<BoidBehavior>().atractStrength = atractStrength;
        agent.GetComponent<BoidBehavior>().speed = speed;
        agent.layer = LayerMask.NameToLayer("Agents");
        agent.GetComponent<BoidBehavior>().sightRays = DrawSphere();
        for(int i = 0; i < numAgents; i ++){
            Vector3 pos = UnityEngine.Random.insideUnitCircle;
            pos += this.gameObject.transform.position;
            pos.x *= UnityEngine.Random.value * spawnRadius;
            pos.y *= UnityEngine.Random.value * spawnRadius;
            pos.z *= UnityEngine.Random.value * spawnRadius;
            GameObject currentAgent = Instantiate(agent, pos, Quaternion.identity);
            currentAgent.transform.parent = this.transform;
            agents[i] = currentAgent;
        }


        kernelHandle = computeShader.FindKernel("CSMain");
    }

    void setUniforms(){
        
    }
    void runShader(){

    }
    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate(){
        runShader();
    }
}
