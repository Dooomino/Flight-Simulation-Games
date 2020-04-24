using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonAction : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void changeScene(){
        SceneManager.LoadScene("Game/Scenes/Game");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
