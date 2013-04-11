using UnityEngine;
using System.Collections;
using System;
using Pathfinding;

[RequireComponent (typeof(CharacterController)), RequireComponent (typeof(TrailRenderer)), RequireComponent (typeof(AudioSource)),
	RequireComponent (typeof(Seeker))]
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
	int currentPatternWaypoint; //lastWaypoint;
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
	
	//pathing variables
	private Seeker seeker;
	//The calculated path
	Path path;
	//The max distance from the AI to a waypoint for it to continue to the next waypoint
	public float nextWaypointDistance = 3;
	//The waypoint we are currently moving towards
	private int currentPathWaypoint;
	//private Vector3 dir;
	//private Vector3 drunkDir;
	public TimeSpan timeDelta;
	public DateTime lastUpdate;
	private Vector3 cross;
	private bool posVector;
	private float magnitude;
	public float CREEPER_DRUNK_VALUE;
	public int deltaSeconds;
	bool needNewPath;
	bool switchedModes;
	bool playerMoved;
	Vector3 lastPlayerPosition;
	public float PLAYER_MOVE_DIST;
	public int audioPauseTimeSeconds;
	DateTime chasePlayTime;
	
	
	
	// Use this for initialization
	void Start ()
	{
		
		//pathing
		timeDelta = new TimeSpan (0, 0, deltaSeconds);
		magnitude = 1;
		posVector = true;
		lastUpdate = System.DateTime.Now;
		seeker = GetComponent<Seeker> ();
		this.needNewPath = true;
		this.currentPathWaypoint = 0;
		this.playerMoved = false;
		
		this.chaseMode = false;
		switchedModes = false;
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
		this.currentPatternWaypoint = 0;
		
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
				DateTime audioCheckTime = DateTime.Now;
				TimeSpan chaseAudioDelta = audioCheckTime - this.chasePlayTime;
				TimeSpan audioGapTime = new TimeSpan(0, 0, this.audioPauseTimeSeconds);
				if (!audio.isPlaying && (chaseAudioDelta > audioGapTime)) {
					audio.Play ();
					this.chasePlayTime = DateTime.Now;
				}
				destination = this.player.transform.position;
				this.gameObject.GetComponent<TrailRenderer> ().enabled = false;
				this.pathLine.renderer.enabled = false;
				if (!this.chaseMode) {
					//beginning chase...
					this.chaseVelocity = this.velocity;
				}
				
				if (!this.chaseMode) {
					this.chaseMode = true;
					this.switchedModes = true;
					this.lastPlayerPosition = this.player.transform.position;
					this.playerMoved = false;
				}
			} else {
				audio.Pause ();
				if (this.chaseMode) {
					this.chaseMode = false;
					this.switchedModes = true;
				}
				this.gameObject.GetComponent<TrailRenderer> ().enabled = true;
				updateDest ();
				destination = this.waypoints [this.currentPatternWaypoint].transform.position;
				
				int maxWaypointArrayIndex = this.waypoints.Length - 1;
				
				if (this.currentPatternWaypoint != maxWaypointArrayIndex) {
					this.canEnableTrailDisplay = true;
				}
				
				if ((!this.trailDisplay) && (this.currentPatternWaypoint == maxWaypointArrayIndex) && (this.canEnableTrailDisplay)) {
					
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
			
			if (this.switchedModes) {
				this.path = null;
				this.needNewPath = true;
				this.switchedModes = false;
			}
			//pathing
			if (path == null) {
				//calculate new path
				if (needNewPath) {
					Debug.Log ("start path: " + this.gameObject.transform.position + ", " + destination);
					seeker.StartPath (this.gameObject.transform.position, destination, OnPathComplete);
					this.needNewPath = false;
				}
				//We have no path to move after yet
				return;
			}
			
			if (this.currentPathWaypoint >= path.vectorPath.Count) {
				Debug.Log ("End Of Path Reached");
				this.path = null;
				this.needNewPath = true;
				return;
			}
			
			
			if (this.chaseMode) {
				double playerMovedDistance = Vector3.Distance (this.lastPlayerPosition, this.player.transform.position);
				if (playerMovedDistance > PLAYER_MOVE_DIST) {
					this.playerMoved = true;
				}
				//calculate new path every update since player is moving...
				if (this.playerMoved) {
					if (seeker.IsDone () && this.needNewPath) {
						//Path p = seeker.GetNewPath (this.gameObject.transform.position, destination);
						seeker.StartPath (this.gameObject.transform.position, destination, OnPathComplete);
						this.needNewPath = false;
					}
					/*
					if (!p.error) {
						this.path = p;
						//Reset the waypoint counter
						this.currentPathWaypoint = 0;
					} else {
						this.needNewPath = true;
						this.path = null;
					}
					*/
					this.playerMoved = false;
					this.lastPlayerPosition = this.player.transform.position;
				}
			}
		
			if (this.path != null) {
				Vector3 travelDirection = calculateMoveVector ();
				//Debug.Log ("travelDirection: " + travelDirection);
			
				//Vector3 currentPosition = this.creeper.transform.position;
			
				//Vector3 travelDirection = destination - currentPosition;
				
			
				//Vector3 lookDirection = Vector3.RotateTowards (this.moveDirection, travelDirection, (200 * Mathf.Deg2Rad) * Time.deltaTime, this.rotationVelocity);
				this.moveDirection = Vector3.RotateTowards (this.moveDirection, travelDirection, (200 * Mathf.Deg2Rad) * Time.deltaTime, this.rotationVelocity);
 
				//rotate our creeper
				if (travelDirection != Vector3.zero) {
					Vector3 lookDirection = new Vector3 (this.moveDirection.x, 0, this.moveDirection.z);
					//this.creeper.transform.rotation = Quaternion.LookRotation (this.moveDirection);
					this.creeper.transform.rotation = Quaternion.LookRotation (lookDirection);
				}
				
				//move our creeper
				//Vector3 movement = new Vector3(this.moveDirection.x, this.moveDirection.y, this.moveDirection.z);
				//movement.Normalize();
				travelDirection.Normalize ();
				float curVelocity = this.velocity;
				if (this.chaseMode) {
					this.chaseVelocity += this.chaseVelocityIncrementValue;
					if (this.chaseVelocity > this.chaseMaxVelocity) {
						this.chaseVelocity = this.chaseMaxVelocity;
					}
					curVelocity = this.chaseVelocity;
				}
				curVelocity *= Time.deltaTime;
				//movement *= curVelocity;
				//this.controller.Move (movement);
				travelDirection *= curVelocity;
				this.controller.Move (travelDirection);
			
				
				updatePathWaypoint ();
			}
			
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
	
	void updatePathWaypoint ()
	{
		//Vector3 positionZeroY = transform.position;
		//positionZeroY.y = 0;
		if (Vector3.Distance (this.gameObject.transform.position, path.vectorPath [currentPathWaypoint]) < nextWaypointDistance) {
			currentPathWaypoint++;
		}
	}
	
	public void OnPathComplete (Path p)
	{
		//Debug.Log ("Yey, we got a path back. Did it have an error? " + p.error);
		if (!p.error) {
			this.path = p;
			//Reset the waypoint counter
			this.currentPathWaypoint = 0;
		} else {
			this.needNewPath = true;
			Debug.Log ("path failure");
		}
	}
	
	Vector3 calculateMoveVector ()
	{
		//Direction to the next waypoint
		//Debug.Log ("vector path: " + this.path.vectorPath);
		//for (int i = 0; i < this.path.vectorPath.Count; i++) {
		//	Debug.Log ("vector path " + i + ": " + this.path.vectorPath [i]);
		//}
		//Debug.Log ("current creeper position: " + this.creeper.transform.position);
		//Debug.Log ("current path waypoint: " + this.path.vectorPath [this.currentPathWaypoint]);
		Vector3 dir = (this.path.vectorPath [this.currentPathWaypoint] - this.creeper.transform.position).normalized;
		//Debug.Log ("dir: " + dir);
		//check if it's time for a magnitude and direction update
		DateTime now = System.DateTime.Now;
		TimeSpan diff = now - this.lastUpdate;
		if (diff > this.timeDelta) {
			//calculate new magnitude and pos/neg direction for perp vector
			System.Random r = new System.Random ();
		
			this.magnitude = (float)r.NextDouble () * CREEPER_DRUNK_VALUE;
			
			double posOrNeg = r.NextDouble ();
			if (posOrNeg < 0.5) {
				this.posVector = true;
			}
			
			this.lastUpdate = now;
		}
		
		//update perpendicular vector every calculation using current posVector true/false and magnitude...
		//Vector3 groundDir = new Vector3 (dir.x, 0, dir.z);
		Vector3 downDir = new Vector3 (0, -1, 0);
		/*
		if (posVector) {
			groundDir.y = dir.y + 1;
		} else {
			groundDir.y = dir.y - 1;
		}
		*/
		
		cross = Vector3.Cross (dir, downDir);
		if (!posVector) {
			cross = -cross;
		}
		cross.Normalize ();
		cross *= magnitude;
		
		Vector3 drunkDir = new Vector3 (dir.x, dir.y, dir.z);
		drunkDir += cross;
		//drunkDir *= speed * Time.fixedDeltaTime;
		
		return drunkDir;
	}
	
	void updateDest ()
	{
		
		Vector3 currentPosition = this.creeper.transform.position;
		Vector3 destination = this.waypoints [currentPatternWaypoint].transform.position;
		
		float distance = Vector3.Distance (currentPosition, destination);
		
		if (distance < WAYPOINT_DIST) { //TODO check value
			//if (this.weight >= 1.0) {
			//this.weight = 0;
			//this.lastWaypoint = this.currentWaypoint;
			this.currentPatternWaypoint = (this.currentPatternWaypoint + 1) % this.waypoints.Length;
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
