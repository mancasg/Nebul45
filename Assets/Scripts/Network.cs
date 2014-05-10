using UnityEngine;
using System.Collections;

public class Login {
	
	/*
	 *      NETWORK PROTO
	 */
	
	private HostData[] hostList;
	private string Type="Login";
	private const string gameType = "Nebul45";
	private const string gameName = "firstGame";
	private string Username="";
	private string Password="";
	
	/* 
	 * Start and initialise the server
	 */
	private void startServer() {
		Network.InitializeServer(8, 25000, !Network.HavePublicAddress());
		MasterServer.RegisterHost (gameType, gameName);
	}
	
	
	/*
	 *
	 */
	private void refreshHostsList() {
		MasterServer.RequestHostList (gameType);
	}
	
	
	/*
	 *
	 */
	private void joinServer(HostData hostData) {
		Network.Connect(hostData);
	}
	
	
	/*
	 * On server initialised message handler
	 */
	void OnServerInitialized() {
		Debug.Log("Server Initialised");
		(GameObject.Find("GameLogic")).AddComponent<Game>();
	}
	
	/*
	 * On connected to sever message handler
	 */
	void OnConnectedToServer() {
		Debug.Log("Server Joined");
		(GameObject.Find("GameLogic")).AddComponent<Game>();
	}	
	
	
	/*
	 * Master server event handler
	 */
	void OnMasterServerEvent(MasterServerEvent evt) {
		if ( evt == MasterServerEvent.HostListReceived ) {
			hostList = MasterServer.PollHostList();
		}
	}
	
	
	
	
	void OnGUI() {
		/* 
		 * If the user is neither a client nor a server
		 */		
		switch (Type) {
			
		case "Login":
			GUI.Box(new Rect(0,0,Screen.width,Screen.height),"");
			Username = GUI.TextField (new Rect (100, 80, 50, 20),Username,25);
			Password = GUI.PasswordField (new Rect (300,80,50,20),Password,"*"[0],25);
			if(GUI.Button(new Rect(100,100,100,50),"Login")){
				Type="Menu";
			}
			if(GUI.Button(new Rect(300,100,100,50),"Quit")){
			}
			if(GUI.Button(new Rect(500,100,100,50),"Register")){
				Type="Register";
			}
			break;
		case "Register":
			if(GUI.Button(new Rect(100,100,100,50),"Register")){
				Type="Menu";
			}
			if(GUI.Button(new Rect(300,100,100,50),"Cancel")){
				Type="Login";
			}
			break;
		case "Menu":
			if ( GUI.Button(new Rect(100, 250, 250, 100), "Refresh Hosts") ) {
				refreshHostsList();
			}
			
			if ( hostList != null ) {
				for ( int i = 0; i < hostList.Length; i++ ) {
					if ( GUI.Button(new Rect(400, 100 + (110*i), 300, 100),"Join to "+ hostList[i].gameName) ) {
						joinServer (hostList[i]);
					}
				}
			}
			break;
		}
		
	}
}

