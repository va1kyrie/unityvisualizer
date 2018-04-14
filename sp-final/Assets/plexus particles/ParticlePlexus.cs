using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(ParticleSystem))]
public class ParticlePlexus : MonoBehaviour {

	public float maxDistance = 1.0f;
	public int maxConnections = 4;
	public int maxLr = 100;


	new ParticleSystem ps;
	ParticleSystem.Particle[] particles;

	ParticleSystem.MainModule mainMod;

	//line renderer stuff
	public LineRenderer lrendtemp;
	List<LineRenderer> lrlist = new List<LineRenderer>();
	Transform trans;

	// Use this for initialization
	void Start () {
		ps = GetComponent<ParticleSystem> ();
		mainMod = ps.main;
		trans = transform;
	}
	
	// Update is called once per frame
	void LateUpdate () {
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
