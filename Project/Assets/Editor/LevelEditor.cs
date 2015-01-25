using UnityEngine;
using System.Collections;
using UnityEditor;

public class LevelEditor : EditorWindow 
{
	//Make the menu heading
	[MenuItem("GameJam/Level Editor")]
	
	//Show the window
	public static void ShowWindow()
	{
		EditorWindow.GetWindow(typeof(LevelEditor));
	}
	
	//Make an array for our grid in the editor
	private int[,] index = new int[16,16];
	
	//Create a bunch of materials for drawing on the buttons
	private Material empty = null;
	private Material corner = null;
	private Material corner90 = null;
	private Material corner180 = null;
	private Material corner270 = null;
	private Material straight = null;
	private Material straight90 = null;
	private Material threeway = null;
	private Material threeway90 = null;
	private Material threeway180 = null;
	private Material threeway270 = null;
	private Material cross = null;
	private Material deadend = null;
	private Material deadend90 = null;
	private Material deadend180 = null;
	private Material deadend270 = null;
	private Material[] materials = new Material[16];
	
	//Create some offsets - this lets us align more rooms if it needs to be bigger than 16x16
	private int xOffset;
	private int yOffset;
	
	//Check to see if our buttons are being used -- this makes sure we don't make changes by accident
	private bool checkClear = false;
	private bool checkGrid = false;
	private bool checkGenerate = false;
	
	//Some text which shows what's happening
	private string warningText = "This is a message";
		
	//When the screen opens
	void OnEnable()
	{
		//Load an index based on button position in the grid
		for(int x = 0; x < 16; ++x)
		{
			for(int y = 0; y < 16; ++y)
			{
				index[x,y] = EditorPrefs.GetInt("index" + x.ToString() + y.ToString(),0);
			}
		}
		
		//Load all of the textures from the resources/materials folder
		empty = Resources.Load<Material>("Materials/m_empty");
		corner = Resources.Load<Material>("Materials/m_corner");
		corner90 = Resources.Load<Material>("Materials/m_corner90");
		corner180 = Resources.Load<Material>("Materials/m_corner180");
		corner270 = Resources.Load<Material>("Materials/m_corner270");
		straight = Resources.Load<Material>("Materials/m_straight");
		straight90 = Resources.Load<Material>("Materials/m_straight90");
		threeway = Resources.Load<Material>("Materials/m_3way");
		threeway90 = Resources.Load<Material>("Materials/m_3way90");
		threeway180 = Resources.Load<Material>("Materials/m_3way180");
		threeway270 = Resources.Load<Material>("Materials/m_3way270");
		cross = Resources.Load<Material>("Materials/m_cross");
		deadend = Resources.Load<Material>("Materials/m_deadend");
		deadend90 = Resources.Load<Material>("Materials/m_deadend90");
		deadend180 = Resources.Load<Material>("Materials/m_deadend180");
		deadend270 = Resources.Load<Material>("Materials/m_deadend270");
		//Assign the materials into an array
		materials[0] = empty;
		materials[1] = corner;
		materials[2] = corner90;
		materials[3] = corner180;
		materials[4] = corner270;
		materials[5] = straight;
		materials[6] = straight90;
		materials[7] = threeway;
		materials[8] = threeway90;
		materials[9] = threeway180;
		materials[10] = threeway270;
		materials[11] = cross;
		materials[12] = deadend;
		materials[13] = deadend90;
		materials[14] = deadend180;
		materials[15] = deadend270;
	}
	
	//When we close the window we save the index so that the layout is the same when we open it
	void OnDisable()
	{
		for(int x = 0; x < 16; ++x)
		{
			for(int y = 0; y < 16; ++y)
			{	
				EditorPrefs.SetInt("index" + x.ToString() + y.ToString(),index[x,y]);
			}
		}
	}
	
