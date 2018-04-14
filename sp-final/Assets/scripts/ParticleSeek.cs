using UnityEngine;

public class ParticleSeek : MonoBehaviour {
	public Transform target;
	public float force = 10f;

	ParticleSystem ps;

	// Use this for initialization
	void Start () {
		ps = GetComponent<ParticleSystem> ();
	}
	
	// Update is called once per frame
	void LateUpdate () {
		ParticleSystem.Particle[] parr = new ParticleSystem.Particle[ps.particleCount];

		//now fill array with particles
		ps.GetParticles(parr);

		ParticleSystem.Particle p;
		Vector3 dir2tar, seekforce;

		//now we're iterating all the particles
		float df = force * Time.deltaTime;
		for (int i = 0; i < parr.Length; i++) {
			dir2tar = Vector3.Normalize(target.position - parr[i].position);
			seekforce = dir2tar * df;
			parr[i].velocity += seekforce;
		}
		ps.SetParticles (parr, ps.particleCount);
	}
}
