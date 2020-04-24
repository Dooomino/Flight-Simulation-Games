using UnityEngine;
public class SaveState : MonoBehaviour {
    private string savePath;
    public int highestScore = 0;
    void Update()
    {
        float hp = GameObject.Find("Player").GetComponent<PlayerStats>().getHp();
        if(hp <=0 ){ //If the player dies, save the chest count if it beats the record
            Load();
            if(highestScore < ScoreData.getScore())
                highestScore = ScoreData.getScore();
            Debug.Log(ScoreData.getScore());
            Save();
        }
    }
    private void Awake() {        
        savePath = Application.persistentDataPath + "/saveData.json"; 
        Debug.Log("Save will be save to the path: " + savePath);
        Load();
    }    
    private void Save() {        
        SaveManager.SaveAsJSON(savePath, new SaveData(this,highestScore));  
    }
    private void Load() {  
        SaveData save = SaveManager.LoadFromJSON(savePath);
        this.highestScore = save.highestScore;
    }
}