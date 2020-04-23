using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private float startTime;
    public float timeToDespawn;
    public LayerMask enemyLayer;
    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
    }
    void OnTriggerEnter(Collider otherCollider){
        //Deal damage to the agent if it hits
        if(otherCollider.gameObject.layer == LayerMask.NameToLayer("Agents")){
            otherCollider.gameObject.GetComponent<Stats>().takeDamage(50);
        }

        Destroy(this.gameObject);
    }
    void FixedUpdate(){
        float currentTime = Time.time;
        if((currentTime - startTime) >= timeToDespawn){
            Destroy(this.gameObject);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
