#pragma strict


var Yawn:AudioSource;
var Crying:AudioSource;
var Dialog:AudioSource;
var Memo:AudioSource;
var YouAreHere:AudioSource;

var YawnPlayed = false;
var CryingPlayed = false;
var DialogPlayed = false;
var YouAreHerePlayed = false;

var Sleeping:Texture2D;
var Sleeping2:Texture2D;
var Sleeping3:Texture2D;
var CryingArt:Texture2D;
var LookingUp:Texture2D;
var Pleading:Texture2D;

var Alpha : float;

var last : DateTime;

function Start ()
{
	Alpha = 1.0;
	this.last = DateTime.Now;
}

function Update ()
{

	if (Input.GetKey ("b")) {
		Memo.Play();
	}
	
	var cur = DateTime.Now;
	
	var yawnTS = new TimeSpan(0, 0, 0, 2, 0);
	var yawnStart = this.last + yawnTS;
	
	var yawnEndTS = new TimeSpan(0, 0, 0, 6, 0);
	var yawnEnd = yawnStart + yawnEndTS;

	var deadspaceTS = new TimeSpan(0, 0, 0, 2, 0);
	var deadspaceEnd = yawnEnd + deadspaceTS;

	var cryingTS = new TimeSpan(0, 0, 0, 10, 0);
	var cryingEnd = deadspaceEnd + cryingTS;

	var cryingDeadspaceTS = new TimeSpan(0, 0, 0, 1, 0);
	var cryingDeadspaceEnd = cryingEnd + cryingDeadspaceTS;

	var youAreHereTS = new TimeSpan(0, 0, 0, 2, 0);
	var youAreHereEnd = cryingDeadspaceEnd + youAreHereTS;

	var dialogTS = new TimeSpan(0, 0, 0, 5, 0);
	var dialogEnd = youAreHereEnd + dialogTS;

	if (cur > yawnStart && cur < yawnEnd)
	{
		if (!YawnPlayed)
		{
			YawnPlayed = true;
			Yawn.Play();
			
		}
	}
		
	if (cur > deadspaceEnd && cur < cryingEnd)
	{
		if (!CryingPlayed)
		{
			CryingPlayed = true;
			Crying.Play();
			
		}
	}
	
	if (cur > cryingDeadspaceEnd && cur < youAreHereEnd)
	{
		if (!YouAreHerePlayed)
		{
			YouAreHerePlayed = true;
			YouAreHere.Play();
		}
	}
	
	if (cur > youAreHereEnd && cur < dialogEnd)
	{
		if (!DialogPlayed)
		{
			DialogPlayed = true;
			Dialog.Play();
			
		}
	}

}

