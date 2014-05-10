using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameStateMachine {
	
	private List<GameState> gameStates;
	
	public GameStateMachine () {
		gameStates = new List<GameState>();
	}
	
	public void Update () {
		for ( int i = 0; i < gameStates.Count; i++ ) {
			gameStates[i].Update();
		}
	}
	
	public void OnGUI() {
		for ( int i = 0; i < gameStates.Count; i++ ) {
			gameStates[i].OnGUI();
		}
	}
	
	public void PushState( GameState __state ) {
		gameStates.Add(__state);
		gameStates[0].Start();
	}
	
	public void PopState() {
		gameStates.RemoveAt(gameStates.Count - 1);
	}
	
	public void ReplaceState( GameState __state ) {
		gameStates.RemoveAt(gameStates.Count - 1);
		gameStates.Add(__state);
		__state.Start();
	}
}
