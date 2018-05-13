using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorGenerator : MonoBehaviour {

	public GameObject WhiteTilePrefab;
	public GameObject BlackTilePrefab;
	public GameObject WallPrefab;

	public LayerMask mask = -1;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void GenerateFloor(int width, int length, int wallHeight) {
		this.Generate (width, length);
		this.AddBoundaryWalls (width, length, wallHeight);
	}

	private void AddBoundaryWalls(int width, int length, int height) {
		var walls = new GameObject("Floor Walls");

		// north wall
		var northWall = new GameObject ("North Wall");
		for (int y = 1; y < height; y++) {
			for (int x = -1; x < width; x++) {
				var position = new Vector3 (x, y, -1);
				var wallBlock = Instantiate (WallPrefab, position, Quaternion.identity, northWall.transform);
				wallBlock.GetComponent<Renderer> ().enabled = false;
			}
		}

		// south wall
		var southWall = new GameObject ("South Wall");
		for (int y = 1; y < height; y++) {
			for (int x = 0; x < width + 1; x++) {
				var position = new Vector3 (x, y, length);
				var wallBlock = Instantiate (WallPrefab, position, Quaternion.identity, southWall.transform);
				wallBlock.GetComponent<Renderer> ().enabled = false;		
			}
		}

		// east wall
		var eastWall = new GameObject ("East Wall");
		for (int y = 1; y < height; y++) {
			for (int z = 0; z < length + 1; z++) {
				var position = new Vector3 (-1, y, z);
				var wallBlock = Instantiate (WallPrefab, position, Quaternion.identity, eastWall.transform);
				wallBlock.GetComponent<Renderer>().enabled = false;
			}
		}

		// west wall
		var westWall = new GameObject ("West Wall");
		for (int y = 1; y < height; y++) {
			for (int z = -1; z < length; z++) {
				var position = new Vector3 (length, y, z);
				var wallBlock = Instantiate (WallPrefab, position, Quaternion.identity, westWall.transform);
				wallBlock.GetComponent<Renderer>().enabled = false;
			}
		}

		northWall.transform.SetParent (walls.transform);
		southWall.transform.SetParent (walls.transform);
		eastWall.transform.SetParent (walls.transform);
		westWall.transform.SetParent (walls.transform);

		walls.transform.SetParent (this.transform);
	}

	private void Generate(int width, int height) {

		var floorParent = new GameObject ("Floor");

		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				bool isWhite = (x+y) % 2 == 0;
				var tilePrefab = isWhite ? WhiteTilePrefab : BlackTilePrefab;
				var position = new Vector3 (x, 0, y);
				var tile = Instantiate (tilePrefab, position, Quaternion.identity, floorParent.transform);
			}
		}

		floorParent.transform.SetParent (this.transform);
	}
}
