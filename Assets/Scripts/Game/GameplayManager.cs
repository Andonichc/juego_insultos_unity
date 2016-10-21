using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;


public class GameplayManager : MonoBehaviour {

	private string[] answers; //All the answers available
	private Insult[] insults; //All the insults available
	private int current; // which insult is the game on

	private int userPoints, computerPoints; //scored points by the player and the computer

	public GameObject answersParent; //container in which we'll fill all the player's options
	public Text computerText; //text in which we'll show all the computer insults and answers

	public Text computerPointsText, playerPointsText;

	//audio source
	private AudioSource source;

	//audios
	private AudioClip userWinsRoundAudio;
	private AudioClip computerWinsRoundAudio;


	[System.Serializable]
	public class Insult {
		
		public string insultText;
		public int correctAnswer;
	}

	// Use this for initialization
	void Start () {

		//get the gameObjects and the transform from the scene
		answersParent = GameObject.Find("AnswersContainer");
		computerText = GameObject.Find ("ComputerResponseText").GetComponent<Text>();

		computerPointsText = GameObject.Find ("ComputerPoints").GetComponent<Text> ();
		playerPointsText = GameObject.Find ("PlayerPoints").GetComponent<Text> ();
		//get the answers and questions
		answers = GameFiller.LoadAnswers ();
		insults = GameFiller.loadInsults ();

		//audio variables
		source = GetComponent<AudioSource>();
		computerWinsRoundAudio = Resources.Load ("boo") as AudioClip;
		userWinsRoundAudio = Resources.Load ("applause") as AudioClip;
		//initializate the game variables
		userPoints = 0;
		computerPoints = 0;
		current = 0;
		int startTurn = Random.Range (0, 1);
		if (startTurn > 0) {
			Debug.Log ("Player starts!");
			Fillnsults ();
		} else {
			Debug.Log ("Computer starts!");
			StartCoroutine (SetRandomInsult ());
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	/*
	* USER PROCEDURES
 	*/


	/*
	*Called when the user selects an answer
	*/
	private void AnswerSelected (int index) {
		if (IsCorrect (index)) {
			PlayerWinsRound ();
		} else {
			ComputerWinsRound ();
		}
	}

	/*
	*Called when the user selects an insult
	*/
	private void InsultSelected(int index) {
		current = index;
		StartCoroutine (GetRandomAnswer());
	}

	/*
	 * COMPUTER FUNCTIONS
	 */

	/*
	* Called when the computer starts the turn, it selects a random insult to show the player
	*/
	private IEnumerator SetRandomInsult(){
		yield return new WaitForSeconds(1);

		current = Random.Range (0, insults.Length);
		ShowText (insults[current].insultText);
		FillAnswers ();
	}

	/*
	* Called when the player selects an insult, it selects a random answer to answer the player
	*/
	private IEnumerator GetRandomAnswer(){
		yield return new WaitForSeconds(1);

		int answerIndex = Random.Range (0, answers.Length);
		ShowText (answers[answerIndex]);

		yield return new WaitForSeconds(1);

		if (IsCorrect (answerIndex)) {
			ComputerWinsRound ();
		} else {
			PlayerWinsRound ();
		}
	}


	/**
	 * GAME STATE PROCEDURES AND FUNCTIONS
	 */

	/*When the computer wins the round it adds a point to the computer and checks if it has won 3 assaults
	* in that case the player has lost the game
	*/
	private void ComputerWinsRound(){
		Debug.Log ("Computer won round");
		computerPoints++;
		UpdateGamePoints ();

		source.PlayOneShot (computerWinsRoundAudio);

		if (computerPoints >= 3) {
			//DialogManager.ShowEndDialog (false, "Canvas");
			PlayerPrefs.SetString("winner", "computer");
			SceneManager.LoadScene ("FinalScene");

			Debug.Log ("Player has lost!");
			return;
		}
		StartCoroutine (SetRandomInsult());
	}

	/*When the player wins the round it adds a point to the player and checks if he has won 3 assaults
	* in that case the player has won the game
	*/
	private void PlayerWinsRound(){
		Debug.Log ("Player won round");
		userPoints++;
		UpdateGamePoints ();

		source.PlayOneShot (userWinsRoundAudio);

		if (userPoints >= 3) {
			//DialogManager.ShowEndDialog (true, "Canvas");
			PlayerPrefs.SetString("winner", "player");
			SceneManager.LoadScene ("FinalScene");

			Debug.Log ("Player wins!");
			return;
		}
		ShowText ("...");
		Fillnsults ();
	}

	/*
	 * Checks if the answer given is valid for the actual insult
	 */
	private bool IsCorrect(int index) {
		return insults[current].correctAnswer == index;
	}


	/*
	 * UI PROCEDURES
	 */

	private void FillAnswers() {

		//first we destroy the previous options
		foreach (Transform child in answersParent.transform) {
			Destroy (child.gameObject);
		}

		float height = answersParent.GetComponent<RectTransform>().rect.yMax - 20;

		for (int i = 0; i < answers.Length; i++) {
			//as response button is a prefab we can instantiate it as many times as we want
			GameObject answerButton =(GameObject) Instantiate (Resources.Load("ResponseButton"), new Vector3(0,height), new Quaternion(0,0,0,0));

			//atach it to the answers container
			answerButton.transform.SetParent(answersParent.transform, false);

			FillAnswerButton (answerButton, i);
			height += answerButton.GetComponent<RectTransform> ().rect.y - 20;
		}
	}

	private void FillAnswerButton(GameObject answerButton, int index){
		Text textUi = answerButton.GetComponentInChildren<Text> ();
		textUi.text = answers[index];

		answerButton.GetComponent<Button> ().onClick.AddListener (() => {
			AnswerSelected(index);
			textUi.color = Color.green;
		});
	}

	private void Fillnsults() {
		//first we destroy the previous options
		foreach (Transform child in answersParent.transform) {
			Destroy (child.gameObject);
		}

		float height = answersParent.GetComponent<RectTransform>().rect.yMax - 20;

		for(int i = 0; i < insults.Length; i++) {
			//as response button is a prefab we can instantiate it as many times as we want
			GameObject answerButton =(GameObject) Instantiate (Resources.Load("ResponseButton"), new Vector3(0,height), new Quaternion(0,0,0,0));

			//atach it to the answers container
			answerButton.transform.SetParent(answersParent.transform, false);

			FillButtonInsult (answerButton, i);

			height += answerButton.GetComponent<RectTransform> ().rect.y - 20;
		}
	}

	private void FillButtonInsult(GameObject answerButton, int index) {
		Text textUi = answerButton.GetComponentInChildren<Text> ();
		textUi.text = insults[index].insultText;

		answerButton.GetComponent<Button> ().onClick.AddListener (() => {
			InsultSelected (index);
			textUi.color = Color.green;
		});
	}
		
	/*
	 * Shows the given text into the computer "conversation" container
	 */
	private void ShowText(string text) {
		computerText.text = text;
	}
	/*
	 * Updates the Points text indicator for both, computer and player
	 */
	private void UpdateGamePoints(){
		computerPointsText.text = "Sir Arthur: " + computerPoints.ToString();
		playerPointsText.text = "You: " + userPoints.ToString();
	}
}