	//Draw stuff!
	void OnGUI()
	{
		//Draw the buttons
		for(int y = 15; y > -1; --y)
		{
			GUILayout.BeginHorizontal();
			for(int x = 0; x < 16; ++x)
			{
				//See if we've pressed a button
				if(GUILayout.Button(materials[index[x,y]].GetTexture("_MainTex"),GUILayout.Width(35),GUILayout.Height(35)))
				{
					Event e = Event.current;
					//Left click counts forward through index (images)
					if(e.button == 0)
					{
						++index[x,y];
						if(index[x,y] == materials.Length)
							index[x,y] = 0;
					}
					//Right click counts backwards
					if(e.button == 1)
					{
						--index[x,y];
						if(index[x,y] < 0)
							index[x,y] = materials.Length -1;
					}
				}
			}
			GUILayout.EndHorizontal();
		}
		//Draw an area for us to enter our offset values (1 = a single 16x16 gridspace
		GUILayout.BeginHorizontal();
		xOffset = EditorGUILayout.IntField("x offset: ",xOffset,GUILayout.Width(200));
		yOffset = EditorGUILayout.IntField("y offset: ",yOffset,GUILayout.Width(200));
		GUILayout.EndHorizontal();
		//Draw more buttons
		GUILayout.BeginHorizontal();
		//If we're not confiring anything, draw the 'generate', 'clear grid' and 'clear scene' buttons
		if(!checkClear && !checkGrid && !checkGenerate)
		{
			if(GUILayout.Button("Generate",GUILayout.Width(100),GUILayout.Height(30)))
			{
				//Create the prefabs
				checkGenerate = true;
				warningText = "Generate the grid into the game?";
			}
			
			
			if(GUILayout.Button("Clear Grid",GUILayout.Width(100),GUILayout.Height(30)))
			{
				//Clear the grid in the editor
				checkGrid = true;
				warningText = "Are you sure you want to clear the grid?";
			}
			if(GUILayout.Button("Clear Scene",GUILayout.Width(100),GUILayout.Height(30)))
			{
				//Destroy all of the tiles in the level
				checkClear = true;
				warningText = "Are you sure you want to clear the scene?";
			}
		}
		else
		{
			//If we have clicked a button, replace them with confirmation buttons
			if(GUILayout.Button("YES",GUILayout.Width(100),GUILayout.Height(30)))
			{
				if(checkGrid)
				{
					checkGrid = false;
					ClearGrid();
					warningText = "The grid has been cleared.";
				}
				if(checkClear)
				{
					checkClear = false;
					ClearScene();
					warningText = "The scene has been cleared.";
				}
				if(checkGenerate)
				{
					checkGenerate = false;
					GenerateMap();
					warningText = "The grid has been generated.";
				}
			}
			
			if(GUILayout.Button("NO",GUILayout.Width(100),GUILayout.Height(30)))
			{
				checkGrid = false;
				checkClear = false;
				checkGenerate = false;
				warningText = "No changes have been made.";
			}
		}
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUI.color = Color.yellow;
		GUILayout.Label(warningText,GUILayout.Width(400));
		
//		if(GUILayout.Button("LETS DO THIS"))
//		{
//			LetsHopeThisWorks();
//		}
	}
	
	void GenerateMap()
	{
		for(int y = 15; y > -1; --y)
		{
			for(int x = 0; x < 16; ++x)
			{
				if(index[x,y] > 0)
				{
					string modelName = "";
					int rotation = 0;
					switch(index[x,y])
					{
					case 1:
						modelName = "model_corner";
						rotation = 270;
						break;
					case 2:
						modelName = "model_corner";
						rotation = 0;
						break;
					case 3:
						modelName = "model_corner";
						rotation = 90;
						break;
					case 4:
						modelName = "model_corner";
						rotation = 180;
						break;
					case 5:
						modelName = "model_straight";
						rotation = 90;
						break;
					case 6:
						modelName = "model_straight";
						break;
					case 7:
						modelName = "model_t";
						rotation = 270;
						break;
					case 8:
						modelName = "model_t";
						rotation = 0;
						break;
					case 9:
						modelName = "model_t";
						rotation = 90;
						break;
					case 10:
						modelName = "model_t";
						rotation = 180;
						break;
					case 11:
						modelName = "model_cross";
						break;
					case 12:
						modelName = "model_end";
						rotation = 0;
						break;
					case 13:
						modelName = "model_end";
						rotation = 90;
						break;
					case 14:
						modelName = "model_end";
						rotation = 180;
						break;
					case 15:
						modelName = "model_end";
						rotation = 270;
						break;
					}
					GameObject t = Resources.Load<GameObject>("Tiles/" + modelName);
					GameObject u = (GameObject)Instantiate (t,new Vector3((x*5)+(80*xOffset),0,(y*5)+(80*yOffset)),Quaternion.identity);
					u.transform.eulerAngles = new Vector3(0,rotation,0);
					u.transform.parent = GameObject.Find ("_Tiles").transform;
//					GameObject t = (GameObject)Instantiate(tile,new Vector3(x+(16*xOffset),0,y+(16*yOffset)),Quaternion.identity);
//					t.renderer.material = materials[index[x,y]];
				}
			}
		}
	}
	
	void LetsHopeThisWorks()
	{
		GameObject[] myTiles = GameObject.FindGameObjectsWithTag("Tile");
		foreach(GameObject t in myTiles)
		{
			string name = t.name.Remove(t.name.Length - 7);
			GameObject p = Resources.Load<GameObject>("Tiles/" + name);
			GameObject i = (GameObject)Instantiate (p, t.transform.position,Quaternion.identity);
			i.transform.eulerAngles = t.transform.eulerAngles;
			i.transform.parent = t.transform;
			t.renderer.enabled = false;
		}
	}
	
	void ClearGrid()
	{	
		for(int x = 0; x < 16; ++x)
		{
			for(int y = 0; y < 16; ++y)
			{
				index[x,y] = 0;
			}
		}
	}
	
	void ClearScene()
	{
		GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");
		foreach(GameObject t in tiles)
		{
			DestroyImmediate(t);
		}
	}
}
