using UnityEngine;
using System.Collections;
using System;
using Pathfinding;

[RequireComponent (typeof(CharacterController)), RequireComponent (typeof(TrailRenderer)), RequireComponent (typeof(AudioSource)),
	RequireComponent (typeof(Seeker))]
public class CreeperMovement : MonoBehaviour
{
	
	GameObject creeper;
	CharacterController controller;

	private Vector3 moveDirection = Vector3.zero;

	public GameObject[] waypoints;

	int currentPatternWaypoint;
	bool collided;
	bool grabbed;
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
	public bool goalCreeper;
	
	//pathing variables
	private Seeker seeker;
	//The calculated path
	Path path;
	//The max distance from the AI to a waypoint for it to continue to the next waypoint
	public float nextPathPointDistance;
	//The waypoint we are currently moving towards
	private int currentPathWaypoint;

	public TimeSpan timeDelta;
	public DateTime lastUpdate;
	private Vector3 cross;
	private bool posVector;
	private float magnitude;
	public float CREEPER_DRUNK_VALUE;
	public int chaseAudioDeltaSeconds;
	public double whisperProbability;
	bool needNewPath;
	bool switchedModes;
	bool playerMoved;
	Vector3 lastPlayerPosition;
	public float PLAYER_MOVE_DIST;
	public int audioChasePauseTimeSeconds;
	DateTime chasePlayTime;
	public AudioSource chaseAudio;
	public AudioSource whisperAudio;
	public DreamwalkerState dreamwalkerState;
	bool chasePhase;
	
	void Start ()
	{
		
		this.chasePhase = false;
		
		//pathing
		timeDelta = new TimeSpan (0, 0, chaseAudioDeltaSeconds);
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
		this.creeper = this.gameObject;

		this.currentPatternWaypoint = 0;
		
	}
	
	public void enterChasePhase ()
	{
		this.chasePhase = true;
		this.PLAYER_PROXIMITY = 5000; //change based on level dimensions!
		audioChasePauseTimeSeconds = 0;
		this.chaseMaxVelocity = 6;
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
			
			PlayerScript.Health += PlayerScript.DamageSpeed;
			
			if (PlayerScript.Health > PlayerScript.MaxHealth){
				//End Game
				Application.LoadLevel(2);
			}
		}
		if (!this.grabbed) { //easy way to "pause"
			
			if (!whisperAudio.isPlaying) {
				System.Random r = new System.Random ();
				double roll = r.NextDouble ();
				if (roll < this.whisperProbability) {
					whisperAudio.Play ();
				}
			}

			Vector3 destination;
			float playerDistance = Vector3.Distance (this.creeper.transform.position, this.player.transform.position);
			if (playerDistance < PLAYER_PROXIMITY) {
				//Debug.Log ("Creeper near!!");
				//this.creeper.transform.LookAt (this.player.transform.position); //turns to face the player immediately... kinda creepy!
				DateTime audioChaseCheckTime = DateTime.Now;
				TimeSpan chaseAudioDelta = audioChaseCheckTime - this.chasePlayTime;
				TimeSpan audioChaseGapTime = new TimeSpan (0, 0, this.audioChasePauseTimeSeconds);
				if (!chaseAudio.isPlaying && (chaseAudioDelta > audioChaseGapTime)) {
					chaseAudio.Play ();
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
				updateDest ();
				destination = this.waypoints [this.currentPatternWaypoint].transform.position;
				
				int maxWaypointArrayIndex = this.waypoints.Length - 1;
				
				if (this.currentPatternWaypoint != maxWaypointArrayIndex) {
					this.canEnableTrailDisplay = true;
				}
				
				if ((!this.trailDisplay) && (this.currentPatternWaypoint == maxWaypointArrayIndex) && (this.canEnableTrailDisplay)) {
					
					this.canEnableTrailDisplay = false;
					this.enabledTrailTime = DateTime.Now;
					this.trailDisplay = true;
					this.pathLine.renderer.enabled = true;
				}
				
				DateTime now = DateTime.Now;
				TimeSpan diff = now - this.enabledTrailTime;
				if (this.trailDisplay && (diff.Seconds > TRAIL_SHOW_TIME_SECS)) {
					this.trailDisplay = false;
					this.pathLine.renderer.enabled = false;
				}
				
			}
			
			if (this.switchedModes) {
				this.path = null;
				this.needNewPath = true;
				this.switchedModes = false;
			}
			//pathing
			if (path == null) {
				//calculate new path
				if (needNewPath) {
					//Debug.Log ("start path: " + this.gameObject.transform.position + ", " + destination);
					seeker.StartPath (this.gameObject.transform.position, destination, OnPathComplete);
					this.needNewPath = false;
				}
				//We have no path to move after yet
				return;
			}
			
			if (this.currentPathWaypoint >= path.vectorPath.Count) {
				//Debug.Log ("End Of Path Reached");
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
					//if (seeker.IsDone () && this.needNewPath) {
					if (seeker.IsDone ()) {
						//Path p = seeker.GetNewPath (this.gameObject.transform.position, destination);
						seeker.StartPath (this.gameObject.transform.position, destination, OnPathComplete);
						this.needNewPath = false;
					}
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
			
		}
	}
	
	void updatePathWaypoint ()
	{
		//Vector3 positionZeroY = transform.position;
		//positionZeroY.y = 0;
		if (Vector3.Distance (this.gameObject.transform.position, path.vectorPath [currentPathWaypoint]) < this.nextPathPointDistance) {
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
		Vector3 downDir = new Vector3 (0, -1, 0);		
		cross = Vector3.Cross (dir, downDir);
		if (!posVector) {
			cross = -cross;
		}
		cross.Normalize ();
		cross *= magnitude;
		
		Vector3 drunkDir = new Vector3 (dir.x, dir.y, dir.z);
		drunkDir += cross;
		
		return drunkDir;
	}
	
	void updateDest ()
	{
		
		Vector3 currentPosition = this.creeper.transform.position;
		Vector3 destination = this.waypoints [currentPatternWaypoint].transform.position;
		
		float distance = Vector3.Distance (currentPosition, destination);
		
		if (distance < WAYPOINT_DIST) {
			this.currentPatternWaypoint = (this.currentPatternWaypoint + 1) % this.waypoints.Length;
			this.gameObject.GetComponent<TrailRenderer> ().enabled = true;
			
		} else {
			//Debug.Log ("Not close enough to switch waypoint" + this.creeper.gameObject.name + ", dist: " + distance);
		}
	}
	
	void OnControllerColliderHit (ControllerColliderHit hit)
	{
		//Debug.Log ("OnControllerColliderHit");
		
		//Debug.Log ("CREEPER MOVEMENT HIT " + hit.gameObject.name);
		if ((hit.gameObject.tag.Equals ("Player")) && !this.collided) {
			Debug.Log ("Creeper collision: " + DateTime.Now);
			//LIZZY: SCREEN SHAKE!!!! :)
			this.dreamwalkerState.creeperHit (this.goalCreeper);
			this.collided = true;
			this.lastHitTime = DateTime.Now;
		}
		
	}
	
	
			
}
