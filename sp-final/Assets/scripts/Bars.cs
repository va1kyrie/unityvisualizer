//https://www.youtube.com/watch?v=AkKuXVPlbcE << check this out

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bars : MonoBehaviour {
	public GameObject cube;
	GameObject[] thecubes = new GameObject[512];
	public float scale = 2;

	// Use this for initialization
	void Start () {
		for (int i = 0; i < 512; i++) {
			thecubes [i] = (GameObject)Instantiate (cube);
			thecubes [i].transform.position = this.transform.position;
			thecubes [i].transform.parent = this.transform;
			this.transform.eulerAngles = new Vector3 (0, -.703125f * i, 0);
			thecubes [i].transform.position = Vector3.forward * 180;
		}
	}
	
	// Update is called once per frame
	void Update () {
		for (int i = 0; i < 512; i++) {
			if (thecubes != null) {
				thecubes [i].transform.localScale = new Vector3 (10, Visualizer.samples [i] * scale + 2, 10);
			}
		}
	}
}
