using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void exitGame(){
		Debug.Log ("Quitting the game!");
		Application.Quit ();
	}

	public void playGame() {
		Debug.Log ("Starting to play...");
		SceneManager.LoadScene ("GameSceneWithGraphics");
	}


}
