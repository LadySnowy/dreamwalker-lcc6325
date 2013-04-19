using UnityEngine;
using System.Collections;

public class PauseScript : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKey ("space")) {
			KeyPressedEventHandler ();
		}
	}
	
	void OnGUI ()
	{

		int hintX;
		int hintY = Screen.height - 75;
		
		GUIStyle style = new GUIStyle ();
		style.fontSize = 24;
		style.normal.textColor = new Color (1.0f, 1.0f, 1.0f, 1.0f);
	
		string please;
		GUIContent content;
		Rect hintRect;
		Rect placedRect;
		float xPlacement;

		please = "Press spacebar...";
		content = new GUIContent (please);
		hintRect = GUILayoutUtility.GetRect (content, style);
		xPlacement = Screen.width / 2;
		xPlacement = xPlacement - 100; //dunno why, but hintRect.width is not set correctly for the text (it's 1024...)
		placedRect = new Rect (xPlacement, hintY, hintRect.width, hintRect.height);
		GUI.Label (placedRect, please, style);	
	}

	void KeyPressedEventHandler ()
	{
		Application.LoadLevel ("YouWin");
	}

}
