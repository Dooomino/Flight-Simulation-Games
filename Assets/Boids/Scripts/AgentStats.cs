using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class AgentStats : MonoBehaviour, Stats
{

    public int maxHealth = 100;
    public int currentHealth;

    public GameObject scoreText;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }
    
    public void takeDamage(int damage){
        currentHealth -= damage;
        if(currentHealth <= 0 && !this.gameObject.GetComponent<BoidBehavior>().isDead){
            
            scoreText.GetComponent<ScoreData>().addScore(10);    
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
