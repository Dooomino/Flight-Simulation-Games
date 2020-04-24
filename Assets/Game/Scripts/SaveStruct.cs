using UnityEngine;

[System.Serializable]
public class SaveData
{
    public int highestScore;
    public SaveData(SaveState save,int Highest){
        highestScore = Highest;
    }
    public SaveData(int Highest){
        highestScore = Highest;
    }
}
