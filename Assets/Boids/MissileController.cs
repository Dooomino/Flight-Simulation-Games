using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileController : MonoBehaviour
{
    public float explosionRadius = 3.0f;
    public float timeToDet = 3.0f;
    private float startTime;
    private float currentTime;
    public float sightRadius = 5.0f;
    public float rocketForce = 5.0f;
    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider otherCollider){
        Detonate();
    }
    private void Detonate(){
        Destroy(this.gameObject);
    }
    void FixedUpdate(){
        currentTime = Time.time;
        if((currentTime - startTime) > timeToDet){
            Detonate();
            return;
        }
        this.gameObject.GetComponent<Rigidbody>().AddForce(this.gameObject.transform.forward * rocketForce, ForceMode.Impulse);
        Debug.DrawRay(this.transform.position, this.transform.forward, Color.red);
    }
}
