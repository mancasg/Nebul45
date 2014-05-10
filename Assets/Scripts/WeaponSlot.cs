using UnityEngine;
using System.Collections;

public class WeaponSlot : MonoBehaviour {
	
	private Weapon missileLauncher;
	
	/*
	 * Initialisations
	 */
	void Start () {
		missileLauncher = new MissileLauncher(this.transform, "prefabMissile");
	}
	
	/*
	 * Called once per frame
	 */
	void Update () {
		
		if ( Input.GetKeyDown(KeyCode.Space) ) {
			missileLauncher.fire();
		}
	}
}
