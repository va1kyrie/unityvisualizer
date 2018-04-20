using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleAudio : MonoBehaviour {

	ParticleSystem ps;
	ParticleSystem.Particle[] particles;
	ParticleSystem.MainModule mainMod;
	public float scale = 2;

	Vector3 origin = new Vector3(0f, 0f, 0f);

	// Use this for initialization
	void Start () {
		ps = GetComponent<ParticleSystem> ();
		mainMod = ps.main;
	}
	
	// Update is called once per frame
	void Update () {
		int maxParticles = mainMod.maxParticles;
		if (particles == null || particles.Length < maxParticles) {
			particles = new ParticleSystem.Particle[maxParticles];
		}
			
		int pCount = ps.particleCount;
		ps.GetParticles (particles);
		for (int i = 0; i < pCount; i++) {
			Vector3 pos = particles [i].position;
			pos.y += Visualizer.samples [i] * scale + 2;
			//print ("pos " + i + "= " + pos);
			particles [i].position = pos;

			/*
			Color32 col = particles [i].GetCurrentColor (ps);
			col.b += (byte) ((Visualizer.samples [i] * scale + 2) / 255);
			particles [i].color = col;
			*/

			/*if (particles != null) {
				for (int j = 0; j < 512; j++) {
					//Vector3 av3 = particles [i].angularVelocity3D;
					float angle = Vector3.Angle (origin, particles [i].position);
					if (Mathf.Approximately (angle, Mathf.Abs(-.703125f * j))) {
						particles [i].angularVelocity3D = new Vector3 (Visualizer.samples[j] * scale + 2, Visualizer.samples [j] * scale + 2, Visualizer.samples [j] * scale + 2);
					}
				}

			}*/
		}

		ps.SetParticles (particles, particles.Length);
		
	}
}
