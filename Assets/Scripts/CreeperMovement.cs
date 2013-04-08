using UnityEngine;
using System.Collections;
using System;

[RequireComponent (typeof(CharacterController)), RequireComponent (typeof(TrailRenderer)), RequireComponent (typeof(AudioSource))]
public class CreeperMovement : MonoBehaviour
{
	
	//public String gameObjectName;
	GameObject creeper;
	CharacterController controller;
	//Vector3 creeperOrigPosition;
	//Vector3 cube1Position;
	//Vector3 destination;
	//float weight = 0.0f;
	//float step = 0.01f;
	private Vector3 moveDirection = Vector3.zero;
	private Vector3 forward = Vector3.zero;
	//private Vector3 right = Vector3.zero;
	
	//float step = 5.0f;
	//int direction = 1;
	//DateTime lastUpdate;
	public GameObject[] waypoints;
	//Vector3[] creeperPositions;
	int currentWaypoint; //lastWaypoint;
	bool collided;
	bool grabbed;
	bool canBeGrabbed;
	string msg;
	public float PLAYER_PROXIMITY;
	public GameObject player;
	public float velocity;
	bool chaseMode;
	public float chaseVelocity;
	public float chaseMaxVelocity;
	public float chaseVelocityIncrementValue;
	public float rotationVelocity;
	public float WAYPOINT_DIST;
	DateTime enabledTrailTime;
	DateTime lastHitTime;
	bool trailDisplay;
	public float TRAIL_SHOW_TIME_SECS;
	public GameObject pathLine;
	public float CREEPER_HIT_DELTA;
	bool canEnableTrailDisplay;
	
	// Use this for initialization
	void Start ()
	{
		this.chaseMode = false;
		this.canEnableTrailDisplay = false;
		if (this.rotationVelocity == 0) {
			this.rotationVelocity = 10;
		}
		
		if (this.velocity == 0) {
			this.velocity = 0.2f; //default
		}
		
		//this.gameObject.GetComponent<TrailRenderer> ().enabled = false;
		this.trailDisplay = false;
		this.pathLine.renderer.enabled = false;
		
		this.controller = this.gameObject.GetComponent<CharacterController> ();
		this.controller.isTrigger = true; //apparently can't set this in the inspector
		this.grabbed = false;
		this.canBeGrabbed = true;
		this.creeper = this.gameObject;

		if (this.creeper.name.Contains ("Star")) {
			this.msg = "VICTORY!!";
		} else {
			this.msg = "LOSS";
		}
		//this.creeperOrigPosition = this.creeper.transform.position;
		//this.lastUpdate = System.DateTime.Now;

		//this.creeperPositions = new Vector3[5];
		//this.creeperPositions [0] = GameObject.Find ("Cube1").transform.position;
		//this.creeperPositions [1] = GameObject.Find ("Cube2").transform.position;
		//this.creeperPositions [2] = GameObject.Find ("Cube3").transform.position;
		//this.creeperPositions [3] = GameObject.Find ("Cube4").transform.position;
		//this.creeperPositions [4] = GameObject.Find ("Cube5").transform.position;
		
		//this.lastWaypoint = 0;
		this.currentWaypoint = 0;
		
		//disable collisions between the two colliders on this gameobject
		//Collider collider = GetComponent<CapsuleCollider>();
		//Physics.IgnoreCollision(this.controller,collider);
		
	}
	
