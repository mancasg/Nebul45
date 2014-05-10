using UnityEngine;
using System.Collections;

public class Register {

	private string Username;
	private string Password;
	private string ConfirmPassword;

	public void Start(){
		Username="";
		Password="";
		ConfirmPassword="";
	}

	public void Update(){
	}

	public void OnGUI(){
		GUI.Label(new Rect(100,100,100,100),"Username:");
		GUI.Label(new Rect(100,300,100,100),"Password:");
		GUI.Label(new Rect(100,500,100,100),"Confirm Password:");
		Username=GUI.TextField(new Rect(300,100,100,100),Username,25);
		Password = GUI.PasswordField (new Rect (300,300,100,100),Password,"*"[0],25);
		Password = GUI.PasswordField (new Rect (300,500,100,100),ConfirmPassword,"*"[0],25);
		if(GUI.Button(new Rect(100,100,100,50),"Register")){

		}
		if(GUI.Button(new Rect(300,100,100,50),"Cancel")){

		}
		if(GUI.Button(new Rect(20,100,100,50),"Quit")){
			Application.Quit();
		}
	}
}

