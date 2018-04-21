using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Drop : MonoBehaviour {
	//public AudioSource aud;
	public GameObject songs;
	GameObject[] players;
	int songCount;
	//AudioSource[] aud;
	Dropdown drop;
	GameObject current;
	//public GameObject character;
	//MouseLook[] mice;

	public UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController cam;
	public UnityStandardAssets.Characters.FirstPerson.FirstPersonController cha;
	public void Start(){
		songCount = songs.transform.childCount;
		players = new GameObject[songCount];
		current = GameObject.FindGameObjectWithTag ("nowplaying");
		drop = GetComponent<Dropdown> ();
		onEnable ();
	}

	void onEnable(){
		print ("enabled");
		cha.MouseSwitch (false);
		drop.ClearOptions ();
		List<string> ops = new List<string> ();
		ops.Add ("CHOOSE A SONG");
		for (int i = 0; i < songCount; i++) {
			players [i] = songs.transform.GetChild (i).gameObject;
			ops.Add (players[i].name);
			print (players [i].name);
		}
		//songs = Resources.LoadAll("/music") as AudioClip[];

		drop.AddOptions (ops);
	}


	public void ChangeSong(){
		int choice = drop.value;
		if (choice > 0) {
			GameObject aud = players [choice - 1];
			if (!aud.CompareTag ("nowplaying")) {
				if (current != null) {
					current.tag = "possible";
					current.SetActive (false);
				}
				aud.SetActive (true);
				aud.GetComponent<AudioSource> ().Play ();
				aud.tag = "nowplaying";
				current = aud;
			}
			//aud.Pause ();
			//aud.clip = songs [choice - 1];
			//aud.clip.LoadAudioData ();
			//aud.Play ();
		}
		//character.SetActive (true);
		//cam.MouseSwitch(true);
		cha.MouseSwitch(true);
		//string cap = drop.captionText.text;
		//cap = cap.ToUpper ();
		//drop.captionText.text = cap;
		drop.RefreshShownValue();
		drop.gameObject.SetActive (false);
	}
}
