using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GracesGames.SimpleFileBrowser.Scripts;

// Include these namespaces to use BinaryFormatter
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class ChooseSong : MonoBehaviour {
	string pathtosong = "";
	public string[] extensions;
	bool landscape = true;

	AudioSource aud;
	public UnityStandardAssets.Characters.FirstPerson.FirstPersonController cha;

	// Use this for initialization
	void Start () {
		aud = GetComponent<AudioSource> ();
		#if UNITY_STANDALONE || UNITY_EDITOR_WIN
		//FileBrowser.SetFilters (true, new FileBrowser.Filter ("Audio", ".wav", ".ogg"));
		//extensions = new string[] {"wav", "ogg"};
		#endif


		#if UNITY_ANDROID
		Permission perm = FileBrowser.CheckPermission ();
//		if(perm == Permission.Granted){
//			FileBrowser.SetFilters (true, new FileBrowser.Filter ("Audio", ".wav", ".ogg", ".mp3"));
//		}else{
//			Permission now = Permission.RequestPermission();
//			if(now != Permission.Granted){
//				print("requires permission");
//			}
//		}
		extensions = new string[] {"wav", "ogg", "mp3"};
		landscape = false;

		#endif

	}

	public GameObject FileBrowserPrefab;
	public bool PortraitMode;

	public void OpenFileBrowser(){
		aud.Pause ();
		GameObject fileBrowserObject = Instantiate(FileBrowserPrefab, transform);
		fileBrowserObject.name = "FileBrowser";
		// Set the mode to save or load
		FileBrowser fileBrowserScript = fileBrowserObject.GetComponent<FileBrowser>();
		fileBrowserScript.SetupFileBrowser(PortraitMode ? ViewMode.Portrait : ViewMode.Landscape);
		fileBrowserScript.OpenFilePanel(extensions);
		// Subscribe to OnFileSelect event (call LoadFileUsingPath using path) 
		fileBrowserScript.OnFileSelect += PickSong;
		fileBrowserScript.OnFileBrowserClose += NoChange;
	}

	void NoChange(){
		aud.Play ();
		cha.MouseSwitch (true);
	}

	void PickSong(string path){
		print ("time to change");
		print ("path = " + path);
		//bool open = SimpleFileBrowser.FileBrowser.ShowLoadDialog((path) => {pathtosong = path;}, null, false, "%userprofile%/Music", "Select a Song", "Select");

		if (path != "" && path != null) {
			pathtosong = path;
			UpdateSong ();
		} else {
			aud.Play ();
			cha.MouseSwitch (true);
		}
			
	}

	void UpdateSong(){
		print ("processing file");
		StartCoroutine (LoadSong ());
	}

	IEnumerator LoadSong(){
		#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
		WWW www = new WWW("file:///" + pathtosong);
		#endif

		#if UNITY_ANDROID
		WWW www = new WWW("file://" + pathtosong);
		#endif

		yield return www;

		AudioClip clippie = www.GetAudioClip ();
		aud.clip = clippie;

		aud.clip.LoadAudioData ();
		aud.Play ();
		print ("done!");
		cha.MouseSwitch (true);

	}
}
