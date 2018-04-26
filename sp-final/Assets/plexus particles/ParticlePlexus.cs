using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(ParticleSystem))]
public class ParticlePlexus : MonoBehaviour
{

	public float maxDistance = 1.0f;
	public static float maxDext;
	//for the visualizer
	public int maxConnections = 4;
	public int maxLr = 100;
	public float scale = 2;
	public float maxMag = 20;
	float percentSampled = 0.5f;
	public int numSamples = 512;
	public float smooth = 10f;

	ParticleSystem ps;
	ParticleSystem.Particle[] particles;

	ParticleSystem.MainModule mainMod;
	ParticleSystem.EmissionModule emit;

	//line renderer stuff
	public LineRenderer lrendtemp;
	List<LineRenderer> lrlist = new List<LineRenderer> ();
	Transform trans;
	Vector3 origin;

	// Use this for initialization
	void Start ()
	{
		ps = GetComponent<ParticleSystem> ();
		mainMod = ps.main;
		emit = ps.emission;
		trans = transform;
		//modifications for audio visualization
		maxDext = maxDistance;
		origin = ps.transform.localPosition;
		print ("origin = " + origin);
	}
	
	// Update is called once per frame
	void Update ()
	{
		emit.rateOverTime = new ParticleSystem.MinMaxCurve ((int)numSamples / 5);
		//print ("emit rate = " + emit.rateOverTime.constant);

		int maxParticles = mainMod.maxParticles;
		if (particles == null || particles.Length < maxParticles) {
			particles = new ParticleSystem.Particle[maxParticles];
		}

		int pCount = ps.particleCount;
		percentSampled = (float)maxParticles / AudioSampler.spectrum.Length;
		int avgsize = (int)((AudioSampler.spectrum.Length * percentSampled) / pCount);
		//print("spectrum length = " + AudioSampler.spectrum.Length);
		//print ("percentsampled = " + percentSampled);
		//print("avgsize = " + avgsize);
		int j;
		float sum;
		int specind = 0;
		ps.GetParticles (particles);
		for (int i = 0; i < pCount; i++) {
			j = 0;
			sum = 0;
			while (j < avgsize) {
				sum += AudioSampler.spectrum [specind];
				specind++;
				j++;
			}
			
			Vector3 pos = particles[i].position;
			//pos *= Mathf.Sin(Visualizer.samples [i] / scale + 2);
			//pos *= (AudioSampler.spectrum [i] * scale + 2);
			//pos.x -= Time.deltaTime * smooth;
			//pos.y -= Time.deltaTime * smooth;
			pos -= new Vector3(0, Time.deltaTime * smooth, 0);
			//pos = new Vector3(sum / avgsize * scale, sum / avgsize * scale, sum / avgsize * scale);
			//print("sum = " + sum);
			Vector3 newpos = pos * ((sum / avgsize) * scale);
			//print("pos = " + pos);
			if (pos.magnitude < newpos.magnitude) {
				pos = newpos;
			}
			if (pos.magnitude > maxMag) {
				//print ("mag too much");
				
				//pos = Vector3.Normalize (pos);
				float ratio = maxMag / pos.magnitude;
				pos *= ratio;
				//pos *= scale;
				//pos += new Vector3 (50f, 50f, 50f);

				//print("pos = " + pos);
				//pos *= (AudioSampler.spectrum [i] / 500f);
			}
			//pos *= Mathf.Sin(Visualizer.samples [i] * scale + 2);
			//pos.z *= Mathf.Sin(Visualizer.samples[i] * scale + 2);
			//print ("pos " + i + "= " + pos);

			if (pos != origin) {
				particles [i].position = pos;
			}

			//Color c = particles[i].color;
			//c.a = Mathf.Sin(sum * scale);
			//particles [i].color = c;

			Color32 col = particles [i].GetCurrentColor (ps);
			//col.r *= (byte)(AudioSampler.pitchvalue * scale / 255);
			col.g += (byte)(AudioSampler.pitchvalue * scale / 255);
			col.b += (byte)(AudioSampler.pitchvalue * scale / 255);
			//col.a *= (byte) (AudioSampler.pitchvalue / 255);
			//Color32 col = particles[i].startColor;
			//col.b += (byte) ((sum * scale + 2) / 255);
			//col.g *= (byte)((sum * scale + 2) / 255);
			//col.r *= (byte)((sum * scale + 2) / 255);
			particles [i].color = col;
			//particles [i].startColor = col;



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


	void LateUpdate ()
	{
		int maxParticles = mainMod.maxParticles;

		if (particles == null || particles.Length < maxParticles) {
			particles = new ParticleSystem.Particle[maxParticles];
		}

		int lchk = lrlist.Count;

		if (lchk > maxLr) {
			for (int i = maxLr; i < lchk; i++) {
				Destroy (lrlist [i].gameObject);
			}

			int removed = lchk - maxLr;
			lrlist.RemoveRange (maxLr, removed);
			lchk -= removed;
		}

		int lCount = lrlist.Count;
		if (maxConnections > 0 && maxLr > 0) {

			ps.GetParticles (particles);
			int pCount = ps.particleCount;

			float maxDistanceSqr = maxDistance * maxDistance;

			int lrind = 0;

			for (int i = 0; i < pCount; i++) {
				if (lrind == maxLr) {
					break;
				}
				Vector3 p1pos = particles [i].position;

				int connections = 0;

				for (int j = i + 1; j < pCount; j++) {
					Vector3 p2pos = particles [j].position;
					float distanceSq = Vector3.SqrMagnitude (p1pos - p2pos);
					if (distanceSq <= maxDistanceSqr) {
						//then make connection
						LineRenderer lr;
						if (lrind == lCount) {
							lr = Instantiate (lrendtemp, trans, false);
							lrlist.Add (lr);
							lCount++;
						}
						lr = lrlist [lrind];
						lr.enabled = true;

						lr.SetPosition (0, p1pos);
						lr.SetPosition (1, p2pos);

						lrind++;
						connections++;
						if (connections == maxConnections || maxLr == lrind) {
							break;
						}

					}
				}
			}

			for (int i = lrind; i < lCount; i++) {
				lrlist [i].enabled = false;
			}
		}
	}
}
