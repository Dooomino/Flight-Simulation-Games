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
        Debug.Log("Hit");
        if(otherCollider.gameObject.layer == enemyLayer){
            otherCollider.gameObject.GetComponent<Stats>().takeDamage(10);
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
