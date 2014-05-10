using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour {
	
	/* 
	 * GameObject's prefabs
	 */
	public GameObject prefabDbgCube;
	public GameObject prefabPlayer;
	
	/* 
	 * Player already instantiated in the scene. 
	 * Composed of ship model, lights, camera ...
	 */
	private GameObject player;
	private Camera	   playerCamera;
	
	/* 
	 * Spawn points already created in the scene and hierarchy
	 */
	private GameObject spawnPoints;
	
	
	private const int GAME_AREA_WIDTH = 400;
	private const int GAME_AREA_HEIGHT = 200;
	private const int GAME_AREA_LENGTH = 400;
	
	/* ======================================== SINGLETON TEST ==== */
	
	private static Game instance = null;
	private Game() {
	}
	public static Game Instance() {
		if ( instance == null ) {
			instance = GameObject.Find("GameLogic").AddComponent<Game>();
		}
		return instance;
	}
	
	
	/*
	 * Initialisations
	 */
	void Start () {
		/*
		 * Prevent creation of other monobehaviours
		 */
		if ( instance == null ) {	
			instance = this;
		}
		else {
			Destroy(this);
			return;
		}
		
		Application.runInBackground = true;
	
		/* TEST ================= */
		prefabDbgCube = (GameObject)Resources.Load("dbgCube");
		prefabPlayer = (GameObject)Resources.Load("spaceshipOne");
		/* ============================================================= */
		
		spawnPoints = GameObject.Find("SpawnPoints");
		
		/*
		 * Instantiate the player and set his position to the first
		 * spawn point.
		 */
		player = (GameObject)Network.Instantiate(prefabPlayer,
		                                 spawnPoints.transform.GetChild(0).transform.position,
		                                 spawnPoints.transform.GetChild(0).transform.rotation,
		                                 0
		                                 );
        
        /*
         * Make the MainCamera the player's camera and make it follow the player
         * by using the SmoothFollow script
         */            
		playerCamera = GameObject.FindGameObjectWithTag("MainCamera").camera;
		playerCamera.gameObject.AddComponent<SmoothFollow>();
		SmoothFollow smoothFollowScript = playerCamera.gameObject.GetComponent<SmoothFollow>();
		smoothFollowScript.target = player.transform;
		smoothFollowScript.height = 3.0f;
		
		
		
		instantiateCubes ();
	}
	
	/*
	 * Called once per frame
	 */
	void Update () {
		
		/*
		 * Respawn the player if he exits the game area
		 */
		if ( Mathf.Abs(player.transform.position.x) > GAME_AREA_LENGTH/2 ||
		    Mathf.Abs(player.transform.position.y) > GAME_AREA_HEIGHT/2 ||
		    Mathf.Abs(player.transform.position.z) > GAME_AREA_WIDTH/2
		) 
		{
			player.transform.position = spawnPoints.transform.GetChild(0).transform.position;
		}
		
		
	}
	
	/* 
	 * Place some cubes inside the game area
	 * for testing purposes
	 */
	void instantiateCubes() {

		for (int height = -1 * (GAME_AREA_HEIGHT/2); height < (GAME_AREA_HEIGHT/2); height += 20) {
			for ( int length = -1 * (GAME_AREA_LENGTH/2); length < (GAME_AREA_LENGTH/2); length += 40 ) {
				for ( int width = -1 * (GAME_AREA_WIDTH/2); width < (GAME_AREA_WIDTH/2); width += 40 ) {
					Instantiate (prefabDbgCube,
					             new Vector3(width, height, length),
					             new Quaternion()
					             );
				}
			}
		}

	}	
	
	
	void OnGUI() {
		
		/* Crosshair code */
		/* !!! must reposition */
		Texture crosshair = (Texture)Resources.Load("Textures/crosshair");
		
		Vector3 horizon = player.transform.position;
		horizon += player.transform.forward * 500.0f;
		Vector3 screenPos = playerCamera.WorldToScreenPoint(horizon);
		
		/*
		GUIUtility.RotateAroundPivot( -1 * player.transform.rotation.eulerAngles.z,
		                              new Vector2(screenPos.x - 25, Screen.height - screenPos.y + 25)
		                             );
		*/
		
		GUI.DrawTexture(new Rect(screenPos.x - 25, Screen.height - screenPos.y + 25, 50, 50),
						crosshair,
						ScaleMode.ScaleToFit,
						true,
						0.0f
						);
		/* end of crosshair code */
		
	}
	
}
