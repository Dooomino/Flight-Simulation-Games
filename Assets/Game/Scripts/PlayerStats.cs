using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerStats : MonoBehaviour, Stats
{
    public int maxHealth = 100;
    private int currentHealth;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void takeDamage(int damage){
        currentHealth -= damage;
        if(currentHealth <= 0){
            die();
        }
    }

    public int getHp(){
        return currentHealth;
    }

    private void die(){
        SceneManager.LoadScene("GameOver");
        // Destroy(this.gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
