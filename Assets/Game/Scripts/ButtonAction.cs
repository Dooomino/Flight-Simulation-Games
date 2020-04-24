using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; 

public class ButtonAction : MonoBehaviour
{
    // Start is called before the first frame update
    public TMP_Text scoreText;
    void Start()
    {
        string savePath = Application.persistentDataPath + "/saveData.json"; 
        float score = SaveManager.LoadFromJSON(savePath).highestScore;
        scoreText.text = score.ToString();
    }

    public void changeScene(){
        SceneManager.LoadScene("Game");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
