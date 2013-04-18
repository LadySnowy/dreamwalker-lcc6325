#pragma strict

var Laugh:AudioSource;

var LaughPlayed = false;

var Newspaper:Texture2D;

var Alpha : float;

function Start ()
{
	Alpha = 1.0;
}

function Update ()
{

	if (Time.time > 2 && Time.time < 8)
	{
		if (!LaughPlayed)
		{
			LaughPlayed = true;
			Laugh.Play();
			
		}
	}
	
}

function OnGUI() {

	if (Event.current.type == EventType.KeyDown) {
        KeyPressedEventHandler();
    }

	if (Time.time > 3 && Time.time < 5)
	{
		FadeIn();
		PlaceImage(Newspaper);
	}

		if (Time.time > 5 && Time.time < 10)
	{
		PlaceImage(Newspaper);
	}

	if (Time.time > 10 && Time.time < 12)
	{
		FadeOut();
		PlaceImage(Newspaper);
	}

	if (Time.time > 12 && Time.time < 20)
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