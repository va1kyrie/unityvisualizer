using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class FileSeek : MonoBehaviour {

	string path;
	//public GameObject gm;
	AudioSource aud;

	void Start(){
		aud = GetComponent<AudioSource> ();
	}

	public void OpenExplorer(){

		//path = EditorUitility.OpenFilePanel("Overwrite wtih mp3", "", "mp3");
		string[] filetypes = {"MP3", "mp3", "WAV", "wav", "OGG", "ogg"};
		path = EditorUtility.OpenFilePanelWithFilters("choose a song", "", filetypes);
		//print ("path = \"" + path + "\"");
		GetMp3 ();
	}

	void GetMp3(){
		if (path != null && path != "") {
			aud.Pause ();
			UpdateMp3 ();
		}
	}

	void UpdateMp3(){

		print ("processing file");
		StartCoroutine(LoadSong ());


	}

	IEnumerator LoadSong(){
		WWW www = new WWW ("file:///" + path);
		yield return www;

		AudioClip clippie = www.GetAudioClip ();
		aud.clip = clippie;

		//float[] data = new float[clippie.samples];
		//clippie.GetData (data, 0);
		//aud.clip.SetData(data,0);
		aud.clip.LoadAudioData();
		aud.Play ();

		print ("done!");
		
	}

}
