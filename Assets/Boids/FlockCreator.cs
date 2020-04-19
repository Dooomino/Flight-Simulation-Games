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

    public float sightDistance = 5.0f;
    private GameObject[] agents;

    //GPU stuff
    private ComputeBuffer posBuffer;
    private ComputeBuffer velBuffer;
    private ComputeBuffer resultBuffer;
    private Vector3[] resultData;
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

        posBuffer = new ComputeBuffer(numAgents, sizeof(float)*3);
        velBuffer = new ComputeBuffer(numAgents, sizeof(float)*3);
        resultBuffer = new ComputeBuffer(numAgents, sizeof(float)*3);
        resultData = new Vector3[numAgents];
    }

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
    private void setBuffer(){
        posBuffer.SetData(agents.Select(x => x.transform.position).ToArray());
        velBuffer.SetData(agents.Select(x => x.GetComponent<Rigidbody>().velocity).ToArray());

        computeShader.SetBuffer(kernelHandle, "posBuffer", posBuffer);
        computeShader.SetBuffer(kernelHandle, "velBuffer", velBuffer);
        computeShader.SetBuffer(kernelHandle, "resultBuffer", resultBuffer);
    } 
    void runShader(){
        computeShader.Dispatch(kernelHandle, numAgents, 1, 1);
        resultBuffer.GetData(resultData);
    }

    private void moveAgents(){
        for(int i = 0; i < numAgents; i ++){
            agents[i].GetComponent<Rigidbody>().AddForce(resultData[i]);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate(){
        setUniforms();
        setBuffer();
        runShader();
        moveAgents();
    }
}
