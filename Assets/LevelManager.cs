using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LevelManager : MonoBehaviour {

	private LevelGenerator LevelGenerator;

	public float DropDelay;
	public float DropDelayVariance;

	public int NumRows;
	private int RowCounter;

	private bool IsDropping;
	private float TimeUntilDrop;
	private int DropIndex;

	private List<PieceParent> Pieces = new List<PieceParent>();

	// Use this for initialization
	void Start () {
		this.RowCounter = 0;
		this.LevelGenerator = GetComponentInChildren<LevelGenerator>();
		this.StartNextRow ();
	}

	void StartNextRow() {
		var newPieces = this.LevelGenerator.GenerateLevel ().Select (piece => piece.GetComponent<PieceParent> ());
		newPieces.ToList().ForEach(piece => piece.SetLevelManager (this));

		this.Pieces.AddRange (newPieces);
		this.StartDrop ();
	}

	void StartDrop() {
		this.IsDropping = true;
		this.DropIndex = 0;
	}

	// Called by pieces when they have finished falling to check for layer completion
	public void HandlePieceFell(PieceParent piece) {
		this.CheckForRowCompletion (piece.GetRowIndex());
	}

	private void CheckForRowCompletion(int rowIndex) {
		var rowPieces = this.Pieces.Where (piece => piece.GetRowIndex () == rowIndex);
		bool complete = rowPieces.All (piece => piece.CannotMove ());

		if (complete) {
			rowPieces.ToList ().ForEach (piece => piece.RowComplete ());
		}
	}

	float GetDropDelay() {
		return Random.Range (DropDelay - DropDelayVariance, DropDelay + DropDelayVariance);
	}

	void DropNext() {
		if (this.DropIndex >= Pieces.Count) {
			IsDropping = false;

			this.StartNextRow ();
			return;
		}
		
		this.TimeUntilDrop = GetDropDelay ();

		var piece = Pieces [DropIndex];
		var controller = piece.GetComponent<PieceParent> ();
		controller.Begin ();

		this.DropIndex++;
	}
	
	// Update is called once per frame
	void Update () {
		if (IsDropping) {
			if (TimeUntilDrop <= 0) {
				this.DropNext ();
			} else {
				TimeUntilDrop -= Time.deltaTime;
			}
		}
	}
}
