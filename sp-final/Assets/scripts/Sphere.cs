using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sphere : MonoBehaviour {
	public int resolution = 8;
	public float threshold = 0.5f;
	public int scale = 2;
	private ParticleSystem.Particle[] points;
	ParticleSystem ps;

	private int currentRes;

	private void CreatePoints() {
		currentRes = resolution;
		points = new ParticleSystem.Particle[resolution * resolution * resolution];
		float incremet = 1f / (resolution - 1);
		int i = 0;
		for (int x = 0; x < resolution; x++) {
			for (int z = 0; z < resolution; z++) {
				for (int y = 0; y < resolution; y++) {
					Vector3 p = new Vector3 (x, y, z) * incremet;
					points [i].position = p;
					points [i].color = ps.main.startColor.color;
					points [i++].size = 0.1f;
				}
			}
		}
	}

	// Use this for initialization
	void Start () {
		//points = new ParticleSystem.Particle[resolution];
		ps = GetComponent<ParticleSystem> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (currentRes != resolution || points == null) {
			CreatePoints ();
		}
		float t = Time.timeSinceLevelLoad;
		//ps.SetParticles (points, points.Length);
		for (int i = 0; i < points.Length; i++) {
			Color c = points [i].color;
			//print ("vis = " + Visualizer.samples [i]);

			c.a = Ripple (points [i].position, t, (float) (Visualizer.samples[i] * scale + 2));
			points [i].color = c;

		}
		//print ("i = " + points.Length);
		ps.SetParticles (points, points.Length);
	}

	float Ripple(Vector3 p, float t, float v){
		//print ("t = " + t);
		p.x -= 0.5f;
		p.y -= 0.5f;
		p.z -= 0.5f;
		float sqRad = p.x * p.x + p.y * p.y + p.z * p.z;
		return Mathf.Sin (4f * Mathf.PI * sqRad - 2f * t * v);
	}
}
