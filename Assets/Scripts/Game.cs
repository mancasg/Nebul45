using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour {
	
	private GameStateMachine gameStateMachine = null;
	
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
	
		gameStateMachine = new GameStateMachine();
		
		
		gameStateMachine.PushState ( (GameState)(new PlayState()) );
	}
	
	/*
	 * Called once per frame
	 */
	void Update () {
		
		gameStateMachine.Update();
		
	}
	
	
	void OnGUI() {
		
		gameStateMachine.OnGUI();
		
	}
	
	public GameStateMachine GetGameStateMachine() {
		return gameStateMachine;
	}
}
