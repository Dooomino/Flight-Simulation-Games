using UnityEngine;
using System.Collections;

public class DestroyEffect : MonoBehaviour {
	float startTime;
	//Modifcations to destroy it after a certain time
	void Start(){
		startTime = Time.time;
	}

	void FixedUpdate(){
		if((Time.time - startTime)  > 3.0f){
			Destroy(transform.gameObject);
		}
	}
	/*
	void Update ()
	{

		if(Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.C))
		   Destroy(transform.gameObject);
	
	}*/
}
