using UnityEngine;
using System.Collections;

public class MenuGUI : MonoBehaviour {
	

	public Texture mouse;
	
	void OnGUI () {
			var width = Screen.width;
	var height = Screen.height;
		
		GUI.skin.box.fontSize = 40;
		GUI.skin.button.fontSize = 20;
		GUI.skin.label.fontSize = 20;
		
		// Make a background box
		GUI.Box(new Rect(width/2-250,50,500,height-100), "DreamWalker");

		GUI.TextArea(new Rect(width/2-200, 100, 400, 100),"As you fall asleep you start to hear crying and screaming. You open your eyes and see a person, slumped over, crying. They look up and notice you. They plead with you, saying “Please, find my child! The white lights will help you find them and the yellow will help guide you.” As you walk away, you hear them mumble, “Please get the right one.”");
		
		// CONTROLS
		GUI.Label(new Rect(width/2-40, 220, 100, 50), "Controls");
		GUI.enabled = false;
		GUI.Button(new Rect(width/2-125, 250, 45,45), "W");
		GUI.Button(new Rect(width/2-175, 300, 45,45), "A");
		GUI.Button(new Rect(width/2-75, 300, 45,45), "D");
		GUI.Button(new Rect(width/2-125, 350, 45,45), "S");
		GUI.enabled = true;
		
		GUI.DrawTexture(new Rect(width/2+50, 300, 50,50), mouse, ScaleMode.ScaleToFit);
		GUI.Label(new Rect(width/2+100, 300, 125,200), "Move mouse to look");
		
		GUI.Label(new Rect(width/2-175, 400, 400, 100), "To interact with something, walk into it.");
		
		// START GAME
		if(GUI.Button(new Rect(width/2-75,height-120,150,50), "Start Game")) {
			Application.LoadLevel(1);
		}
	}
}