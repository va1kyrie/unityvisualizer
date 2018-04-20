using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereEdge : MonoBehaviour {

	public LineRenderer lrendtemp;
	public float maxDist = 100;
	public int maxConn = 4;
	public int maxLr = 362;
	GameObject[] verts;
	int children;

	List<LineRenderer> lrlist = new List<LineRenderer> ();
	Mesh mesh;

	Transform trans;

	// Use this for initialization
	void Start () {
		//children = transform.childCount;
		verts = GameObject.FindGameObjectsWithTag("verts");
		children = verts.Length;
		mesh = transform.GetChild (0).GetComponent<MeshFilter> ().mesh;
		trans = transform;
		//mesh = GetComponent<MeshFilter> ().mesh;
	}
	
	// Update is called once per frame
	/*
	void Update () {
		Vector3[] vertices = mesh.vertices;
		Vector3[] normals = mesh.normals;
		for (int i = 0; i < vertices.Length; i++) {
			vertices [i] += normals [i] * Mathf.Sin (Time.time);
		}

		mesh.vertices = vertices;
	}
	*/

	void LateUpdate(){
		int lchk = lrlist.Count;
		if (lchk > maxLr) {
			for (int i = maxLr; i < lchk; i++) {
				Destroy (lrlist [i].gameObject);
			}

			int removed = lchk - maxLr;
			lrlist.RemoveRange (maxLr, removed);
			lchk -= removed;
		}

		if (maxConn > 0 && maxLr > 0) {
			int lrind = 0;

			float maxDistSqr = maxDist * maxDist;

			for (int i = 0; i < children; i++) {
				if (lrind == maxLr) {
					break;
				}
				Vector3 pos1 = verts [i].transform.position;
				int connections = 0;
				for (int j = i + 1; j < children; j++) {
					Vector3 pos2 = verts [j].transform.position;
					float distSqr = Vector3.SqrMagnitude (pos1 - pos2);
					if (distSqr <= maxDistSqr) {
						LineRenderer lr;
						if (lrind == lchk) {
							lr = Instantiate (lrendtemp, trans, false);
							lrlist.Add (lr);
							lchk++;
						}

						lr = lrlist [lrind];
						lr.enabled = true;

						lr.SetPosition (0, pos1);
						lr.SetPosition (1, pos2);

						lrind++;
						connections++;
						if (connections == maxConn || maxLr == lrind) {
							break;
						}
					}
				}
			}

			for (int i = lrind; i < lchk; i++) {
				lrlist [i].enabled = false;
			}
		}
	}
}
