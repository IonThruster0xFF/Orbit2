using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Vector2Data
{
    float x;
    float y;
    public Vector2 GetData()
    {
        return new Vector2(x, y);
    }
    public void SaveData(Vector2 vec)
    {
        x = vec.x;
        y = vec.y;
    }
}
[System.Serializable]
public class Vector3Data
{
    float x;
    float y;
    float z;
    public Vector3 GetData()
    {
        return new Vector3(x, y, z);
    }
    public void SaveData(Vector3 vec)
    {
        x = vec.x;
        y = vec.y;
        z = vec.z;
    }
}
[System.Serializable]
public class Vector2IntData
{
    int x;
    int y;
    public Vector2Int GetData()
    {
        return new Vector2Int(x, y);
    }
    public void SaveData(Vector2Int vec)
    {
        x = vec.x;
        y = vec.y;
    }
}