using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {
	
	/*
	 *      NETWORK PROTO
	 */
	 
	private HostData[] hostList;
	
	private const string gameType = "Nebul45";
	private const string gameName = "firstGame";
	
	
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
		if ( !Network.isClient && !Network.isServer ) {
			if ( GUI.Button(new Rect(100, 100, 250, 100), "Start Server") ) {
				startServer();
			}
			
			if ( GUI.Button(new Rect(100, 250, 250, 100), "Refresh Hosts") ) {
				refreshHostsList();
			}
			
			if ( hostList != null ) {
				for ( int i = 0; i < hostList.Length; i++ ) {
					if ( GUI.Button(new Rect(400, 100 + (110*i), 300, 100), hostList[i].gameName) ) {
						joinServer (hostList[i]);
					}
				}
			}
		}
	}
}
