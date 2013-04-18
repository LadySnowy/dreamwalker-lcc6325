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

function Start ()
{
	Alpha = 1.0;
}

function Update ()
{

	if (Input.GetKey ("b")) {
		Memo.Play();
	}
	
	if (Input.GetKey ("space")) {
		Application.LoadLevel("Menu");
	}
	
	if (Time.time > 2 && Time.time < 8)
	{
		if (!YawnPlayed)
		{
			YawnPlayed = true;
			Yawn.Play();
			
		}
	}
	
	if (Time.time > 10 && Time.time < 20)
	{
		if (!CryingPlayed)
		{
			CryingPlayed = true;
			Crying.Play();
			
		}
	}
	
	if (Time.time > 22 && Time.time < 27)
	{
		if (!DialogPlayed)
		{
			DialogPlayed = true;
			Dialog.Play();
			
		}
	}

}

function OnGUI() {
	if (Time.time > 2 && Time.time < 4)
	{
		PlaceImage(Sleeping3);
	}

		if (Time.time > 4 && Time.time < 6)
	{
		PlaceImage(Sleeping2);
	}

		if (Time.time > 6 && Time.time < 8)
	{
		PlaceImage(Sleeping);
	}

	if (Time.time > 8 && Time.time < 10) {
		FadeOut();
		PlaceImage(Sleeping);
		Debug.Log("Setting GUI alpha: " + GUI.color.a);
	}
	
	if (Time.time > 10 && Time.time < 20)
	{
		FadeIn();
		PlaceImage(CryingArt);
	}
	
	if (Time.time > 20 && Time.time < 21) {
		FadeOut();
		PlaceImage(CryingArt);	
	}
	
	if (Time.time > 21 && Time.time < 22)
	{
		FadeIn();
		PlaceImage(LookingUp);
	}
	
	if (Time.time > 22 && Time.time < 26)
	{
		PlaceImage(LookingUp);
	}


	if (Time.time > 26 && Time.time < 38)
	{
		PlaceImage(Pleading);
	}
	
	if (Time.time > 38 && Time.time < 40)
	{
		FadeOut();
		PlaceImage(Pleading);
	}
	
	if (Time.time > 41) {
		Application.LoadLevel("Menu");
	}
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