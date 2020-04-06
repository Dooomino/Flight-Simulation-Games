using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    // Start is called before the first frame update
    void Start()
    {
        agent.GetComponent<BoidBehavior>().attractor = attractor;
        agent.GetComponent<BoidBehavior>().sepStrength = sepStrength;
        agent.GetComponent<BoidBehavior>().cohesStrength = cohesStrength;
        agent.GetComponent<BoidBehavior>().aligStrength = aligStrength;
        agent.GetComponent<BoidBehavior>().atractStrength = atractStrength;
        agent.GetComponent<BoidBehavior>().speed = speed;

        for(int i = 0; i < numAgents; i ++){
            Vector3 pos = Random.insideUnitCircle;
            pos += this.gameObject.transform.position;
            pos.x *= Random.value * spawnRadius;
            pos.y *= Random.value * spawnRadius;
            pos.z *= Random.value * spawnRadius;
            Instantiate(agent, pos, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
