using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ScoreData : MonoBehaviour
{

    int score;
    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        this.gameObject.GetComponent<TextMeshProUGUI>().text = "Score: " + score;
    }


    public void addScore(int addition){
        score += addition;
        this.gameObject.GetComponent<TextMeshProUGUI>().text = "Score: " + score;
        Debug.Log(score);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
