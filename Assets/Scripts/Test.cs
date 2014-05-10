using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {
	private string Username;
	private string Password;
	private string UserDB;
	private bool Corect;


	void Start () {
		Username = "";
		Password = "";
		Corect=false;
	}

	void Update () {
	
	}

	IEnumerator WaitForRequest(WWW www)
	{
		yield return www;
		
		// check for errors
		if (www.error == null){
			string [] DataBase=www.text.Replace('{',' ').Replace('}',' ').Replace('"',' ').Trim().Split(',');
			Debug.Log(DataBase[1]);
		} 
		else {
			Debug.Log("WWW Error: "+ www.error);
		}
	}

	void OnGUI(){
		GUI.Box(new Rect(0,0,Screen.width,Screen.height),"");
		Username = GUI.TextField (new Rect (100, 80, 50, 20),Username,25);
		Password = GUI.PasswordField (new Rect (300,80,50,20),Password,"*"[0],25);
		if(GUI.Button(new Rect(100,100,100,50),"Login")){

			WWW www = new WWW ("https://nebul45.firebaseio.com/.json");
			StartCoroutine (WaitForRequest(www));
			/*
			if(DataBase.IndexOf("Costel")==null){
				Debug.Log("Username sau Parola Incorecte");
			}
			else{
				DataBase.
			}*/

		}
		if(GUI.Button(new Rect(300,100,100,50),"Quit")){
			Application.Quit();
		}
		if(GUI.Button(new Rect(500,100,100,50),"Register")){
			
		}
	}
}