function OnGUI() {

	if (Event.current.type == EventType.KeyDown) {
		if (!Input.GetKey ("b")) {
        	KeyPressedEventHandler();
        }
    }

	var cur = DateTime.Now;
	var sleeping3TS = new TimeSpan(0, 0, 0, 2, 0);
	var sleeping3Start = this.last + sleeping3TS;

	var sleeping3DurationTS = new TimeSpan(0, 0, 0, 2, 0);
	var sleeping3End = sleeping3Start + sleeping3DurationTS;

	var sleeping2DurationTS = new TimeSpan(0, 0, 0, 2, 0);
	var sleeping2End = sleeping3End + sleeping2DurationTS;

	var sleepingDurationTS = new TimeSpan(0, 0, 0, 2, 0);
	var sleepingEnd = sleeping2End + sleepingDurationTS;
	
	var sleepingFadeOutDurationTS = new TimeSpan(0, 0, 0, 2, 0);
	var sleepingFadeOutStart = sleepingEnd + sleepingFadeOutDurationTS;

	var cryingFadeInDurationTS = new TimeSpan(0, 0, 0, 10, 0);
	var cryingFadeInStart = sleepingFadeOutStart + cryingFadeInDurationTS;

	var cryingFadeOutDurationTS = new TimeSpan(0, 0, 0, 1, 0);
	var cryingFadeOutStart = cryingFadeInStart + cryingFadeOutDurationTS;

	var lookingUpFadeInTS = new TimeSpan(0, 0, 0, 3, 0);
	var lookingUpFadeInStart = cryingFadeOutStart + lookingUpFadeInTS;
	
	var theCluesTextTS = new TimeSpan(0, 0, 0, 4, 0); //text
	var theCluesTextStart = lookingUpFadeInStart + theCluesTextTS;
	
	var whenYouFindTextTS = new TimeSpan(0, 0, 0, 3.75, 0); //text
	var whenYouFindTextStart = theCluesTextStart + whenYouFindTextTS;

	var pleadingDurationTS = new TimeSpan(0, 0, 0, 13, 0);
	var pleadingEnd = lookingUpFadeInStart + pleadingDurationTS;

	var pleadingFadeOutTS = new TimeSpan(0, 0, 0, 2, 0);
	var pleadingFadeOutStart = pleadingEnd + pleadingFadeOutTS;

	if (cur > sleeping3Start && cur < sleeping3End)
	{
		PlaceImage(Sleeping);
	}
	
	if (cur > sleeping3End && cur < sleeping2End)
	{
		PlaceImage(Sleeping2);
	}

	if (cur > sleeping2End && cur < sleepingEnd)
	{
		PlaceImage(Sleeping3);
	}

	if (cur > sleepingEnd && cur < sleepingFadeOutStart)
	{
		FadeOut();
		PlaceImage(Sleeping3);
	}
	
	if (cur > sleepingFadeOutStart && cur < cryingFadeInStart)
	{
		FadeIn();
		PlaceImage(CryingArt);
	}
	
	if (cur > cryingFadeInStart && cur < cryingFadeOutStart)
	{
		FadeOut();
		PlaceImage(CryingArt);	
	}
	
	if (cur > cryingFadeOutStart && cur < lookingUpFadeInStart)
	{
		FadeIn();
		PlaceImage(LookingUp);
	}
	
	var hintX: int;
	var hintY = Screen.height - 75;
		
	var style = new GUIStyle ();
	style.fontSize = 24;
	style.normal.textColor = new Color (1.0f, 1.0f, 1.0f, 1.0f);
	
	var please : String;
	var content : GUIContent;
	var hintRect : Rect;
	var placedRect : Rect;
	
	var xPlacement : double;

	if (cur > lookingUpFadeInStart && cur < pleadingEnd)
	{
		placedRect = PlaceImage(Pleading);
		please = "Please, help me...";
		content = new GUIContent(please);
		hintRect = GUILayoutUtility.GetRect(content, style);
		xPlacement = Screen.width / 2;
		xPlacement = xPlacement - 100; //dunno why, but hintRect.width is not set correctly for the text (it's 1024...)
		placedRect = new Rect(xPlacement, hintY, hintRect.width, hintRect.height);
		GUI.Label (placedRect, please, style);
	}

	if (cur > theCluesTextStart && cur < whenYouFindTextStart)
	{
		PlaceImage(Pleading);
		please = "The clues will lead you to my child...";
		content = new GUIContent(please);
		hintRect = GUILayoutUtility.GetRect(content, style);
		xPlacement = Screen.width / 2;
		xPlacement = xPlacement - 200; //dunno why, but hintRect.width is not set correctly for the text (it's 1024...)
		placedRect = new Rect(xPlacement, hintY, hintRect.width, hintRect.height);
		GUI.Label (placedRect, please, style);	
	}
	
	if (cur > whenYouFindTextStart && cur < pleadingEnd)
	//if (cur > this.last && cur < whenYouFindTextStart)
	{
		PlaceImage(Pleading);
		please = "When you find her she will follow you to the light, but beware of what you encounter...";
		content = new GUIContent(please);
		hintRect = GUILayoutUtility.GetRect(content, style);
		xPlacement = Screen.width / 2;
		xPlacement = xPlacement - 450; //dunno why, but hintRect.width is not set correctly for the text (it's 1024...)
		placedRect = new Rect(xPlacement, hintY, hintRect.width, hintRect.height);
		GUI.Label (placedRect, please, style);
	}
		
	if (cur > pleadingEnd && cur < pleadingFadeOutStart)
	{
		FadeOut();
		PlaceImage(Pleading);
	}
	
	if (cur > pleadingFadeOutStart)
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
		var rect = GUILayoutUtility.GetRect(image.width, image.height);
		var placedRect = new Rect(xPos, yPos, rect.width, rect.height);
		GUI.Label (placedRect, guiContent);	
		return placedRect;	
}