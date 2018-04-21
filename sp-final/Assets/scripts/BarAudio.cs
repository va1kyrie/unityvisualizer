using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarAudio : MonoBehaviour {

	Transform[] visualList;
	float[] vscale;
	public int amnVisual = 10;

	public float scale = 200f;
	public float smooth = 10f;
	public float maxvscale = 25f;
	public float keeppercent = 0.5f;

	void SpawnLine(){
		vscale = new float[amnVisual];
		visualList = new Transform[amnVisual];

		for (int i = 0; i < amnVisual; i++) {
			GameObject go = GameObject.CreatePrimitive (PrimitiveType.Cube) as GameObject;
			visualList [i] = go.transform;
			visualList [i].transform.position = Vector3.right * i;
		}
	}

	// Use this for initialization
	void Start () {
		SpawnLine ();
	}
	
	// Update is called once per frame
	void Update () {
		int visualindex = 0;
		int specind = 0;
		int avgsize = (int) ((1024 * keeppercent) / amnVisual);

		while (visualindex < amnVisual) {
			int j = 0;
			float sum = 0;
			while (j < avgsize) {
				sum += AudioSampler.spectrum [specind];
				specind++;
				j++;
			}

			float scaleY = sum / avgsize * scale;
			vscale [visualindex] -= Time.deltaTime * smooth;
			if (vscale [visualindex] < scaleY) {
				vscale [visualindex] = scaleY;
			}
			if(vscale[visualindex] > maxvscale){
				vscale [visualindex] = maxvscale;
			}

			visualList [visualindex].localScale = Vector3.one + Vector3.up * vscale [visualindex];
			visualindex++;
		}
	}
}
