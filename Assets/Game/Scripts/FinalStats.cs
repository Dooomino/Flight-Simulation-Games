using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class FinalStats : MonoBehaviour
{
    // Start is called before the first frame update
     public TMP_Text  scoreText;
    void Start()
    {
        float score = ScoreData.getScore();
        scoreText.text = score.ToString();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
