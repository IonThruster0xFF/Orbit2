using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsManager : MonoBehaviour
{
	public List<Rigidbody2D> mCelestialRequireGravityRigidbody2D=new List<Rigidbody2D>();
	public float mGravityConstant = GetGravityConstant();
	/// <summary>
	/// 返回引力常数，待补
	/// </summary>
	/// <returns></returns>
	public static float GetGravityConstant()
	{
		return 6.67f;
	}
	public void Update()
	{
		foreach (var rig2d in mCelestialRequireGravityRigidbody2D)
		{
			foreach (var otherRig2d in mCelestialRequireGravityRigidbody2D)
			{
				if (rig2d != otherRig2d)
				{
					Vector2 dir = (otherRig2d.transform.position - rig2d.transform.position).normalized;
					float dis = Vector2.Distance(otherRig2d.transform.position, rig2d.transform.position);
					float gravityValue = rig2d.mass * otherRig2d.mass * mGravityConstant * (1 / (dis * dis));
					rig2d.AddForce(dir * gravityValue);
				}
			}
		}
	}
}
