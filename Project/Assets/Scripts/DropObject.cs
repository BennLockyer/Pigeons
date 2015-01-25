using UnityEngine;
using System.Collections;

public class DropObject : MonoBehaviour 
{
	private float startTime;
	private float distance;
	
	private Vector3 startPos;
	private Vector3 endPos;
	private Vector3 openPos;
	private Vector3 closedPos;
	private Vector3 lastPos;
	
	private bool isMoving;
	
	public float movementSpeed;
	
	void Start()
	{
		if(transform.position.y > 0)
		{
			closedPos = transform.position;
			openPos = new Vector3(transform.position.x, transform.position.y - 6.0f, transform.position.z);
			lastPos = closedPos;
		}
		else
		{
			openPos = transform.position;
			closedPos = new Vector3(transform.position.x, transform.position.y + 6.0f, transform.position.z);
			lastPos = openPos;
		}
	}
	
	void Update()
	{
		if(isMoving)
		{
			float covered = (Time.time - startTime) * movementSpeed;
			float perc = covered/distance;
			transform.position = Vector3.Lerp (startPos,endPos,perc);
			
			if(transform.position == endPos)
			{
				isMoving = false;
			}
		}
	}
	
	public void Toggle()
	{
		Debug.Log ("Last: " + lastPos.ToString());
		if(lastPos == closedPos)
		{
			Close ();
		}
		else if(lastPos == openPos)
		{
			Open ();
		}
	}
	
	void Close()
	{
		startPos = transform.position;
		endPos = openPos;
		lastPos = openPos;
		startTime = Time.time;
		distance = Vector3.Distance(startPos,endPos);
		isMoving = true;
	}
	
	void Open()
	{
		startPos = transform.position;
		endPos = closedPos;
		lastPos = closedPos;
		startTime = Time.time;
		distance = Vector3.Distance(startPos,endPos);
		isMoving = true;
	}
}
