using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 单独的一个Block,不存在交互...
/// </summary>
public class Space : BlockBase 
{
    //public 
    public override void Init(Vector2Int pos, string sceneName)
    {
        base.Init(pos, sceneName);
        preview = Resources.Load<Sprite>("space");
        if(preview == null)
            Debug.Log("NULL");
        blockInfo = new BlockInfo("Space");
    }
    // 不通过ID加载数据!!
    public void Init(SpaceSaveData data,ID id)
    {
        this.id = id;
        this.id.Init();
        this.position = new Vector2Int(id.idx, id.idy);
        preview = Resources.Load<Sprite>("space");
        if(preview == null)
            Debug.Log("NULL");
        blockInfo = new BlockInfo("Space");
    }

    public override void fromSaveData(SaveData data)
    {}
    public override SaveData toSaveData()
    {
        return new SpaceSaveData();
    }
    public override bool Update()
    {
        return false;
    }
    public override void EnterUpdate()
    {}
    public override void ExitUpdate()
    {}
}
[System.Serializable]
public class SpaceSaveData : CreatableSaveData
{
    public override T Create<T>(ID id)
    {
        Space space = ScriptableObject.CreateInstance<Space>();
        space.Init(this,id);
        return (T)(System.Object)space;
    }
}
