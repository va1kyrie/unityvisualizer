using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallsAud : MonoBehaviour {
	GameObject[] verts;
	int children;
	public int scale = 2;

	// Use this for initialization
	void Start () {
		verts = GameObject.FindGameObjectsWithTag ("verts");
		children = verts.Length;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 orig, nv;
		//Vector3 norm;
		for (int i = 0; i < verts.Length; i++) {
			orig = verts [i].transform.position;
			nv = orig + new Vector3(0, Visualizer.samples [i] * scale, Visualizer.samples [i] * scale);
			verts[i].transform.position += nv.normalized;
		}
	}
}
