using UnityEngine;
using System.IO;



public class GameFiller{
	public static string[] LoadAnswers() {
		string answersString = LoadFileToString ("answers");

		AnswersFile answersFile = JsonUtility.FromJson<AnswersFile> (answersString);
		return answersFile.answers;
	}

	public static GameplayManager.Insult[] loadInsults(){
		string insultsString = LoadFileToString ("insults");

		InsultsFile insultsFile = JsonUtility.FromJson<InsultsFile> (insultsString);
		Debug.Log (insultsFile.insults.Length.ToString());
		return insultsFile.insults;
	}

	public static string LoadFileToString(string name)
	{
		TextAsset targetFile = Resources.Load<TextAsset>(name);

		return targetFile.text;
	}
		
	public class AnswersFile{
		public float version;
		public string[] answers;
	}

	public class InsultsFile{
		public float version;
		public GameplayManager.Insult[] insults;
	}
}

