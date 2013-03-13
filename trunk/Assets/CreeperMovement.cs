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
	float weight = 0.0f;
	float step = 0.01f;
	//int direction = 1;
	DateTime lastUpdate;
	public GameObject[] waypoints;
	//Vector3[] creeperPositions;
	int currentWaypoint, lastWaypoint;
	public float yValue;
	
	
	// Use this for initialization
	void Start ()
	{
		this.creeper = this.gameObject;
		
		//this.creeperOrigPosition = this.creeper.transform.position;
		this.lastUpdate = System.DateTime.Now;

		//this.creeperPositions = new Vector3[5];
		//this.creeperPositions [0] = GameObject.Find ("Cube1").transform.position;
		//this.creeperPositions [1] = GameObject.Find ("Cube2").transform.position;
		//this.creeperPositions [2] = GameObject.Find ("Cube3").transform.position;
		//this.creeperPositions [3] = GameObject.Find ("Cube4").transform.position;
		//this.creeperPositions [4] = GameObject.Find ("Cube5").transform.position;
		
		this.lastWaypoint = 0;
		this.currentWaypoint = 1;
		
	}
	
	// Update is called once per frame
	void Update ()
	{
	
		DateTime cur = System.DateTime.Now;
		TimeSpan diff = cur - this.lastUpdate;
		if (diff.Milliseconds > 25) {
			this.lastUpdate = cur;
			updateDest ();
			
			//Vector3 currentPosition = GameObject.Find ("Creeper").transform.position;
			
			Vector3 destination = this.waypoints[currentWaypoint].transform.position;
			Vector3 lastLocation = this.waypoints[lastWaypoint].transform.position;
			//interpolate movement between original position and cube position
			float x = weight * destination.x + (1 - weight) * lastLocation.x;
			//float y = weight * this.destination.y + (1 - weight) * this.cubePositions[this.lastCube].y;
			float y = this.yValue;
			float z = weight * destination.z + (1 - weight) * lastLocation.z;
			
			Vector3 newPosition = new Vector3 (x, y, z);		
			this.creeper.transform.position = newPosition;
		}
	}
			
	void updateDest ()
	{
		if (this.weight >= 1.0) {
			this.weight = 0;
			this.lastWaypoint = this.currentWaypoint;
			this.currentWaypoint = (this.currentWaypoint + 1) % this.waypoints.Length;
			//this.destination = this.creeperPositions [this.currentCube];
			//this.direction = -1;
		} else {
		//if (this.weight <= 0.0) {
		//		this.direction = 1;
		//}
		//this.weight += this.direction * this.step;
		this.weight += this.step;
		}
	}
	
}
