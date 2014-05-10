using UnityEngine;
using System.Collections;

public abstract class GameState {

	public abstract void Start ();
	
	public abstract void Update ();
	
	public abstract void OnGUI();
}
