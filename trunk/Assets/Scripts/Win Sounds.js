#pragma strict

var Laughter:AudioSource;

var LaughterPlayed = false;

function Start ()
{
}

function Update ()
{
	if (Time.time > 2)
	{
		if (!LaughterPlayed)
		{
			Laughter.Play();
			LaughterPlayed = true;
		}
	}
}