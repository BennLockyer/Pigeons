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
		
		empty = Resources.Load<Material>("m_empty");
		corner = Resources.Load<Material>("m_corner");
		corner90 = Resources.Load<Material>("m_corner90");
		corner180 = Resources.Load<Material>("m_corner180");
		corner270 = Resources.Load<Material>("m_corner270");
		straight = Resources.Load<Material>("m_straight");
		straight90 = Resources.Load<Material>("m_straight90");
		threeway = Resources.Load<Material>("m_3way");
		threeway90 = Resources.Load<Material>("m_3way90");
		threeway180 = Resources.Load<Material>("m_3way180");
		threeway270 = Resources.Load<Material>("m_3way270");
		cross = Resources.Load<Material>("m_cross");
		deadend = Resources.Load<Material>("m_deadend");
		deadend90 = Resources.Load<Material>("m_deadend90");
		deadend180 = Resources.Load<Material>("m_deadend180");
		deadend270 = Resources.Load<Material>("m_deadend270");
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
		
		if(GUILayout.Button("Generate",GUILayout.Width(100),GUILayout.Height(30)))
		{
			GenerateMap();
		}
		
		GUILayout.BeginHorizontal();
		if(GUILayout.Button("Clear Grid",GUILayout.Width(100),GUILayout.Height(30)))
		{
			ClearGrid();
		}
		if(GUILayout.Button("Clear Scene",GUILayout.Width(100),GUILayout.Height(30)))
		{
			ClearScene();
		}
	}
	
	void GenerateMap()
	{
		for(int y = 15; y > -1; --y)
		{
			for(int x = 0; x < 16; ++x)
			{
				if(index[x,y] > 0)
				{
					GameObject t = (GameObject)Instantiate(tile,new Vector3(x,0,y),Quaternion.identity);
					t.renderer.material = materials[index[x,y]];
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
