using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
/// <summary>
/// 区块生成代码
/// </summary>
public class ChunkCreator:MonoBehaviour
{
	/// <summary>
	/// 区块模板
	/// </summary>
	public GameObject ChunkPrefab;
	/// <summary>
	/// 区块生成函数
	/// </summary>
	/// <param name="pos">区块的坐标</param>
	/// <returns></returns>
		public GameObject CreateNewChunk(Vector2 pos)
		{
			GameObject gmobj = GameObject.Instantiate(ChunkPrefab, pos, Quaternion.identity,transform);
		//此处放区块生成代码，待补
			return gmobj;
		}
	}
