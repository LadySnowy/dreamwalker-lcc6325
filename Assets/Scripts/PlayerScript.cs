using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour
{
	
	public AudioSource walking;
	public AudioSource wind;
	public AudioSource retrieval;
	public AudioSource found;
	public AudioSource bringHer;
	public double windProbability;
	public DreamwalkerState dreamwalkerState;
	Vector3 lastPos;
	
	public double Health;// = 3;
	public double CreeperDrainTick;// = 0.02;
	public double MinHealth;// = 0;
	public double MaxHealth;// = 3;
	//public double DamageSpeed = 0.1;
	public double HealthRecoveryTick;// = 0.01;	
	
	void Start ()
	{
		this.lastPos = this.gameObject.transform.position;
		CharacterController controller = GetComponent<CharacterController> ();
		Collider collider = GetComponent<BoxCollider> ();
		Physics.IgnoreCollision (controller, collider);
		this.walking.mute = true;
		this.walking.loop = true;
		this.walking.Play();
	}
	
	// Update is called once per frame
	void Update ()
	{
		//character auto-heals....
		if (this.Health < this.MaxHealth) {
			this.Health += this.HealthRecoveryTick;
		} else {
			this.Health = this.MaxHealth;
		}
		
		if (this.CharMoved ()) {
			walking.mute = false;
		} else {
			walking.mute = true;
		}
		
		if (!wind.isPlaying) {
			System.Random r = new System.Random ();
			double roll = r.NextDouble ();
			if (roll < this.windProbability) {
				wind.Play ();
			}
		}
	}
	
	void OnTriggerEnter (Collider other)
	{
		//Debug.Log ("Player trigger enter!");
		//Debug.Log ("triggered by tag " + other.gameObject.tag);
		
		if (other.gameObject.tag == "Clue") {
			string name = other.gameObject.name;
			if (name.Contains ("1")) {
				//this.clues [0] = true;
				this.dreamwalkerState.clueGrab (0);
			} else if (name.Contains ("2")) {
				//this.clues [1] = true;
				this.dreamwalkerState.clueGrab (1);
			} else if (name.Contains ("3")) {
				//this.clues [2] = true;
				this.dreamwalkerState.clueGrab (2);
			} else if (name.Contains ("4")) {
				//this.clues [3] = true;
				this.dreamwalkerState.clueGrab (3);
			} else if (name.Contains ("5")) {
				//this.clues [4] = true;
				this.dreamwalkerState.clueGrab (4);
			}

		}
	}
	
	public void retrievalEvent ()
	{
		StartCoroutine (retrievalAudioSequence ());
	}
			
	IEnumerator retrievalAudioSequence ()
	{
		this.retrieval.Play ();
		while (this.retrieval.isPlaying) {
			yield return new WaitForSeconds(0.25f);
		}
		
		this.found.Play ();
		
		while (this.found.isPlaying) {
			yield return new WaitForSeconds(0.25f);
		}

		this.bringHer.Play ();
		
		while (this.bringHer.isPlaying) {
			yield return new WaitForSeconds(0.25f);
		}
		
	}
	
	/*
	void myCoRoutine ()
	{
		Color color = this.gameObject.renderer.material.color;
		color.a -= 0.01f;
		this.gameObject.renderer.material.color = color;
	}
	*/
	
	bool CharMoved ()
	{
		Vector3 displacement = this.gameObject.transform.position - this.lastPos;
		this.lastPos = this.gameObject.transform.position;
		return (displacement.magnitude > 0.05); // return true if char moved 1mm
	}
	
	void OnGUI ()
	{
		GUIStyle style = new GUIStyle ();
		int x = 0;
		int y = 0;
		int w = Screen.width;
		int h = Screen.height;
		float r = 1;
		float g = 0;
		float b = 0;
		double a = (this.MaxHealth - this.Health) / MaxHealth;
		Texture2D rgb_texture = new Texture2D (1, 1);
		rgb_texture.wrapMode = TextureWrapMode.Repeat;
		Color rgb_color = new Color (r, g, b, (float)a);
		rgb_texture.SetPixel (0, 0, rgb_color);
		rgb_texture.Apply ();
		GUI.skin.box = style;
		GUI.DrawTextureWithTexCoords (new Rect (x, y, w, h), rgb_texture, new Rect (0, 0, 1, 1));
	}
}

