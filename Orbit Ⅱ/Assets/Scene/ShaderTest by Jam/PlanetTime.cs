using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetTime : MonoBehaviour {

    Material planetMat;
	// Use this for initialization
    public float lightPosX = 0.2f;
    public float lightPosY = 0.2f;

	void Start () 
    {
        planetMat = gameObject.GetComponent<SpriteRenderer>().material;
	}
	
	void Update () 
    {
        float oldX = lightPosX;
        float oldY = lightPosY;
        lightPosX = oldX*Mathf.Cos(0.01f) + oldY*Mathf.Sin(0.01f);
        lightPosY = -oldX*Mathf.Sin(0.01f) + oldY*Mathf.Cos(0.01f);

        planetMat.SetFloat("_LightPosX", lightPosX + 0.5f);
        planetMat.SetFloat("_LightPosY", lightPosY + 0.5f);

	}

}
