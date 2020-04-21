using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentStats : MonoBehaviour
{

    public int maxHealth = 100;
    public int currentHealth;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }
    
    public void takeDamage(int damage){
        currentHealth -= damage;
        if(currentHealth <= 0 && !this.gameObject.GetComponent<BoidBehavior>().isDead){
            this.gameObject.GetComponent<BoidBehavior>().die();
        }
        
    }
    public void FixedUpdate(){
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
