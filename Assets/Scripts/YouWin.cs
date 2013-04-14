using UnityEngine;
using System.Collections;

public class YouWin : MonoBehaviour {

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
		style.normal.textColor = new Color (0.117f, 0.879f, 0.398f, 1.0f);
		
		GUI.Label (r, "We are the champions... of the world!");
		
	}
}
