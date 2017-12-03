using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testor : MonoBehaviour {
	// Update is called once per frame

	public float speed;
	void Update ()
	{
		transform.Translate(new Vector3(Input.GetAxis("Horizontal") * speed * Time.deltaTime,
			Input.GetAxis("Vertical") * speed * Time.deltaTime));
	}
}
