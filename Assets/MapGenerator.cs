using UnityEngine;
using System.Collections;
using System;

public class MapGenerator : MonoBehaviour {

	// percent of map filled with wall
	[Range(0,100)]
	public int randomFillPercent;

	public int width, height;
	public string seed;
	public bool useRandomSeed;

	//2-D array of ints
	int [,] map;

	// Use this for initialization
	void Start () {
	 	// initiialize map
		map = new int[width,height];
		GenerateMap ();
		
	}

	void Update() {
		if (Input.GetMouseButtonDown(0)) {
			GenerateMap ();
		}
	}

	void SmoothMap() {
		for (int x = 0; x < width; x ++) {
			for (int y = 0; y < height; y ++) {
				int neighbourWallTiles = GetSurroundingWallCount(x,y);

				if (neighbourWallTiles > 4)
					map[x,y] = 1;
				else if (neighbourWallTiles < 4)
					map[x,y] = 0;

			}
		}
	}

	void GenerateMap(){
		RandomFillMap ();

		for (int i = 0; i < 5; i ++) {
			SmoothMap();
		}

		MeshGenerator meshGen = GetComponent<MeshGenerator>();
		meshGen.GenerateMesh(map, 1);

	}

	int GetSurroundingWallCount(int gridX, int gridY) {
		int wallCount = 0;
		for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX ++) {
			for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY ++) {
				if (neighbourX >= 0 && neighbourX < width && neighbourY >= 0 && neighbourY < height) {
					if (neighbourX != gridX || neighbourY != gridY) {
						wallCount += map[neighbourX,neighbourY];
					}
				}
				else {
					wallCount ++;
				}
			}
		}

		return wallCount;
	}

	void RandomFillMap(){
		if (useRandomSeed) {
			seed = Time.time.ToString ();
		}

		System.Random rand = new System.Random (seed.GetHashCode ());
	
		for(int i=0; i< width ; i++)
			for(int j=0; j< height ; j++)
			{
				if (i == 0 || i == width - 1 || j == 0 || j == height - 1)
					map [i, j] = 1;
				else
					map [i, j] = (rand.Next (0, 100) < randomFillPercent)?1:0;
			}
	}

	void roshDisableOnDrawGizmos(){
		if (map != null)
			for(int i=0; i< width ; i++)
				for(int j=0; j< height ; j++)
				{
					Gizmos.color = (map [i, j] == 1) ? Color.black : Color.white;
					Vector3 pos = new Vector3(-width/2 + i + 0.5f, -height/2 + j + 0.5f);
					Gizmos.DrawCube(pos,Vector3.one);
				}
	}
}
