using UnityEngine;
using System.Collections;
using System;

public class CreeperMovement : MonoBehaviour
{
	
	//public String gameObjectName;
	GameObject creeper;
	//Vector3 creeperOrigPosition;
	//Vector3 cube1Position;
	//Vector3 destination;
	//float weight = 0.0f;
	float step = 0.01f;
	//float step = 5.0f;
	//int direction = 1;
	DateTime lastUpdate;
	public GameObject[] waypoints;
	//Vector3[] creeperPositions;
	int currentWaypoint; //lastWaypoint;
	public float yValue;
	
	bool grabbed;
	bool canBeGrabbed;
	string msg;
	
	public float PLAYER_PROXIMITY;
	
	public GameObject player;
	
	// Use this for initialization
	void Start ()
	{
		this.grabbed = false;
		this.canBeGrabbed = true;
		this.creeper = this.gameObject;

		if (this.creeper.name.Contains("Star")) {
			this.msg = "VICTORY!!";
		} else {
			this.msg = "LOSS";
		}
		//this.creeperOrigPosition = this.creeper.transform.position;
		this.lastUpdate = System.DateTime.Now;

		//this.creeperPositions = new Vector3[5];
		//this.creeperPositions [0] = GameObject.Find ("Cube1").transform.position;
		//this.creeperPositions [1] = GameObject.Find ("Cube2").transform.position;
		//this.creeperPositions [2] = GameObject.Find ("Cube3").transform.position;
		//this.creeperPositions [3] = GameObject.Find ("Cube4").transform.position;
		//this.creeperPositions [4] = GameObject.Find ("Cube5").transform.position;
		
		//this.lastWaypoint = 0;
		this.currentWaypoint = 1;
		
	}
	
	// Update is called once per frame
	void Update ()
	{
	
		if (!this.grabbed) { //easy way to "pause"
		DateTime cur = System.DateTime.Now;
		TimeSpan diff = cur - this.lastUpdate;
			Vector3 destination;
			float playerDistance = Vector3.Distance(this.creeper.transform.position, this.player.transform.position);
			if (playerDistance < PLAYER_PROXIMITY) {
				Debug.Log ("Creeper near!!");
				destination = this.player.transform.position;	
			} else {
				updateDest();
				destination = this.waypoints[currentWaypoint].transform.position;
			}
		if (diff.Milliseconds > 25) {
			this.lastUpdate = cur;
			//updateDest ();
			
			Vector3 currentPosition = this.creeper.transform.position;
			
			Vector3 travelDirection = destination - currentPosition;
				
			
			
			//Vector3 lastLocation = this.waypoints[lastWaypoint].transform.position;
			//interpolate movement between original position and cube position
			//float x = weight * destination.x + (1 - weight) * lastLocation.x;
			//float y = this.yValue;
			//float z = weight * destination.z + (1 - weight) * lastLocation.z;
				
			float x = currentPosition.x + travelDirection.x * this.step;
			float y = this.yValue;
			float z = currentPosition.z + travelDirection.z * this.step;
			
			Vector3 newPosition = new Vector3 (x, y, z);		
			this.creeper.transform.position = newPosition;
				
		}
			
						//check proximity to player
		
		}
	}
			
	void updateDest ()
	{
		
		Vector3 currentPosition = this.creeper.transform.position;
		Vector3 destination = this.waypoints[currentWaypoint].transform.position;
		
		float distance = Vector3.Distance(currentPosition, destination);
		
		if (distance < 4.0f) { //TODO check value
		//if (this.weight >= 1.0) {
			//this.weight = 0;
			//this.lastWaypoint = this.currentWaypoint;
			this.currentWaypoint = (this.currentWaypoint + 1) % this.waypoints.Length;
			//this.destination = this.creeperPositions [this.currentCube];
			//this.direction = -1;
		} else {
		//if (this.weight <= 0.0) {
		//		this.direction = 1;
		//}
		//this.weight += this.direction * this.step;
		//this.weight += this.step;
		}
	}
	
	void OnTriggerEnter (Collider other)
	{
		Debug.Log ("Creeper Trigger enter!");
		if (this.canBeGrabbed) {
			this.grabbed = true;
			this.canBeGrabbed = false;
		}
		
	}
	
	void OnGUI ()
	{
		if (this.grabbed) {
			GUIStyle style = new GUIStyle();
			style.fontSize = 200;
			
			if (this.msg.Equals("LOSS")) {
				style.normal.textColor = Color.red;
			} else {
				style.normal.textColor = new Color(0.117f, 0.879f, 0.398f, 1.0f);
			}
			GUI.Label(new Rect(0, 0, Screen.width, Screen.height), this.msg, style);
			//TODO disable player movement
		}
	}
	
}
