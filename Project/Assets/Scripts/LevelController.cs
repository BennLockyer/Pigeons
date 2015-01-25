using UnityEngine;
using System.Collections;

public class LevelController : MonoBehaviour 
{
	public GameObject player1;
	public GameObject player2;
	[HideInInspector]public Vector3 player1checkpoint;
	[HideInInspector]public Vector3 player2checkpoint;
	public GameObject player1finish;
	public GameObject player2finish;
	
	void Start () 
	{
		player1checkpoint = player1.transform.position;
		player2checkpoint = player2.transform.position;
	}
	
	public void UpdateCheckpoint(int player, Vector3 position)
	{
		if(player == 1)
			player1checkpoint = position;
		if(player ==2)
			player2checkpoint = position;
	}
	
	public void CheckGame()
	{
		if(player1finish.GetComponent<TriggerController>().inUse &&
			player2finish.GetComponent<TriggerController>().inUse)
			{
				//You have beaten the level!
				Debug.Log ("Level complete");
				Application.LoadLevel(0);
			}
	}
}
