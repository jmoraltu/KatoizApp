using UnityEngine;
using System.Collections;

public class MusicManager  : Singleton<MusicManager> {

	public GameObject Music;

	GameObject musicClone;
	AudioSource audioMusic;

	private MusicManager(){
		GameObject MusicObject = GameObject.FindGameObjectWithTag("MusicBackground");
		audioMusic = MusicObject.GetComponent<AudioSource>();
		//musicClone= (GameObject)Instantiate(Music, Vector3.zero, Quaternion.identity);
		//audioMusic = musicClone.GetComponent<AudioSource>();
		Debug.Log("audioMusic.name " + audioMusic.name);
	}

	public void PlayMusic(){

		audioMusic.Play();
	}

	public void StopMusic(){
		audioMusic.Stop();
	}
}
