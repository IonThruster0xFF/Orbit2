using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Manager;

// 存储功能示例
public class 大嘴怪物 : MonoBehaviour, ISaveable
{
    public ID id;//
    public int attack;

    void Start()
    {
        id = new ID();
        id.id = 1;
        id.className = "大嘴怪物";
        id.sceneName = "测试场景";
        id.Init(this);
        SaveManager.Instance.Load(this,id);// 获得这个怪物的数据
    }
    public void SaveMonster()
    {
        Debug.Log("Save Monster");
        SaveManager.Instance.Save(this, id);// 存储这个怪物
    }
    void OnDestroy()
    {
        id.OnDestroy();
    }

    public SaveData toSaveData()
    {
        // 返回数据
        大嘴怪的存储数据 data = new 大嘴怪的存储数据();
        data.储存的attack = attack;
        return data;
    }
    public void fromSaveData(SaveData saveData)
    {
        // 从数据加载自己
        大嘴怪的存储数据 data = (大嘴怪的存储数据)saveData;
        attack = data.储存的attack;
    }
}
[System.Serializable]
public class 大嘴怪的存储数据 : SaveData
{
    public int 储存的attack;
}