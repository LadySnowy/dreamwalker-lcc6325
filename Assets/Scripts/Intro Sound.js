#pragma strict


var Yawn:AudioSource;
var Crying:AudioSource;
var Dialog:AudioSource;
var Memo:AudioSource;

var YawnPlayed = false;
var CryingPlayed = false;
var DialogPlayed = false;

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

	var cryingDeadspaceTS = new TimeSpan(0, 0, 0, 5, 0);
	var cryingDeadspaceEnd = cryingEnd + cryingDeadspaceTS;

	var dialogTS = new TimeSpan(0, 0, 0, 5, 0);
	var dialogEnd = cryingDeadspaceEnd + dialogTS;

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
	
	if (cur > cryingDeadspaceEnd && cur < dialogEnd)
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
        KeyPressedEventHandler();
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

	var lookingUpFadeInTS = new TimeSpan(0, 0, 0, 5, 0);
	var lookingUpFadeInStart = cryingFadeOutStart + lookingUpFadeInTS;

	var pleadingDurationTS = new TimeSpan(0, 0, 0, 13, 0);
	var pleadingEnd = lookingUpFadeInStart + pleadingDurationTS;

	var pleadingFadeOutTS = new TimeSpan(0, 0, 0, 4, 0);
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
	
	if (cur > lookingUpFadeInStart && cur < pleadingEnd)
	{
		PlaceImage(Pleading);
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
		var rect = GUILayoutUtility.GetRect(image.width, image.height);//, style);
		var placedRect = new Rect(xPos, yPos, rect.width, rect.height);
		GUI.Label (placedRect, guiContent);		

}