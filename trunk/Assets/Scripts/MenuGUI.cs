using UnityEngine;
using System.Collections;

public class MenuGUI : MonoBehaviour
{
	

	public Texture mouse;
	
	// Update is called once per frame
	void Update () {
		Screen.showCursor = true;
	}
	
	void OnGUI ()
	{
		var width = Screen.width;
		var height = Screen.height;
		
		GUI.skin.box.fontSize = 40;
		GUI.skin.button.fontSize = 20;
		GUI.skin.label.fontSize = 20;
		
		// Make a background box
		GUIStyle style = new GUIStyle ();
		style.fontSize = 36;
		style.normal.textColor = new Color (1.0f, 1.0f, 1.0f, 1.0f);
		GUIContent content = new GUIContent ("Dreamwalker");
		Rect titleRect = GUILayoutUtility.GetRect (content, style);
		Rect placedRect = new Rect (width / 2 - titleRect.width / 2, titleRect.height, titleRect.width, titleRect.height);
		GUI.Label (placedRect, content, style);
		//GUI.Box(new Rect(width/2-250,50,500,height-100), "");
		
		//string text = "You drift off to a deep sleep filled with dreams. Within one dream, you encounter a parent who fears that her child, currently in a coma, has lost her spirit. Knowing that her child is not the same, she pleads with you to recover it. Startled, and wondering if you could accomplish such a task, you find yourself being pulled into an eerie dream world. As you leave the distressed parent, she says, \"clues will lead you to my child's spirit. Find her spirit and recover it... but beware what you encounter...\"";
		//GUI.TextArea(new Rect(width/2-200, 100, 400, 140),text);
		
		// CONTROLS
		GUI.Label (new Rect (width / 2 - 40, 240, 100, 50), "Controls");
		GUI.enabled = false;
		GUI.Button (new Rect (width / 2 - 125, 270, 45, 45), "W");
		GUI.Button (new Rect (width / 2 - 175, 320, 45, 45), "A");
		GUI.Button (new Rect (width / 2 - 75, 320, 45, 45), "D");
		GUI.Button (new Rect (width / 2 - 125, 370, 45, 45), "S");
		GUI.enabled = true;
		
		GUI.DrawTexture (new Rect (width / 2 + 50, 320, 50, 50), mouse, ScaleMode.ScaleToFit);
		GUI.Label (new Rect (width / 2 + 100, 320, 125, 200), "Move mouse to look");
		
		GUI.Label (new Rect (width / 2 - 175, 420, 400, 40), "Press h for a hint");

		GUI.Label (new Rect (width / 2 - 175, 460, 400, 100), "To interact with something, walk into it.");
		
		// START GAME
		if (GUI.Button (new Rect (width / 2 - 75, height - 100, 150, 50), "Start Game")) {
			Application.LoadLevel ("world");
			//PlayerScript.Health = 0;
		}
	}
}