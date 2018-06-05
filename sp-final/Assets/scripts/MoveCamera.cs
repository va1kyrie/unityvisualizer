using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour {

	Transform player;

	void Start () {
		//Invoke delays getting the player camera because it may not exist yet
		Invoke("GetCamTransform", 1f);
	}
	

	void OnMouseDown(){
		player.position = transform.position;
	}
	void GetCamTransform(){
		player = GameObject.FindWithTag("Player").transform;
	}
}
