#pragma strict
import System;

var OpeningDoor:AudioSource;
var EvilLaugh:AudioSource;

var OpeningDoorPlayed = false;
var EvilLaughPlayed = false;

var ClosedDoor:Texture2D;
var OpenDoor:Texture2D;
var Cracked:Texture2D;

var Alpha : float;

var last : DateTime;

function Start ()
{
	Alpha = 0.0;
	this.last = DateTime.Now;
}

function Update ()
{

	var cur = DateTime.Now;
	
	var openingTS = new TimeSpan(0, 0, 0, 3, 0);
	var openingStart = this.last + openingTS;
	
	var laughTS = new TimeSpan(0, 0, 0, 4, 0);
	var laughStart = openingStart + laughTS;

	var laughDurationTS = new TimeSpan(0, 0, 0, 4, 0);
	var laughEnd = laughStart + laughDurationTS;
	
	if (cur > openingStart && cur < laughStart) // && Time.time < this.last + 6)
	{
		if (!OpeningDoorPlayed)
		{
			OpeningDoor.Play();
			OpeningDoorPlayed = true;
		}
	}
	
	
	if (cur > laughStart && cur < laughEnd)
	{
		if (!EvilLaughPlayed)
		{
			EvilLaugh.Play();
			EvilLaughPlayed = true;
		}
	}
}

function OnGUI() {

	if (Event.current.type == EventType.KeyDown) {
        KeyPressedEventHandler();
    }
    
	var cur = DateTime.Now;
	var crackedTS = new TimeSpan(0, 0, 0, 2, 0);
	var crackedStart = this.last + crackedTS;

	var closedTS = new TimeSpan(0, 0, 0, 3, 0);
	var closedStart = crackedStart + closedTS;

	var openTS = new TimeSpan(0, 0, 0, 3, 0);
	var openStart = closedStart + openTS;

	var openFadeTS = new TimeSpan(0, 0, 0, 3, 0);
	var openFadeStart = openStart + openFadeTS;
	
	var nextSceneTS = new TimeSpan(0, 0, 0, 2, 0);
	var nextSceneStart = openFadeStart + nextSceneTS;
	
	if (cur > crackedStart && cur < closedStart) {
		FadeIn();
		PlaceImage(ClosedDoor);
		
	}    

	
	if (cur > closedStart && cur < openStart)
	{
		PlaceImage(Cracked);
	}


	if (cur > openStart && cur < openFadeStart)
	{
		PlaceImage(OpenDoor);
	}


	if (cur > openFadeStart && cur < nextSceneStart)
	{
		FadeOut();
		PlaceImage(OpenDoor);
	}

	if (cur > nextSceneStart)
	{
		Application.LoadLevel("Menu");
	}

	
}

function KeyPressedEventHandler() {
	Application.LoadLevel("Menu");
}

function FadeOut() {
		Alpha = Alpha - 0.02;
		if (Alpha < 0) { Alpha = 0; }
		GUI.color.a = Alpha;
}

function FadeIn() {
		Alpha = Alpha + 0.02;
		if (Alpha >= 1) { Alpha = 1; }
		GUI.color.a = Alpha;

}

function PlaceImage(image:Texture2D) {
		var xPos = Screen.width - image.width;
		xPos = xPos / 2;
		var yPos = Screen.height - image.height;
		yPos = yPos / 2;
		var guiContent = new GUIContent(image);
		var rect = GUILayoutUtility.GetRect(image.width, image.height);//, style);
		var placedRect = new Rect(xPos, yPos, rect.width, rect.height);
		GUI.Label (placedRect, guiContent);		

}