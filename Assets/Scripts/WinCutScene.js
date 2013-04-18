#pragma strict

var Laugh:AudioSource;

var LaughPlayed = false;

var Newspaper:Texture2D;

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
	var laughStart = this.last + openingTS;
	
	var laughTS = new TimeSpan(0, 0, 0, 6, 0);
	var laughEnd = laughStart + laughTS;

	if (cur > laughStart && cur < laughEnd)
	{
		if (!LaughPlayed)
		{
			LaughPlayed = true;
			Laugh.Play();
			
		}
	}
	
	if (cur > laughEnd) {
		if (LaughPlayed) {
			Laugh.Stop();
		}
	}

}

function OnGUI() {

	if (Event.current.type == EventType.KeyDown) {
        KeyPressedEventHandler();
    }

	var cur = DateTime.Now;
	
	var fadeInTS = new TimeSpan(0, 0, 0, 3, 0);
	var fadeInNewspaperStart = this.last + fadeInTS;

	var newspaperTS = new TimeSpan(0, 0, 0, 2, 0);
	var newspaperStart = fadeInNewspaperStart + newspaperTS;

	var fadeOutTS = new TimeSpan(0, 0, 0, 5, 0);
	var fadeOutNewspaperStart = newspaperStart + fadeOutTS;

	var loadTS = new TimeSpan(0, 0, 0, 2, 0);
	var loadStart = fadeOutNewspaperStart + loadTS;
	
	if (cur > fadeInNewspaperStart && cur < newspaperStart) {
		FadeIn();
		PlaceImage(Newspaper);
	}

	if (cur > newspaperStart && cur < fadeOutNewspaperStart) {
		PlaceImage(Newspaper);
	}

	if (cur > fadeOutNewspaperStart && cur < loadStart) {
		FadeOut();
		PlaceImage(Newspaper);
	}

		if (cur > loadStart) {
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