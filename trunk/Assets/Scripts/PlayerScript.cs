using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour
{
	
	public AudioSource walking;
	public AudioSource wind;
	public double windProbability;
	
	public DreamwalkerState dreamwalkerState;
	
	Vector3 lastPos;
	
	void Start ()
	{
		this.lastPos = this.gameObject.transform.position;
		CharacterController controller = GetComponent<CharacterController>();
		Collider collider = GetComponent<BoxCollider>();
	    Physics.IgnoreCollision(controller, collider);

	}
	
	// Update is called once per frame
	void Update ()
	{
		if (this.CharMoved()) {
			if (!walking.isPlaying) {
				walking.Play();
			}
		} else {
			walking.Pause();
		}
		
		if (!wind.isPlaying) {
				System.Random r = new System.Random();
				double roll = r.NextDouble();
				if (roll < this.windProbability) {
					wind.Play();
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
				this.dreamwalkerState.clueGrab(0);
			} else if (name.Contains ("2")) {
				//this.clues [1] = true;
				this.dreamwalkerState.clueGrab(1);
			} else if (name.Contains ("3")) {
				//this.clues [2] = true;
				this.dreamwalkerState.clueGrab(2);
			} else if (name.Contains ("4")) {
				//this.clues [3] = true;
				this.dreamwalkerState.clueGrab(3);
			} else if (name.Contains ("5")) {
				//this.clues [4] = true;
				this.dreamwalkerState.clueGrab(4);
			}

		}
	}
	
	bool CharMoved() {
  		Vector3 displacement = this.gameObject.transform.position - this.lastPos;
  		this.lastPos = this.gameObject.transform.position;
  		return (displacement.magnitude > 0.05); // return true if char moved 1mm
	}
	
	public static double Health = 0;
	public const double MaxHealth = 10;
	public const double DamageSpeed = 0.1;
	void OnGUI ()
	{
		GUIStyle style = new GUIStyle ();
		int x = 0;
		int y = 0;
		int w = Screen.width;
		int h = Screen.height;
		float r = 1;
		float g = 1;
		float b = 1;
		double a = Health / MaxHealth;
		Texture2D rgb_texture = new Texture2D(1, 1);
		rgb_texture.wrapMode = TextureWrapMode.Repeat;
	    Color rgb_color = new Color(r, g, b, (float)a);
	    rgb_texture.SetPixel(0, 0, rgb_color);
	    rgb_texture.Apply();
	    GUI.skin.box = style;
	    GUI.DrawTextureWithTexCoords(new Rect (x,y,w,h), rgb_texture, new Rect(0, 0, 1, 1));
	}
}

