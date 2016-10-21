using UnityEngine;
using System.Collections;

public class FinalScreenManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		string winner = PlayerPrefs.GetString ("winner");
		//Shows the dialog showing who won
		DialogManager.ShowEndDialog (winner.Equals("player"), "Canvas");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
