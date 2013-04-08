using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour
{
	
	bool[] clues;
	Vector2[][] clueLines;
	
	// Use this for initialization
	void Start ()
	{
		this.clues = new bool[5];
		for (int i = 0; i < this.clues.Length; i++) {
			this.clues [i] = false;
		}
		this.clueLines = new Vector2[5][];
		this.clueLines [0] = new Vector2[2];
		int startX = Screen.width - 100;
		int startY = Screen.height - 50;
		this.clueLines [0] [0] = new Vector2 (startX, startY);
		this.clueLines [0] [1] = new Vector2 (startX - 50, startY - 100);
		
		this.clueLines [1] = new Vector2[2];
		this.clueLines [1] [0] = new Vector2 (startX - 50, startY - 100);
		this.clueLines [1] [1] = new Vector2 (startX - 100, startY);
		
		this.clueLines [2] = new Vector2[2];
		this.clueLines [2] [0] = new Vector2 (startX - 100, startY);
		this.clueLines [2] [1] = new Vector2 (startX + 25, startY - 50);

		this.clueLines [3] = new Vector2[2];
		this.clueLines [3] [0] = new Vector2 (startX + 25, startY - 50);
		this.clueLines [3] [1] = new Vector2 (startX - 125, startY - 50);

		this.clueLines [4] = new Vector2[2];
		this.clueLines [4] [0] = new Vector2 (startX - 125, startY - 50);
		this.clueLines [4] [1] = new Vector2 (startX, startY);

		Debug.Log (Screen.width);
		Debug.Log (Screen.height);

	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
	
	void OnTriggerEnter (Collider other)
	{
		Debug.Log ("Player trigger enter!");
		//DestroyObject (this.gameObject);
		
		Debug.Log ("triggered by tag " + other.gameObject.tag);
		
		if (other.gameObject.tag == "Clue") {
			string name = other.gameObject.name;
			if (name.Contains ("1")) {
				this.clues [0] = true;
			} else if (name.Contains ("2")) {
				this.clues [1] = true;
			} else if (name.Contains ("3")) {
				this.clues [2] = true;
			} else if (name.Contains ("4")) {
				this.clues [3] = true;
			} else if (name.Contains ("5")) {
				this.clues [4] = true;
			}

		}
	}
	
	void OnGUI ()
	{

		/* The "GUIHelper.BeginGroup(Rect)" method can be used now instead of GUI.BeginGroup(Rect) */
		//GUIHelper.BeginGroup (new Rect (Screen.width - 250, Screen.height - 100, 300, 300));
		GUIHelper.BeginGroup (new Rect (0, 0, Screen.width, Screen.height));

		for (int i = 0; i < this.clues.Length; i++) {
			
     
			/* The "GUIHelper.DrawLine(Vector2, Vector2, Color);"
    method will draw a 2D line, with a specified color */
			if (this.clues [i]) {
				//Debug.Log ("Drawing clue " + i);
				GUIHelper.DrawLine (this.clueLines [i] [0], this.clueLines [i] [1], Color.red);
			}
			
			//GUIHelper.DrawLine (new Vector2(Screen.width - 200, Screen.height - 200), new Vector2(Screen.width - 250, Screen.height - 300), Color.red);
			//GUIHelper.DrawLine (new Vector2 (Screen.width - 200, Screen.height - 50), new Vector2 (Screen.width - 250, Screen.height - 100), Color.red);
     
				
		}
		/* The "GUIHelper.EndGroup()" method will pop the clipping, and disable it. Must be called for every "GUIHelper.BeginGroup(Rect);" */
		GUIHelper.EndGroup ();
	}
}

class GUIHelper
{
	protected static bool clippingEnabled;
	protected static Rect clippingBounds;
	protected static Material lineMaterial;
     
	/* @ Credit: "http://cs-people.bu.edu/jalon/cs480/Oct11Lab/clip.c" */
	protected static bool clip_test (float p, float q, ref float u1, ref float u2)
	{
		float r;
		bool retval = true;
		if (p < 0.0) {
			r = q / p;
			if (r > u2)
				retval = false;
			else if (r > u1)
				u1 = r;
		} else if (p > 0.0) {
			r = q / p;
			if (r < u1)
				retval = false;
			else if (r < u2)
				u2 = r;
		} else if (q < 0.0)
			retval = false;
     
		return retval;
	}
     
	protected static bool segment_rect_intersection (Rect bounds, ref Vector2 p1, ref Vector2 p2)
	{
		float u1 = 0.0f, u2 = 1.0f, dx = p2.x - p1.x, dy;
		if (clip_test (-dx, p1.x - bounds.xMin, ref u1, ref u2))
		if (clip_test (dx, bounds.xMax - p1.x, ref u1, ref u2)) {
			dy = p2.y - p1.y;
			if (clip_test (-dy, p1.y - bounds.yMin, ref u1, ref u2))
			if (clip_test (dy, bounds.yMax - p1.y, ref u1, ref u2)) {
				if (u2 < 1.0) {
					p2.x = p1.x + u2 * dx;
					p2.y = p1.y + u2 * dy;
				}
				if (u1 > 0.0) {
					p1.x += u1 * dx;
					p1.y += u1 * dy;
				}
				return true;
			}
		}
		return false;
	}
     
	public static void BeginGroup (Rect position)
	{
		clippingEnabled = true;
		clippingBounds = new Rect (0, 0, position.width, position.height);
		GUI.BeginGroup (position);
	}
     
	public static void EndGroup ()
	{
		GUI.EndGroup ();
		clippingBounds = new Rect (0, 0, Screen.width, Screen.height);
		clippingEnabled = false;
	}
     
	public static void DrawLine (Vector2 pointA, Vector2 pointB, Color color)
	{
		if (clippingEnabled)
		if (!segment_rect_intersection (clippingBounds, ref pointA, ref pointB))
			return;
     
		if (!lineMaterial) {
			/* Credit:  */
			lineMaterial = new Material ("Shader \"Lines/Colored Blended\" {" +
               "SubShader { Pass {" +
               "   BindChannels { Bind \"Color\",color }" +
               "   Blend SrcAlpha OneMinusSrcAlpha" +
               "   ZWrite Off Cull Off Fog { Mode Off }" +
               "} } }");
			lineMaterial.hideFlags = HideFlags.HideAndDontSave;
			lineMaterial.shader.hideFlags = HideFlags.HideAndDontSave;
		}
     
		lineMaterial.SetPass (0);
		GL.Begin (GL.LINES);
		GL.Color (color);
		GL.Vertex3 (pointA.x, pointA.y, 0);
		GL.Vertex3 (pointB.x, pointB.y, 0);
		GL.End ();
	}
};