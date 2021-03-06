﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {

	public int Width;
	public int Length;
	public int NumWalks;
	public int WalkLength;

	public GameObject[] BlockPrefabs;
	public GameObject LevelWorld;
	public GameObject WalkParent;

	public FloorGenerator FloorGenerator;
	private float singleTileProbability;

	// Use this for initialization
	void Start () {
		var player = FindObjectOfType<PlayerController> ();
		player.SpawnX = Random.Range (0, Width - 1);
		player.SpawnZ = Random.Range (0, Length - 1);

		FloorGenerator.GenerateFloor (Width, Length, wallHeight: 10);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ExpandSizeByOne() {
		Width++;
		Length++;
		FloorGenerator.ExpandByOne();
	}
		
	public GameObject[] GenerateLevelRow(float singleTileProbability, int maxWalkLength) {
		this.singleTileProbability = singleTileProbability;
		var row = GenerateRow (this.Width, this.Length, this.NumWalks, maxWalkLength);
		int yPos = 20;
		return this.InstantiateRow (row, yPos);
	}

	GameObject[] InstantiateRow(Walk[] row, int yPos) {
		var pieces = new GameObject[row.Length];

		for (int walkIndex = 0; walkIndex < row.Length; walkIndex++) {
			var walk = row[walkIndex];
			var walkParent = Instantiate (WalkParent, LevelWorld.transform);

			int prefabIndex = (walk.Symbol - 1) % this.BlockPrefabs.Length; // wrap around available colours
			var prefab = this.BlockPrefabs [prefabIndex];
				
			foreach (int[] coord in walk.Coords) {
				var position = new Vector3 (coord [0], yPos,coord [1]);
				var block = Instantiate (prefab, position, Quaternion.identity, walkParent.transform);
			}

			walkParent.transform.SetParent (LevelWorld.transform);
			pieces [walkIndex] = walkParent;
		}

		return pieces;
	}

	int[] PickStartPoint (int[,] row, int blank, int recursionDepth = 10) {

		if (recursionDepth == 0) {
			return new int[] { -999, -999 };
		}

		int i1 = Random.Range (0, row.GetLength(0));
		int i2 = Random.Range (0, row.GetLength(1));

		if (row [i1, i2] == blank) {
			return new int[]{ i1, i2 };
		} else {
			return this.PickStartPoint (row, blank, recursionDepth - 1);
		}
	}
		
	bool InRange (int val, int min, int max) {
		return min <= val && val <= max;
	}

	int[][] GetValidMoves(int[,] row, int i1, int i2, int blankSymbol) {
		var allOffsets = new int[][] {
			new int [] { 0, 1 },
			new int [] { 1, 0 },
			new int [] { 0, -1 },
			new int [] { -1, 0 }
		};
			
		var allMoves = allOffsets.Select (offset => new int[] { i1 + offset [0], i2 + offset [1] });

		var validMoves = allMoves.Where (move => this.IsValidMove (row, move[0], move[1], blankSymbol));

		return validMoves.ToArray ();
	}

	bool IsValidMove(int[,] row, int i1, int i2, int blank){
		int i1Max = row.GetLength (0) - 1;
		int i2Max = row .GetLength(1) - 1;

		bool i1InRange = this.InRange (i1, 0, i1Max);
		bool i2InRange = this.InRange (i2, 0, i2Max);

		if (!(i1InRange && i2InRange)) {
			return false;
		}

		return row [i1, i2] == blank;
	}

	int GetWalkLength(int maxWalkLength) {
		float random = Random.Range (0f, 1f);

		if (random < this.singleTileProbability)
			return 1;

		if (random > 0.8) 
			return Random.Range (3, maxWalkLength);
	
		return Random.Range(1, Mathf.RoundToInt(maxWalkLength/ 2));
	}

	Walk[] GenerateRow(int width, int length, int numWalks, int maxWalkLength) {
		int[,] row = new int[width, length];

		var walks = new List<Walk> ();

		int blankSymbol = 0;

		for (int walkIndex = 0; walkIndex < numWalks; walkIndex++) {
			int walkSymbol = walkIndex + 1;
			int[] currentPosition = this.PickStartPoint (row, blankSymbol);
			if (currentPosition [0] == -999) {
				return walks.ToArray ();
			}

			row [currentPosition [0], currentPosition [1]] = walkSymbol;

			int walkLength = GetWalkLength (maxWalkLength);

			Walk walk = new Walk (currentPosition, walkSymbol);
			for (int w = 1; w < walkLength; w++) {
				int[][] validMoves = this.GetValidMoves (row, currentPosition[0], currentPosition[1], blankSymbol);

				if (validMoves.Length > 0) {
					int[] randomMove = validMoves [Random.Range (0, validMoves.Length - 1)];

					int i1Next = randomMove [0];
					int i2Next = randomMove [1];

					row [i1Next, i2Next] = walk.Symbol;

					walk.AddCoord (randomMove);
					currentPosition = randomMove;
				}
			}

			walks.Add (walk);
		}

		return walks.ToArray ();
	}

	struct LevelRow {
		List<Walk> Walks;
	}

	struct Walk {
		private List<int[]> coords;
		public int Symbol;

		public Walk(int[] startCoord, int symbol) {
			this.coords = new List<int[]>();
			this.Symbol = symbol;
			this.AddCoord(startCoord);
		}

		public void AddCoord(int[] coord) {
			coords.Add(coord);
		}

		public int[][] Coords {
			get { return (int[][])coords.ToArray (); }	
		}
	}
}

