using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
	public int mChunkID;
	public Vector2Int mChunkPos=Vector2Int.zero;
	public void Initialize(int id)
	{
		mChunkID = id;
	}
}
