using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Visualizer : MonoBehaviour {

	AudioSource music;
	public static float[] samples = new float[512];
	public static float[] freq = new float[8]; //frequency bands

	// Use this for initialization
	void Start () {
		music = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		GetSpectrumAudioSource ();
		MakeFrequencyBands ();
	}

	void GetSpectrumAudioSource (){
		music.GetSpectrumData (samples, 0, FFTWindow.BlackmanHarris);
	}

	void 

	void MakeFrequencyBands(){
		//song has 22050 hz (from tut)
		//have 512 samples
		// 43 hz per sample

		/* samples
		 * 0-86 hz
		 * 87-258 hz
		 * 259-602
		 * 603-1290
		 * 1291-2666
		 * 2667-5418
		 * 5419-10922
		 * 10923-21930
		 */

	}
}
