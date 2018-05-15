using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

	private static int DropPieceScore = 10;
	private static int CompleteRowScore = 500;

	private LevelGenerator LevelGenerator;

    public AudioSource gameOverAudio, completeAudio;

    public float DropDelay;
	public float DropDelayVariance;

	public int NumRows;
	private int RowCounter;

	private bool IsDropping;
	private float TimeUntilDrop;
	private int DropIndex;

	public Text ScoreCounter;
	public Text DifficultyCounter;
	public Text GoalIndicator;
	public Text HeightIndicator;

	private float difficultyIncreaseRate = 1f; // 1 = normal
	private float difficulty = 0;
	private int goalBlocks = 0;
	private int score;

	private int currentHeight = 0;

	private PlayerController Player;

	private bool freezeInput { 
		get { return Player.FreezeInput; } 
		set { Player.FreezeInput = value;} 
	}

	private BlurController BlurController;

	private List<PieceParent> Pieces = new List<PieceParent>();

	// Use this for initialization
	void Start () {
		this.Player = FindObjectOfType<PlayerController> ();
		this.LevelGenerator = GetComponentInChildren<LevelGenerator>();
		this.IncreaseScore (0);
		this.IncreaseDifficulty (1);
		this.DropIndex = 0;
		this.RowCounter = 0;
		this.BlurController = FindObjectOfType<BlurController> ();
		this.BlurController.HideBlur ();
		this.StartNextRow ();
		this.CheckRowHeight ();
	}

	void StartNextRow() {
		var singleTileProb = DifficultyManager.GetVariable (DifficultyVariable.SingleBlockProbability, difficulty);
		var maxWalkLength = DifficultyManager.GetVariable (DifficultyVariable.MaxWalkLength, difficulty);
			
		var piecesGameObjects = this.LevelGenerator.GenerateLevelRow (singleTileProb, Mathf.RoundToInt(maxWalkLength));
		var newPieces = piecesGameObjects.Select (piece => piece.GetComponent<PieceParent> ());

		newPieces.ToList().ForEach(piece => piece.SetLevelManager (this));

		this.Pieces.AddRange (newPieces);
		this.StartDrop ();
	}

	void StartDrop() {
		this.IsDropping = true;
	}

	private void IncreaseScore(int amount) {
		score += amount;
		IncreaseDifficulty (0.05f);

		this.ScoreCounter.text = string.Format ("SCORE: {0}", score);
	}

	private void IncreaseDifficulty(float amount) {
		if (difficulty > 1 && Mathf.RoundToInt(difficulty) % 3 == 0 && Mathf.Floor (difficulty + amount) > Mathf.Floor (difficulty)) { // sauce
			LevelGenerator.ExpandSizeByOne ();
		}
		difficulty += amount;

		var completionThreshold = DifficultyManager.GetVariable (DifficultyVariable.RowCompleteThreshold, difficulty);
		var maxBlocks = LevelGenerator.Width * LevelGenerator.Length;
		var maxHeight = DifficultyManager.GetVariable (DifficultyVariable.MaxHeight, difficulty);

		goalBlocks = (int)Mathf.Floor (completionThreshold * maxBlocks);

		this.GoalIndicator.text = string.Format ("GOAL: {0} BLOCKS/LAYER", goalBlocks);
		this.DifficultyCounter.text = string.Format ("DIFFICULTY: {0:0.0}", difficulty); 


	}

	void OnEnable()
	{
		//Tell our 'OnLevelFinishedLoading' function to start listening for a scene change as soon as this script is enabled.
		SceneManager.sceneLoaded += OnLevelWasLoaded;
	}

	void OnDisable()
	{
		//Tell our 'OnLevelFinishedLoading' function to stop listening for a scene change as soon as this script is disabled.
		//Remember to always have an unsubscription for every delegate you
		SceneManager.sceneLoaded -= OnLevelWasLoaded;
	}


	void OnLevelWasLoaded(Scene scene, LoadSceneMode mode) {
		if (UnityEditor.Lightmapping.giWorkflowMode == UnityEditor.Lightmapping.GIWorkflowMode.Iterative) {
			DynamicGI.UpdateEnvironment();
		}
	}



	// Called by pieces when they have finished falling to check for layer completion
	public void HandlePieceFell(PieceParent piece) {
		
		this.CheckRowHeight ();
		this.CheckForRowCompletion (piece.GetRowIndex());
		this.IncreaseScore (DropPieceScore);
	}

	private void CheckForRowCompletion(int rowIndex) {
		var rowPieces = this.Pieces.Where (piece => piece.GetRowIndex () == rowIndex);
		var numBlocks = rowPieces.Sum (piece => piece.NumBlocks);
		bool complete = numBlocks >= goalBlocks;

		if (complete) {
            completeAudio.Play();
			rowPieces.ToList ().ForEach (piece => piece.RowComplete ());
			IncreaseScore (CompleteRowScore);
			CheckRowHeight (-1);
		}
	}

	public void CheckRowHeight(int offset = 0) {
		this.currentHeight = Pieces.Count == 0 ? 0 : Pieces.Max (p => {
			if (p == null)
				return 0;

			if (p.CanCrush)
				return 0;

			return p.GetRowIndex ();
		}) + offset;

		var maxHeight = Mathf.FloorToInt (DifficultyManager.GetVariable (DifficultyVariable.MaxHeight, difficulty));

		if (currentHeight > maxHeight) {
			this.PlayerDied (CauseOfDeath.BlockHeightReached);
		} else if (currentHeight == maxHeight) {
			HeightIndicator.color = new Color(0.8f, 0.0f, 0.0f);
        } else if (currentHeight == maxHeight - 1) {
			HeightIndicator.color = new Color(0.4f, 0.4f, 0.0f);
        } else {
			HeightIndicator.color = new Color(0.0f, 0.8f, 0.0f);
		}

		this.HeightIndicator.text = string.Format ("HEIGHT: {0}  MAX: {1}", currentHeight, Mathf.FloorToInt(maxHeight)); 
	}

	void DropNext() {
		if (this.DropIndex >= Pieces.Count) {
			IsDropping = false;

			this.StartNextRow ();
			return;
		}
		
		this.TimeUntilDrop = DifficultyManager.GetVariable (DifficultyVariable.DropDelay, difficulty);
		var dropSpeed = DifficultyManager.GetVariable (DifficultyVariable.DropSpeed, difficulty);

		var piece = Pieces [DropIndex];
		var controller = piece.GetComponent<PieceParent> ();
		controller.Begin (dropSpeed);

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

		this.IncreaseDifficulty ((Time.deltaTime / 20) * difficultyIncreaseRate);
	}

	private bool dead = false;

	public void PlayerDied(CauseOfDeath cause) {
        gameOverAudio.Play();
        this.freezeInput = true;
		if (!dead) {
			dead = true;
			BlurController.ShowBlur ();
			FindObjectOfType<GameOverController> ().Show (score, cause);
		}
	}

	public void PlayerCrushed() {
		this.PlayerDied (CauseOfDeath.Crushed);
	}
}

