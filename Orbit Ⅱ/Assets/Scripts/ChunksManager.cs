using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunksManager : MonoBehaviour
{
	/// <summary>
	/// 区块模板
	/// </summary>
	public ChunkCreator mChunkCreator;
	/// <summary>
	/// 视野大小（加载几倍视野范围内的区块）
	/// </summary>
	public int mVisualSize;
	/// <summary>
	/// 区块组件活动地图
	/// </summary>
	public Dictionary<Vector2Int, bool> mChunkMap=new Dictionary<Vector2Int, bool>();
	/// <summary>
	/// 区块组件尺寸
	/// </summary>
	public Vector2 mChunkSize;
	/// <summary>
	/// 测试用对象
	/// </summary>
	public GameObject test;
	/// <summary>
	/// 区块ID值
	/// </summary>
	public int ChunkID = 0;
	/// <summary>
	/// 获取一个游戏对象当前所在的区块坐标
	/// </summary>
	/// <param name="obj">游戏对象</param>
	/// <returns></returns>
	public Vector2Int GetCurrentChunkPos(GameObject obj)
	{
		Vector2Int pos=Vector2Int.zero;
		Vector2 _pos = new Vector2(obj.transform.position.x/mChunkSize.x,obj.transform.position.y/mChunkSize.y);
		if (_pos.x >= ((int) _pos.x + 0.5f) && _pos.x < (int) (_pos.x + 1))
		{
			pos.x = (int) _pos.x + 1;
		}
		else
		{
			pos.x = (int)_pos.x;
		}
		if (_pos.y >= ((int)_pos.y + 0.5f) && _pos.y < (int)(_pos.y + 1))
		{
			pos.y = (int)_pos.y + 1;
		}
		else
		{
			pos.y = (int)_pos.y;
		}
		return pos;
	}
	/// <summary>
	/// Chunk对象字典，存储对应chunk的chunk对象
	/// </summary>
	public Dictionary<Vector2Int,GameObject> mChunks = new Dictionary<Vector2Int, GameObject>();
	/// <summary>
	/// 区块ID对应的区块
	/// </summary>
	public Dictionary<int, GameObject> mChunksByID = new Dictionary<int, GameObject>();
	/// <summary>
	/// 获取一个chunk坐标的中心位置坐标
	/// </summary>
	/// <param name="pos"></param>
	/// <returns></returns>
	public Vector2 GetChunkCenterPos(Vector2Int pos)
	{
		Vector2 _pos = new Vector2(pos.x * mChunkSize.x, pos.y * mChunkSize.y);
		return _pos;
	}
	/// <summary>
	/// 获取某游戏对象周围需要加载的chunk坐标数组
	/// </summary>
	/// <param name="obj"></param>
	/// <returns></returns>
	public Vector2Int[] GetChunksPosNeedLoad(GameObject obj)
	{
		
		int k = 0;
		Vector2Int[] needLoadChunkPos = new Vector2Int[9];
		for (int i = -1; i <2; i++)
		{
			for (int j = -1; j < 2; j++)
			{
				needLoadChunkPos[k] = GetCurrentChunkPos(obj) + new Vector2Int(i, j);
				k++;
			}
		}
		return needLoadChunkPos;
	}
	/// <summary>
	/// 创建一个新的chunk
	/// </summary>
	/// <param name="chunkPos"></param>
	/// <returns></returns>
	public GameObject ChunkPoolNew(Vector2Int chunkPos)
	{
		if (!mChunkMap[chunkPos])
		{
			GameObject newChunk= mChunkCreator.CreateNewChunk(GetChunkCenterPos(chunkPos));
			mChunkMap[chunkPos] = true;
			return newChunk;
		}
		return null;
	}
	/// <summary>
	/// 刷新chunk，判断是否生成新chunk
	/// </summary>
	/// <param name="PosArray">激活的chunk（其余chunk不激活）</param>
	public void RefreshChunk(Vector2Int[] PosArray)
	{
		foreach (Vector2Int pos in PosArray)
		{
			if (!mChunkMap.ContainsKey(pos))
			{
				mChunkMap.Add(pos, false);
			}
			GameObject newChunk = ChunkPoolNew(pos);
			if (newChunk != null)
			{
				mChunks.Add(pos,newChunk);
				mChunksByID.Add(ChunkID, newChunk);
				Chunk chunkComponent=newChunk.AddComponent<Chunk>();
				chunkComponent.mChunkID = ChunkID;
				chunkComponent.mChunkPos = pos;
				ChunkID++;

			}
		}
	}
	/// <summary>
	/// 判断一个二维向量是否在一个二维向量数组内
	/// </summary>
	/// <param name="posArray"></param>
	/// <param name="pos"></param>
	/// <returns></returns>
	public bool IfLoading(Vector2Int[] posArray,Vector2Int pos)
	{
		foreach (Vector2Int _pos in posArray)
		{
			if (_pos == pos)
			{
				return true;
			}
		}
		return false;
	}
	/// <summary>
	/// 设置激活值
	/// </summary>
	/// <param name="obj"></param>
	public void SetChunkActive(GameObject obj)
	{
		foreach (Vector2Int pos in mChunks.Keys)
		{
			if (IfLoading(GetChunksPosNeedLoad(obj), pos))
			{
				mChunks[pos].SetActive(true);
			}
			else
			{
				mChunks[pos].SetActive(false);
			}
		}
	}
	private void Update()
	{
		RefreshChunk(GetChunksPosNeedLoad(test));//刷新test周围区块，判断并创造9个相邻区块中未加载的区块
		SetChunkActive(test);//激活9个相邻区块，并且设置其余区块暂停活动
	}
}
