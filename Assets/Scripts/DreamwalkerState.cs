using UnityEngine;
using System.Collections;

public class DreamwalkerState : MonoBehaviour
{
		
	public Texture2D[] clues;
	bool[] clueVisible;
	int numCreeperHits;
	bool allCluesGrabbed;
	public CreeperMovement[] allCreepers;
	public GameObject exit;
	public GameObject player;
	bool chasePhase;
	public float ALLOWED_EXIT_DIST;
	
	string clueCollectHint;
	string findChildHint;
	string exitPortalHint;
	
	bool showHint;
	
	public PlayerScript playerScript;
	
	// Use this for initialization
	void Start ()
	{
		this.exit.renderer.enabled = false;
		this.showHint = false;
		
		this.numCreeperHits = 0;
		this.allCluesGrabbed = false;
		this.chasePhase = false;

		this.clueVisible = new bool[clues.Length];
		for (int i = 0; i < this.clueVisible.Length; i++) {
			this.clueVisible [i] = false;
		}
		
		this.clueCollectHint = "Find all the sparkling clues... you can survive only so many collisions with the mysterious creepers!";
		this.findChildHint = "Collecting all the hints will show you which creeper represents the child's soul.";
		this.exitPortalHint = "Flee through the portal before they catch you!";
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (this.playerScript.Health <= this.playerScript.MinHealth) {
			//End Game
			Application.LoadLevel("LosePauseScene");
		}
		if (this.chasePhase) {
			float exitDistance = Vector3.Distance (this.exit.transform.position, this.player.transform.position);
			if (exitDistance < ALLOWED_EXIT_DIST) {
				Application.LoadLevel("PauseScene");
			}
		}
		
		if (Input.GetKey ("h")) {
			this.showHint = true;
		} else {
			this.showHint = false;
		}

	}
	
	public void clueGrab (int clueNumber)
	{
		this.clueVisible [clueNumber] = true;
		bool cluesComplete = true;
		for (int i = 0; i < this.clueVisible.Length; i++) {
			if (!this.clueVisible[i]) {
				cluesComplete = false;
			}
		}
		
		if (cluesComplete) {
			this.allCluesGrabbed = true;			
		}
	}
		
	public bool ShouldCreeperHitCount(bool goal) {
		//bool lastCreeper = this.allCluesGrabbed && goal;
		//if (lastCreeper) return false;
		return (!this.chasePhase || !goal);
	}
	
	//call only if shouldCreeperHitCount returns true
	public void creeperHit (bool goal)
	{
		//if (!this.chasePhase || !goal) {
		//	this.numCreeperHits++;
		//}
		//Debug.Log ("DreamwalkerState creeper hit count: " + this.numCreeperHits);
		
		if (this.allCluesGrabbed && goal) {
			//chase phase!
			this.playerScript.retrievalEvent();
			//fog off
			RenderSettings.fog = false;
			//all creepers enter chase phase!
			for (int i = 0; i < this.allCreepers.Length; i++) {
				this.allCreepers[i].enterChasePhase();
			}
			
			this.chasePhase = true;
			this.exit.renderer.enabled = true;
		}
	}
	
	public void DrainPlayerHealth() {
		this.playerScript.Health -= this.playerScript.CreeperDrainTick;
	}
	
	void OnGUI ()
	{
		int startX = Screen.width - 200;
		int startY = Screen.height - 200;
		
		//this.clueLines [0] [0] = new Vector2 (startX, startY);
		//this.clueLines [0] [1] = new Vector2 (startX - 50, startY - 100);
		Rect r = new Rect (startX, startY, 200, 200);
		
		for (int i = 0; i < this.clueVisible.Length; i++) {
			if (this.clueVisible [i]) {
				GUI.Label (r, clues [i]);
			}
		}
		
		int hintX = 50;
		int hintY = Screen.height - 75;
		
		GUIStyle style = new GUIStyle ();
		style.fontSize = 24;
		style.normal.textColor = new Color (0.98f, 1.0f, 0.57f, 1.0f);
		
		if (this.showHint) {
			GUIContent content = null;
			if (!this.allCluesGrabbed) {
				content = new GUIContent(this.clueCollectHint);
			} else if (this.allCluesGrabbed && !this.chasePhase) {
				content = new GUIContent(this.findChildHint);
			} else {
				content = new GUIContent(this.exitPortalHint);
			}
			
			Rect hintRect = GUILayoutUtility.GetRect(content, style);
			Rect placedRect = new Rect(hintX, hintY, hintRect.width, hintRect.height);
			GUI.Label (placedRect, content);
		}
	}
}
