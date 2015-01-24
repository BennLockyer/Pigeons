using UnityEngine;
using System.Collections;
using UnityEditor;

public class LevelEditor : EditorWindow 
{
	[MenuItem("GameJam/Level Editor")]
	
	public static void ShowWindow()
	{
		EditorWindow.GetWindow(typeof(LevelEditor));
	}
	
	private int[,] index = new int[16,16];
	
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
	
	private int xOffset;
	private int yOffset;
	
	private bool checkClear = false;
	private bool checkGrid = false;
	private bool checkGenerate = false;
	
	private string warningText = "This is a message";
	private GameObject tile;
	
	void OnEnable()
	{
		for(int x = 0; x < 16; ++x)
		{
			for(int y = 0; y < 16; ++y)
			{
				index[x,y] = EditorPrefs.GetInt("index" + x.ToString() + y.ToString(),0);
			}
		}
		
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
		tile = Resources.Load<GameObject>("Cube");
	}
	
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
	
	void OnGUI()
	{
		for(int y = 15; y > -1; --y)
		{
			GUILayout.BeginHorizontal();
			for(int x = 0; x < 16; ++x)
			{
				if(GUILayout.Button(materials[index[x,y]].GetTexture("_MainTex"),GUILayout.Width(35),GUILayout.Height(35)))
				{
					Event e = Event.current;
					
					if(e.button == 0)
					{
						++index[x,y];
						if(index[x,y] == materials.Length)
							index[x,y] = 0;
					}
					
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
		GUILayout.BeginHorizontal();
		xOffset = EditorGUILayout.IntField("x offset: ",xOffset,GUILayout.Width(200));
		yOffset = EditorGUILayout.IntField("y offset: ",yOffset,GUILayout.Width(200));
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		if(!checkClear && !checkGrid && !checkGenerate)
		{
			if(GUILayout.Button("Generate",GUILayout.Width(100),GUILayout.Height(30)))
			{
				checkGenerate = true;
				warningText = "Generate the grid into the game?";
			}
			
			
			if(GUILayout.Button("Clear Grid",GUILayout.Width(100),GUILayout.Height(30)))
			{
				checkGrid = true;
				warningText = "Are you sure you want to clear the grid?";
			}
			if(GUILayout.Button("Clear Scene",GUILayout.Width(100),GUILayout.Height(30)))
			{
				checkClear = true;
				warningText = "Are you sure you want to clear the scene?";
			}
		}
		else
		{
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
						rotation = 180;
						break;
					case 2:
						modelName = "model_corner";
						rotation = 270;
						break;
					case 3:
						modelName = "model_corner";
						break;
					case 4:
						modelName = "model_corner";
						rotation = 90;
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
						rotation = 90;
						break;
					case 8:
						modelName = "model_t";
						rotation = 180;
						break;
					case 9:
						modelName = "model_t";
						rotation = 270;
						break;
					case 10:
						modelName = "model_t";
						break;
					case 11:
						modelName = "model_cross";
						break;
					case 12:
						modelName = "model_end";
						rotation = 180;
						break;
					case 13:
						modelName = "model_end";
						rotation = 270;
						break;
					case 14:
						modelName = "model_end";
						break;
					case 15:
						modelName = "model_end";
						rotation = 90;
						break;
					}
					GameObject t = Resources.Load<GameObject>("Tiles/" + modelName);
					GameObject u = (GameObject)Instantiate (t,new Vector3(x+(16*xOffset),0,y+(16*yOffset)),Quaternion.identity);
					u.transform.eulerAngles = new Vector3(0,rotation,0);
//					GameObject t = (GameObject)Instantiate(tile,new Vector3(x+(16*xOffset),0,y+(16*yOffset)),Quaternion.identity);
//					t.renderer.material = materials[index[x,y]];
				}
			}
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
