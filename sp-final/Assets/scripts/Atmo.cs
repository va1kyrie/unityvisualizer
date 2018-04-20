using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Atmo : MonoBehaviour {
	ParticleSystem ps;
	ParticleSystem.Particle[] particles;
	ParticleSystem.MainModule mainMod;
	public float scale = 2;

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
			Color c = particles[i].color;
			c.a = Mathf.Sin(Visualizer.samples[i] * scale);
			particles [i].color = c;
	}

	ps.SetParticles (particles, particles.Length);
	}
}
