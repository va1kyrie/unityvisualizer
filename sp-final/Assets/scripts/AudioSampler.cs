using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSampler : MonoBehaviour {

	public float rmsvalue; //avg power output of sound
	public float dbvalue; //db
	public float pitchvalue; //pitch

	private AudioSource source;
	public static float[] samples;
	public static float[] spectrum;
	private float sampleRate;

	// Use this for initialization
	void Start () {
		source = GetComponent<AudioSource> ();
		samples = new float[1024];
		spectrum = new float[1024];
		sampleRate = AudioSettings.outputSampleRate;
	}
	
	// Update is called once per frame
	void Update () {
		AnalyzeSound ();
	}

	void AnalyzeSound(){
		source.GetOutputData (samples, 0);

		//get rmsvalue
		float sum = 0;
		for (int i = 0; i < samples.Length; i++) {
			sum += samples [i] * samples [i];
		}
		rmsvalue = Mathf.Sqrt (sum / samples.Length);

		//then can calc the dbvalue
		dbvalue = 20 * Mathf.Log10(rmsvalue/0.1f);

		//get visualizer data > sound spectrum
		source.GetSpectrumData(spectrum,0,FFTWindow.BlackmanHarris);

		//find pitch?
		float maxV = 0;
		var maxN = 0;
		for (int i = 0; i < spectrum.Length; i++) {
			if(!(spectrum[i] > maxV) || !(spectrum[i] > 0.0f)){
				continue;
			}
			maxV = spectrum [i];
			maxN = i;
		}

		float freqN = maxN;
		if (maxN > 0 && maxN < spectrum.Length - 1) {
			var dL = spectrum [maxN - 1] / spectrum [maxN];
			var dr = spectrum [maxN + 1] / spectrum [maxN];
			freqN += 0.5f * (dr * dr - dL * dL);
		}

		pitchvalue = freqN * (sampleRate / 2) / samples.Length;
	}
}
