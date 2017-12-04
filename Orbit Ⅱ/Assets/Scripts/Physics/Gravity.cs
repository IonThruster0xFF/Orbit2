using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Gravity : MonoBehaviour {
	private void Awake()
	{
		Rigidbody2D rig2d = GetComponent<Rigidbody2D>();
		rig2d.gravityScale = 0;
		PhysicsManager Physics = GameObject.Find("PhysicsManager").GetComponent<PhysicsManager>();
		if (Physics != null)
		{
			Physics.mCelestialRequireGravityRigidbody2D.Add(rig2d);
		}
	}
}
