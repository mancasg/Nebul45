using UnityEngine;
using System.Collections;

public class LoginState:GameState {

	
	private string Username;
	private string Password;
	private string cacatoare;
	private string pisatoare;

	public void Start(){
		Username = "";
		Password = "";
	}

	public void Update(){
	
	}

	public void OnGUI(){
		GUI.Box(new Rect(0,0,Screen.width,Screen.height),"");
		Username = GUI.TextField (new Rect (100, 80, 50, 20),Username,25);
		Password = GUI.PasswordField (new Rect (300,80,50,20),Password,"*"[0],25);
		if(GUI.Button(new Rect(100,100,100,50),"Login")){

		}
		if(GUI.Button(new Rect(300,100,100,50),"Quit")){
			Application.Quit();
		}
		if(GUI.Button(new Rect(500,100,100,50),"Register")){

		}
	}
}

