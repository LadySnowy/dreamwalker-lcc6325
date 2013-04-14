using UnityEngine;
using System.Collections;

public class YouLose : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI ()
	{
		int startX = 0;
		int startY = 0;
		
		Rect r = new Rect(startX, startY, Screen.width, Screen.height);
		
		GUIStyle style = new GUIStyle ();
		style.fontSize = 48;
		style.normal.textColor = Color.red;
		
		GUI.Label (r, "Awww, snap! Try again?");
		
	}
}
