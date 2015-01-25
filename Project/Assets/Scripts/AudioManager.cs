using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour 
{
	public AudioClip[] sounds;
	
	public GameObject[] audioSources;
	
	private int nextBuzz = 0;
	private int nextSound = 5;
	private int lastAmbient;
	void Start()
	{
		StartCoroutine("PlayAmbient");
	}
	
	//Player communication noises
	public void PlayBuzz()
	{
		int random = Random.Range (0,4);
		audioSources[nextBuzz].GetComponent<AudioSource>().clip = sounds[random];
		audioSources[nextBuzz].GetComponent<AudioSource>().Play();
		++nextBuzz;
		if(nextBuzz == 4)
			nextBuzz = 0;
	}
	
	//Plays a random ambient noise every 10-20 seconds
	IEnumerator PlayAmbient()
	{
		float randomTime = Random.Range (35,45);
		yield return new WaitForSeconds(randomTime);
		int randomAmbient = Random.Range (4,8);
		while(randomAmbient == lastAmbient)
		{
			randomAmbient = Random.Range (4,8);
		}
		lastAmbient = randomAmbient;
		audioSources[4].GetComponent<AudioSource>().clip = sounds[randomAmbient];
		audioSources[4].GetComponent<AudioSource>().Play();
		StartCoroutine("PlayAmbient");
	}
	
	public void PlaySound(int soundIndex)
	{
		audioSources[nextSound].GetComponent<AudioSource>().clip = sounds[soundIndex];
		audioSources[nextSound].GetComponent<AudioSource>().Play ();
		++nextSound;
		if(nextSound == audioSources.Length)
			nextSound = 5;	
	}
}
