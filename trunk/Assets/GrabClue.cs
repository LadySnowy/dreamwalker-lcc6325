using UnityEngine;
using System.Collections;

public class GrabClue : MonoBehaviour
{
	
	public GameObject player;
	bool grabbed;
	bool canBeGrabbed;
	// Use this for initialization
	void Start ()
	{
		this.grabbed = false;
		this.canBeGrabbed = true;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (this.grabbed) {
			StartCoroutine (disappear ());
			this.grabbed = false;
		}
		
	}
	
	void OnTriggerEnter (Collider other)
	{
		Debug.Log ("Grab Clue Trigger enter!");
		if (this.canBeGrabbed) {
			this.grabbed = true;
			this.canBeGrabbed = false;
		}
	}
	
	IEnumerator disappear ()
	{
		this.gameObject.renderer.enabled = false;
		this.gameObject.renderer.particleSystem.Stop ();
		
		myCoRoutine ();
		yield return new WaitForSeconds(1.0f);

		myCoRoutine ();
		yield return new WaitForSeconds(1.0f);

		Destroy (this.gameObject);
	}

	void myCoRoutine ()
	{
		Color color = this.gameObject.renderer.material.color;
		color.a -= 0.01f;
		this.gameObject.renderer.material.color = color;
		Debug.Log ("Decreased alpha");
	}
}