	// Update is called once per frame
	void Update ()
	{
	
		if (this.collided) {
			DateTime now = DateTime.Now;
			TimeSpan diff = now - this.lastHitTime;
			if (diff.Seconds > CREEPER_HIT_DELTA) {
				this.collided = false;
			}
			
		}
		if (!this.grabbed) { //easy way to "pause"
			
			//creeper character controller initialization
			this.forward = this.creeper.transform.forward;
			//this.right = new Vector3(this.forward.z, 0, -this.forward.x);
			
			
			//DateTime cur = System.DateTime.Now;
			//TimeSpan diff = cur - this.lastUpdate;
			Vector3 destination;
			float playerDistance = Vector3.Distance (this.creeper.transform.position, this.player.transform.position);
			if (playerDistance < PLAYER_PROXIMITY) {
				//Debug.Log ("Creeper near!!");
				//this.creeper.transform.LookAt (this.player.transform.position); //turns to face the player immediately... kinda creepy!
				if (!audio.isPlaying) {
					audio.Play();
				}
				destination = this.player.transform.position;
				this.gameObject.GetComponent<TrailRenderer> ().enabled = false;
				this.pathLine.renderer.enabled = false;
				if (!this.chaseMode) {
					//beginning chase...
					this.chaseVelocity = this.velocity;
				}
				this.chaseMode = true;
			} else {
				audio.Pause();
				this.chaseMode = false;
				this.gameObject.GetComponent<TrailRenderer> ().enabled = true;
				updateDest ();
				destination = this.waypoints [this.currentWaypoint].transform.position;
				
				int maxWaypointArrayIndex = this.waypoints.Length - 1;
				
				if (this.currentWaypoint != maxWaypointArrayIndex) {
					this.canEnableTrailDisplay = true;
				}
				
				if ((!this.trailDisplay) && (this.currentWaypoint == maxWaypointArrayIndex) && (this.canEnableTrailDisplay)) {
					
					this.canEnableTrailDisplay = false;
					//this.gameObject.GetComponent<TrailRenderer>().enabled = true;
					this.enabledTrailTime = DateTime.Now;
					this.trailDisplay = true;
					this.pathLine.renderer.enabled = true;
				}
				
				DateTime now = DateTime.Now;
				TimeSpan diff = now - this.enabledTrailTime;
				if (this.trailDisplay && (diff.Seconds > TRAIL_SHOW_TIME_SECS)) {
					this.gameObject.GetComponent<TrailRenderer> ().enabled = false;
					this.trailDisplay = false;
					this.pathLine.renderer.enabled = false;
				}
				
			}
			//if (Time.deltaTime * 1000 > 25) {
			//this.lastUpdate = cur;
			//updateDest ();
			
			Vector3 currentPosition = this.creeper.transform.position;
			
			Vector3 travelDirection = destination - currentPosition;
				
			Vector3 moveDirection = Vector3.RotateTowards (this.moveDirection, travelDirection, (200 * Mathf.Deg2Rad) + 180 * Time.deltaTime, this.rotationVelocity);
 
			//rotate our creeper
			if (travelDirection != Vector3.zero) {
				this.creeper.transform.rotation = Quaternion.LookRotation (moveDirection);
			}
				
			//move our creeper
			Vector3 movement = moveDirection * Time.deltaTime * this.velocity;
			movement.Normalize ();
			float curVelocity = this.velocity;
			if (this.chaseMode) {
				this.chaseVelocity += this.chaseVelocityIncrementValue;
				if (this.chaseVelocity > this.chaseMaxVelocity) {
					this.chaseVelocity = this.chaseMaxVelocity;
				}
				curVelocity = this.chaseVelocity;
				
			}
			curVelocity *= Time.deltaTime;
			movement *= curVelocity;
			this.controller.Move (movement);
			
			
			//Vector3 lastLocation = this.waypoints[lastWaypoint].transform.position;
			//interpolate movement between original position and cube position
			//float x = weight * destination.x + (1 - weight) * lastLocation.x;
			//float y = this.yValue;
			//float z = weight * destination.z + (1 - weight) * lastLocation.z;
				
			//float x = currentPosition.x + travelDirection.x * this.step;
			//float y = this.yValue;
			//float z = currentPosition.z + travelDirection.z * this.step;
			
			//Vector3 newPosition = new Vector3 (x, y, z);		
			//this.creeper.transform.position = newPosition;
				
			//}
			
			//check proximity to player
		
		}
	}
			
	void updateDest ()
	{
		
		Vector3 currentPosition = this.creeper.transform.position;
		Vector3 destination = this.waypoints [currentWaypoint].transform.position;
		
		float distance = Vector3.Distance (currentPosition, destination);
		
		if (distance < WAYPOINT_DIST) { //TODO check value
			//if (this.weight >= 1.0) {
			//this.weight = 0;
			//this.lastWaypoint = this.currentWaypoint;
			this.currentWaypoint = (this.currentWaypoint + 1) % this.waypoints.Length;
			//this.destination = this.creeperPositions [this.currentCube];
			//this.direction = -1;
		} else {
			//Debug.Log ("Not close enough to switch waypoint" + this.creeper.gameObject.name + ", dist: " + distance);
			//if (this.weight <= 0.0) {
			//		this.direction = 1;
			//}
			//this.weight += this.direction * this.step;
			//this.weight += this.step;
		}
	}
	
	void OnCollisionEnter (Collision collision)
	{
		Debug.Log ("CREEPER MOVEMENT oncollisionenter");
		if (collision.gameObject.tag == "Creeper") {
			//this.msg = "Creeper collision";
			//this.collided = true;
		}
	}
	
	void OnControllerColliderHit (ControllerColliderHit hit)
	{
		//Debug.Log ("OnControllerColliderHit");
		
		//Debug.Log ("CREEPER MOVEMENT HIT " + hit.gameObject.name);
		if ((hit.gameObject.tag.Equals ("Player")) && !this.collided) {
			Debug.Log ("CREEPER MOVEMENT HIT PLAYER!!");
			//LIZZY: SCREEN SHAKE!!!! :)
			this.msg = "Creeper collision";
			this.collided = true;
			this.lastHitTime = DateTime.Now;
		}
		
	}
		
	void OnTriggerEnter (Collider other)
	{
		Debug.Log ("Creeper Trigger enter!");
		/*
		if (this.canBeGrabbed) {
			this.grabbed = true;
			this.canBeGrabbed = false;
		}
		*/
		
	}
	
	void OnGUI ()
	{
		if (this.collided) {
			GUIStyle style = new GUIStyle ();
			style.fontSize = 30;
			
			if (this.msg.Equals ("LOSS")) {
				style.normal.textColor = Color.red;
			} else {
				style.normal.textColor = new Color (0.117f, 0.879f, 0.398f, 1.0f);
			}
			GUI.Label (new Rect (0, 0, Screen.width, Screen.height), this.msg, style);
			//TODO disable player movement
		}
	}
	
}
