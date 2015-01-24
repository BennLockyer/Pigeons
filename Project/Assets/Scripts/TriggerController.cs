using UnityEngine;
using System.Collections;

public class TriggerController : MonoBehaviour 
{
	//What type of trigger are we?
	public enum TriggerType {EndGame, Pressure, Toggle, Button, Portal}
	public TriggerType triggerType;
	
	//Who can use this trigger?
	public enum TriggerPlayer {Player1, Player2, BothPlayers}
	public TriggerPlayer triggerPlayer;
	
	//Can we be used?
	public bool canUse = true;
	//Are we currently being used?
	public bool inUse = false;

	//If we are a portal, this is where we want to go	
	public Transform targetPortal;
	
	//What objects will be affected by the trigger?
	public Transform[] targetObjects;
	
	//We enter the collider
	void OnTriggerEnter(Collider other)
	{
		//See if the current player is able to use this trigger
		if((triggerPlayer == TriggerPlayer.Player1 && other.gameObject.tag == "Player1") ||
			(triggerPlayer == TriggerPlayer.Player2 &&  other.gameObject.tag == "Player2") ||
			(triggerPlayer == TriggerPlayer.BothPlayers && other.gameObject.tag.Contains("Player")))
		{
			//Toggle triggers switch on/off when pressed
			if(triggerType == TriggerType.Toggle)
			{
				inUse = !inUse;
				for(int x = 0; x < targetObjects.Length; ++x)
				{
					targetObjects[x].GetComponent<DropObject>().Toggle();
				}
			}
			else
			{
				//Everything else becomes active when entered
				inUse = true;
				//If this is a portal, let's use it!
				if(triggerType == TriggerType.Portal && canUse)
				{
					StartCoroutine("ActivatePortal",other.gameObject);
				}
				//If this is the end position, let's check if the other player is done, too!
				if(triggerType == TriggerType.EndGame)
				{
					GameObject levelController = GameObject.Find ("_LevelController");
					levelController.GetComponent<LevelController>().CheckGame();
				}
			}
			if(canUse && inUse && (triggerType == TriggerType.Button || triggerType == TriggerType.Pressure))
			{
				for(int x = 0; x < targetObjects.Length; ++x)
				{
					targetObjects[x].GetComponent<DropObject>().Toggle();
				}
				if(triggerType == TriggerType.Button)
					canUse = false;
			}
		}
	}
	
	//We exit the trigger
	void OnTriggerExit(Collider other)
	{
		//Check to see if we had activated the trigger
		if((triggerPlayer == TriggerPlayer.Player1 && other.gameObject.tag == "Player1") ||
		   (triggerPlayer == TriggerPlayer.Player2 &&  other.gameObject.tag == "Player2") ||
		   (triggerPlayer == TriggerPlayer.BothPlayers && other.gameObject.tag.Contains("Player")))
		{
			//Any pressure/end game triggers are turned off when left
			if(triggerType == TriggerType.Pressure)
			{
				inUse = false;
				for(int x = 0; x < targetObjects.Length; ++x)
				{
					targetObjects[x].GetComponent<DropObject>().Toggle();
				}
			}
			
			if(triggerType == TriggerType.EndGame)
			{
				inUse = false;
			}
		}
	}
	
	IEnumerator ActivatePortal(GameObject player)
	{
		//Show some particles
		yield return new WaitForSeconds(0.5f);
		//Make sure the other portal can't sent us straight back
		targetPortal.GetComponent<TriggerController>().canUse = false;
		//Send the player over
		player.transform.position = new Vector3(targetPortal.transform.position.x,2.3f,targetPortal.transform.position.z);
		//Wait a short moment
		yield return new WaitForSeconds(0.5f);
		//Activate the target portal again (in case the player wants to come back)
		targetPortal.GetComponent<TriggerController>().canUse = true;
	}
}
