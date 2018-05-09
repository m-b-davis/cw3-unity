using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorGenerator : MonoBehaviour {

	public GameObject WhiteTilePrefab;
	public GameObject BlackTilePrefab;

	public int Width;
	public int Height;

	// Use this for initialization
	void Start () {
		this.Generate ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public void Generate() {
		int counter = 0;
		for (int x = 0; x < Width; x++) {
			for (int y = 0; y < Height; y++) {
				bool isWhite = counter % 2 == 0;
				var tilePrefab = isWhite ? WhiteTilePrefab : BlackTilePrefab;
				var position = new Vector3 (x, 0, y);
				var tile = Instantiate (tilePrefab, position, Quaternion.identity);
				counter++;
			}
		}
	}
}
