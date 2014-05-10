using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	private const float minSpeed = -10.0f;
	private const float maxNormalSpeed = 20.0f;
	private const float maxBoostSpeed = 50.0f;
	
	private const float deltaAcceleration = 10.0f;
	private const float deltaBoostAcceleration = 15.0f;
	
	private bool afterBoostDeceleration = false;

	private float acceleration = 0.0f;
	private float speed = 0.0f;			// velocity actually 
	
	private GameObject leftEngine;
	private GameObject rightEngine;

	/* 
	 * Members used for synchronization
	 */
	private float lastSyncTime = 0.0f;
	private float syncDelay = 0.0f;						// time since last synchronization
	private float syncTime = 0.0f;						// time since beggining of synchronization
	private Vector3 syncStartPosition = Vector3.zero;
	private Vector3 syncEndPosition = Vector3.zero;
	
	/*
	 * Weapon system
	 */
	private Weapon missileLauncher;
	
	/*
	 * Initialisations
	 */
	void Start () {	
		
		leftEngine  = this.transform.Find("EngineLeft").gameObject;
		rightEngine = this.transform.Find("EngineRight").gameObject;
		
		missileLauncher = new MissileLauncher(this.transform.Find("WeaponSlot").transform, "prefabBullet");
	
	}
	
	/*
	 * Called once per frame
	 */
	void Update () {
		if ( networkView.isMine ) {
			movementHandler();
			
			if ( Input.GetKeyDown(KeyCode.Space) ) {
				missileLauncher.fire();
			}
		}
		else {
			syncedMovement();
		}
	}
	
	/*
	 * Handle movement input information and
	 * modify the player's position
	 */
	private void movementHandler() {
		float horiz = Input.GetAxis ("Horizontal");	// control z-rotation
		float vert  = Input.GetAxis ("Vertical");	// control x-rotation
		
		bool boostOn = Input.GetKey (KeyCode.LeftShift);
		
		/* 
		 * Update the z-rotation depending on the horizontal axis
		 * input. 
		 */
		if (horiz != 0.0) {
			horiz *= -1;
			this.gameObject.transform.Rotate(
				(new Vector3(0.0f, 0.0f, 1.0f)) * horiz * 120 * Time.deltaTime
				);
			
		} 
		
		/* 
		 * Update the x-rotation depending on the horizontal axis
		 * input. 
		 */
		if ( vert != 0 ) {
			this.gameObject.transform.Rotate(
				(new Vector3(1.0f, 0.0f, 0.0f)) * vert * 120 * Time.deltaTime
				);
		}
		
		/* 
		 * Modify the engine particle output depending on
		 * the boost mode
		 */
		if ( Input.GetKeyDown(KeyCode.LeftShift) ) {
			leftEngine.particleSystem.startColor  = Color.blue;
			rightEngine.particleSystem.startColor = Color.blue;
		}
		if ( Input.GetKeyUp(KeyCode.LeftShift) ) {
			leftEngine.particleSystem.startColor  = Color.red;
			rightEngine.particleSystem.startColor = Color.red;
			
			afterBoostDeceleration = true;
		}
		
		/*
		 * Fixed acceleration depending on whether
		 * the boost is on or not
		 */
		if (boostOn) {
			acceleration = deltaBoostAcceleration;
		}
		else {
			acceleration = deltaAcceleration;
		}
		
		
		/* 
		 * If the acceleration is not zero calculate
		 * the speed depending on it and delta time
		 * else
		 * decelerate if the speed is not zero
		 */
		if (acceleration != 0) {
			speed += acceleration * Time.deltaTime;
		} else if (acceleration == 0.0f && speed != 0.0f) {
			float ammount = deltaAcceleration * Time.deltaTime;
			if ( Mathf.Abs(speed) <= ammount ) {
				speed = 0.0f;
			}
			else if (speed > 0.0f) {
				speed -= ammount;
			}
			else {
				speed += ammount;
			}
		}
		
		/*
		 * Limit the speed depending on wether the
		 * boost is on or not
		 */
		if ( boostOn ) {
			if (speed > maxBoostSpeed) {
				speed = maxBoostSpeed;
			}	
		}
		else {
			if (speed > maxNormalSpeed) {
				if ( afterBoostDeceleration ) {
					float ammount = deltaBoostAcceleration * Time.deltaTime;
					speed -= ammount;
				}
				else {
					speed = maxNormalSpeed;
				}
			}
			else if ( afterBoostDeceleration ) {
				afterBoostDeceleration = false;
			}				
		}
		if (speed < minSpeed) {
			speed = minSpeed;
		}
		
		
		/*
		 * Update the position depending on the speed
		 * and deltaTime
		 */
		if (speed != 0.0f) {
			this.gameObject.transform.Translate(
				Vector3.forward *
				speed *
				Time.deltaTime
				);
		}
	}
	
	/*
	 * Synchronize movement by interpolating over the current position 
	 * and the last sync position received over the network
	 */
	private void syncedMovement() {
		syncTime += Time.deltaTime;
		rigidbody.position = Vector3.Lerp(syncStartPosition, 
										  syncEndPosition,
										  syncTime / syncDelay
										  );
	}
	
	/*
	 * Send position information over the network through a bit stream
	 * in order to synchronize the state of the game
	 */
	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info) {
		Vector3 syncPosition = Vector3.zero;
		Vector3 syncSpeed = Vector3.zero;
		bool	syncBoost = false;
		if ( stream.isWriting ) {
			// Write player position and speed on the stream
			syncPosition = rigidbody.position;
			stream.Serialize(ref syncPosition);
			
			syncSpeed = (new Vector3(1.0f, 1.0f, 1.0f)) * speed;
			stream.Serialize(ref syncSpeed);
			
			syncBoost = Input.GetKeyDown(KeyCode.LeftShift);
			stream.Serialize(ref syncBoost);
		}
		else {
			// Get other player positions
			stream.Serialize(ref syncPosition);
			stream.Serialize(ref syncSpeed);
			
			syncTime = 0.0f;
			syncDelay = Time.time - lastSyncTime;
			lastSyncTime = Time.time;
			
			syncStartPosition = rigidbody.position;
			syncEndPosition = syncPosition + syncDelay * syncSpeed;
			
			stream.Serialize(ref syncBoost);
			if ( syncBoost ) {
				leftEngine.particleSystem.startColor  = Color.blue;
				rightEngine.particleSystem.startColor = Color.blue;
			}
			else {
				leftEngine.particleSystem.startColor  = Color.red;
				rightEngine.particleSystem.startColor = Color.red;
			}
		}
		
	}
}
