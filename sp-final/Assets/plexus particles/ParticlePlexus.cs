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

	ParticleSystem ps;
	ParticleSystem.Particle[] particles;

	ParticleSystem.MainModule mainMod;

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
		trans = transform;
		//modifications for audio visualization
		maxDext = maxDistance;
		origin = ps.transform.localPosition;
		print ("origin = " + origin);
	}
	
	// Update is called once per frame
	void Update ()
	{
		int maxParticles = mainMod.maxParticles;
		if (particles == null || particles.Length < maxParticles) {
			particles = new ParticleSystem.Particle[maxParticles];
		}

		int pCount = ps.particleCount;
		ps.GetParticles (particles);
		for (int i = 0; i < pCount; i++) {
			
			Vector3 pos = particles [i].position;
			//pos *= Mathf.Sin(Visualizer.samples [i] / scale + 2);
			pos *= (Visualizer.samples [i] * scale + 2);
			if (pos.magnitude > maxMag) {
				
				pos = Vector3.Normalize (pos);
				//print("pos = " + pos);
				pos *= (Visualizer.samples[i] / 500f);
			}
			//pos *= Mathf.Sin(Visualizer.samples [i] * scale + 2);
			//pos.z *= Mathf.Sin(Visualizer.samples[i] * scale + 2);
			//print ("pos " + i + "= " + pos);

			if (pos != origin) {
				particles [i].position = pos;
			}

			Color c = particles[i].color;
			c.a = Mathf.Sin(Visualizer.samples[i] * scale);
			//particles [i].color = c;

			Color32 col = particles [i].GetCurrentColor (ps);
			col.b += (byte) ((Visualizer.samples [i] * scale + 2) / 255);
			particles [i].color = col;



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
