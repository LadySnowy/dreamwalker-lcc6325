using UnityEngine;
using System.Collections;

public class ScreenShader : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Shader s = Shader.Find("Custom/CreeperCollision");
		//Camera.main.SetReplacementShader(s, "");
		Camera.main.RenderWithShader(s, "");
	}
	

}
