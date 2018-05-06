using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {

	public int Width;
	public int Length;

	public GameObject PiecePrefab;
	public GameObject PieceParent;

	// Use this for initialization
	void Start () {
		GenerateLevel ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
		
	void GenerateLevel() {
		var row = GenerateRow (this.Width, this.Length, 4, 4);
		int yPos = 4;
		this.InstantiateRow (row, yPos);
	}

	void InstantiateRow(Walk[] row, int yPos) {
		foreach (var walk in row) {
			var walkParent = new GameObject ();
			walkParent.AddComponent<Rigidbody> ();

			foreach (int[] coord in walk.Coords) {
				var position = new Vector3 (coord [0], yPos,coord [1]);
				Instantiate (this.PiecePrefab, position, Quaternion.identity, walkParent.transform);
			}

			walkParent.transform.SetParent (PieceParent.transform);
		}
	}

	int[] PickStartPoint (int[,] row, int blank) {
		int i1 = Random.Range (0, row.GetLength(0) - 1);
		int i2 = Random.Range (0, row.GetLength(1) - 1);

		if (row [i1, i2] == blank) {
			return new int[]{ i1, i2 };
		} else {
			// Could hit recursion depth here
			return this.PickStartPoint (row, blank);
		}
	}
//
//	int[] DoWalkStep(int[][] row, int i1, int i2, int walkSymbol) {
//
//
//		int[] randomMove = validMoves [Random.Range (0, validMoves.Length - 1)];
//
//		int i1Next = randomMove [0];
//		int i2Next = randomMove [1];
//
//		row [i1Next] [i2Next] = walkSymbol;
//
//		return row;
//	}

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

	Walk[] GenerateRow(int width, int length, int numWalks, int walkLength) {
		int[,] row = new int[width, length];

		var walks = new Walk[numWalks];

		int blankSymbol = 0;

		for (int walkIndex = 0; walkIndex < numWalks; walkIndex++) {
			int walkSymbol = walkIndex + 1;
			int[] currentPosition = this.PickStartPoint (row, blankSymbol);
			Walk walk = new Walk (currentPosition, walkSymbol);
				
			for (int w = 0; w < walkLength; w++) {
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

			walks [walkIndex] = walk;
		}

		return walks;
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

