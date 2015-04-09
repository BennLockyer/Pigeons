using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelController : MonoBehaviour 
{
	public GameObject player1;
	public GameObject player2;
	[HideInInspector]public Vector3 player1checkpoint;
	[HideInInspector]public Vector3 player2checkpoint;
	public GameObject player1finish;
	public GameObject player2finish;

	public RectTransform[] uiGameInstructions;
	
	void Start () 
	{
		player1checkpoint = player1.transform.position;
		player2checkpoint = player2.transform.position;

		foreach(RectTransform gameUI in uiGameInstructions)
			LeanTween.textAlpha (gameUI, 0.0f, 2.0f).setDelay(5.0f);
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
				Kebab ();
			}
	}
	
	IEnumerator Kebab()
	{
		yield return new WaitForSeconds(1.0f);
		Application.LoadLevel(0);
	}
}
