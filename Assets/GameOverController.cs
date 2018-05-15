using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverController : MonoBehaviour {

	public Text GameOverText;
	public Text ScoreText;
	public Text CauseOfDeathText;
	public GameObject TextBackgroundPanel;

	private Dictionary<CauseOfDeath, string> CauseStrings = new Dictionary<CauseOfDeath, string> {
		{ CauseOfDeath.Crushed, "CRUSHED"},
		{ CauseOfDeath.Checkmate, "CHECKMATE"},
		{ CauseOfDeath.BlockHeightReached, "MAX BLOCK HEIGHT HIT"}
	};

	// Use this for initialization
	void Start () {
		SetTextVisible (false);
	}

	void SetTextVisible(bool visible) {
		GameOverText.enabled = visible;
		ScoreText.enabled = visible;
		CauseOfDeathText.enabled = visible;
		TextBackgroundPanel.gameObject.SetActive (visible);
	}


	public void Show(int score, CauseOfDeath cause) {
		SetTextVisible (true);
		ScoreText.text = string.Format ("SCORE: {0}", score);
		CauseOfDeathText.text = "CAUSE OF DEATH: " + CauseStrings [cause];
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
