using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class DialogManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void onClickGoToMenu(){
		Debug.Log ("To menu");
		SceneManager.LoadScene ("MenuScene");
	}

	public void onClickPlayAgain() {
		Debug.Log ("To Game");
		SceneManager.LoadScene ("GameSceneWithGraphics");
	}

	/*
	 * Shows a modal with the result of the game and gives the player the possibility to play again or to return to the main menu
	 */
	public static void ShowEndDialog(bool playerWon, string gameObjectReference){
		GameObject dialogEndGame =(GameObject) Instantiate (Resources.Load("DialogEndGame"), new Vector3(0,0), new Quaternion(0,0,0,0));

		dialogEndGame.transform.SetParent(GameObject.Find(gameObjectReference).transform, false);
		string endText;
		if (playerWon) {
			endText = "Has Ganado!";
		} else {
			endText = "Has Perdido :(";
		}
		dialogEndGame.GetComponentInChildren<Text> ().text = endText;
	}
}
