using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Gyroscope: MonoBehaviour {

	GameObject camContainer;
	UnityEngine.Gyroscope gyro;
	Quaternion rot;

	void Start(){
		PrepCamera();
		EnableGyro();
	}
		
	void Update(){
		transform.localRotation = Input.gyro.attitude * rot;
	}

	void PrepCamera(){
		camContainer = new GameObject("Camera Container");
		camContainer.tag = "Player";
		camContainer.transform.position = transform.position;
		transform.SetParent(camContainer.transform);
		camContainer.transform.rotation = Quaternion.Euler(90f, 90f, 0f);
		rot = new Quaternion(0, 0, 1, 0);
	}
		
	void EnableGyro(){
		if(SystemInfo.supportsGyroscope){
			gyro = Input.gyro;
			gyro.enabled = true; 
		}
	}
		
}