public enum CauseOfDeath {
	Crushed,
	Checkmate,
	BlockHeightReached
}

public enum DifficultyVariable {
	MaxWalkLength,
	SingleBlockProbability,
	DropSpeed,
	DropDelay,
	RowCompleteThreshold,
	MaxHeight
}

public static class DifficultyManager {
	public static Dictionary<DifficultyVariable, float[]> DifficultyRanges = new Dictionary<DifficultyVariable, float[]> {
		{ DifficultyVariable.MaxWalkLength, new float[] { 2f, 7f } },
		{ DifficultyVariable.SingleBlockProbability, new float[] { 0.5f, 0.2f } },
		{ DifficultyVariable.DropSpeed, new float[] { 4f, 10f } },
		{ DifficultyVariable.DropDelay, new float[] { 5f, 1f } },
		{ DifficultyVariable.RowCompleteThreshold, new float[] { 0.45f, 0.8f } },
		{ DifficultyVariable.MaxHeight, new float[] { 4, 8 }}
	};

	public static int MinDifficulty = 1;
	public static int MaxDifficulty = 15;

	public static float GetVariable(DifficultyVariable name, float difficulty) {
		var range = DifficultyRanges [name];
		var percentage = difficulty / MaxDifficulty;
		return Mathf.Lerp (range [0], range [1], percentage);
	}
}
