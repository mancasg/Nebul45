using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour{
	
	private float speed = 80.0f;
	private float maxDistance = 200.0f;
	
	private float currentDistance = 0.0f;
	
	
	/*
	 * Initialisations
	 */
	void Start () {
		//this.gameObject.rigidbody.AddForce(this.gameObject.transform.forward * speed);
	}
	
	/*
	 * Called once per frame
	 */
	void Update () {
		
		if ( currentDistance > maxDistance ) {
			Destroy(this.gameObject);
		}
		
		currentDistance += speed * Time.deltaTime;
		
		this.gameObject.transform.Translate(
			Vector3.forward *
			speed *
			Time.deltaTime
		);	
	}
}
