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
			Color col = particles[i].color;
			col.a = Mathf.Sin(AudioSampler.spectrum[i] * scale)/60;
			//col.r += (byte)(AudioSampler.pitchvalue * scale / 255);
			//col.g += (byte)(AudioSampler.pitchvalue * scale / 255);
			//col.b += (byte)(AudioSampler.pitchvalue * scale / 255);
			//col.a *= (byte) (AudioSampler.pitchvalue / 255);
			particles [i].color = col;
	}

	ps.SetParticles (particles, particles.Length);
	}
}
