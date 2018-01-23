using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Manager;

public enum BlockType{}

public struct BlockInfo
{
    public string name;
    public BlockInfo(string name)
    {
        this.name = name;
    }
}
public abstract class BlockBase : TileBase, ISaveable
{
    public ID id;
    public BlockInfo blockInfo;
    public Vector2Int position;

    public Sprite preview;

    /// <summary>
    /// 通过Position完全新建
    /// </summary>
    public virtual void Init(Vector2Int pos, string sceneName)
    {
        position = pos;
        id = new ID();
        id.idx = pos.x;
        id.idy = pos.y;
        id.className = this.GetType().Name;
        id.sceneName = sceneName;
        id.Init();
    }
    #region Save|实现ISaveable接口
    public ID Save()
    {
        SaveManager.Instance.Save(this, id);
        return id;
    }
    public abstract void fromSaveData(SaveData data);
    public abstract SaveData toSaveData();
    #endregion

    #region 进入Update|正常Update|退出Update
    // 经过一段时间后的刷新
    public abstract void EnterUpdate();
    // 快速刷新 (返回是否更新了自身的Sprite)
    public abstract bool Update();
    // 退出刷新
    public abstract void ExitUpdate();
    #endregion

    // 当新的tile被设置时, 更新周围的哪些tile
    public override void RefreshTile(Vector3Int location, ITilemap tilemap)
    {
        tilemap.RefreshTile(location);
    }
    // 返回给tileMap的数据
    public override void GetTileData(Vector3Int location, ITilemap tilemap, ref TileData tileData)
    {
        tileData.sprite = preview;
        tileData.color = Color.white;
        //tileData.transform.SetTRS(Vector3.zero, GetRotation(), Vector3.one);
        tileData.flags = TileFlags.LockTransform;
        tileData.colliderType = Tile.ColliderType.None;
    }
}
