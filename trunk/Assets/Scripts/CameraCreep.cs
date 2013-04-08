using UnityEngine;
using System.Collections;

public class CameraCreep : MonoBehaviour {

	// Use this for initialization
	void Start () {
		CurrentMode = ZoomMode.Paused;
		Timer = 0;
		Camera.main.fieldOfView = NormalZoom;
	}
	
	const float ZoomSpeed = 0.005f;
	const float ScareSpeed = 4;
	const float ScareZoom = 30;
	const float NormalZoom = 60;
	const float SettleSpeed = 3;
	const float SettleZoom = 70;
	const float SettleAccel = 0.8f;
	float Speed = 0;
	float Timer = 0;
	float PauseDelay = 60*5;
	enum ZoomMode{
		ZoomingIn, ZoomingOut, Paused, Settle
	}
	ZoomMode CurrentMode;
	
	// Update is called once per frame
	void Update () {
		if(CurrentMode == ZoomMode.Paused){
			if(++Timer >= PauseDelay){
				CurrentMode = ZoomMode.ZoomingIn;
				Timer = 0;
			}
		}
		else if(CurrentMode == ZoomMode.ZoomingIn){
			Camera.main.fieldOfView -= ZoomSpeed;
			if(Camera.main.fieldOfView <= ScareZoom){
				Camera.main.fieldOfView = ScareZoom;	
				CurrentMode = ZoomMode.ZoomingOut;
			}
		}
		else if(CurrentMode == ZoomMode.ZoomingOut){
			Camera.main.fieldOfView += ScareSpeed;
			if(Camera.main.fieldOfView >= SettleZoom){
				Camera.main.fieldOfView = SettleZoom;
				CurrentMode = ZoomMode.Settle;
				Speed = ScareSpeed;
			}
		}
		else if(CurrentMode == ZoomMode.Settle){
			Camera.main.fieldOfView	+= Speed;
			if(Speed >= -SettleSpeed){
				Speed -= SettleAccel;
			}
			if(Camera.main.fieldOfView <= NormalZoom){
				Camera.main.fieldOfView = NormalZoom;
				CurrentMode = ZoomMode.Paused;
			}
		}

	}
}
