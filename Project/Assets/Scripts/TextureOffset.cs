using UnityEngine;
using System.Collections;

public class TextureOffset : MonoBehaviour 
{
	public float scrollSpeed;
	
	void Update () 
	{	
		float offset = scrollSpeed * Time.time;
		renderer.material.SetTextureOffset("_MainTex",new Vector2(offset,0));
	}
}
