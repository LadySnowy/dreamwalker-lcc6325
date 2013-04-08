using UnityEngine;
using System.Collections;

public class MenuGUI : MonoBehaviour {

	void OnGUI () {
		var width = Screen.width;
		var height = Screen.height;
		// Make a background box
		GUI.Box(new Rect(width/2-250,50,500,height-100), "DreamWalker");

		// Make the first button. If it is pressed, Application.Loadlevel (1) will be executed
		if(GUI.Button(new Rect(width/2-40,100,80,20), "Start Game")) {
			Application.LoadLevel(1);
		}
	}
}