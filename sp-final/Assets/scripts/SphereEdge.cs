using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereEdge : MonoBehaviour {

	public LineRenderer lrendtemp;
	public int maxConn = 4;
	public int maxLr = 362;
	GameObject[] verts;
	int children;

	//List<LineRenderer> lrlist = new List<LineRenderer> ();
	Mesh mesh;

	// Use this for initialization
	void Start () {
		//children = transform.childCount;
		verts = GameObject.FindGameObjectsWithTag("verts");
		children = verts.Length;
		mesh = transform.GetChild (0).GetComponent<MeshFilter> ().mesh;
		//mesh = GetComponent<MeshFilter> ().mesh;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3[] vertices = mesh.vertices;
		Vector3[] normals = mesh.normals;
		for (int i = 0; i < vertices.Length; i++) {
			vertices [i] += normals [i] * Mathf.Sin (Time.time);
		}

		mesh.vertices = vertices;
	}
}
