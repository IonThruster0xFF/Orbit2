/// <summary>
/// 一个游戏存档信息, 会在游戏存档下生成 record.dat 文件,每次载入存档界面时读取
/// 1. 先创建一个Record对象
/// 2. SetName("xxx")并检测返回值
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Manager;
using System.IO;

[Serializable]
public class Record
{
    public string name = null;//存档名
    public string folderName = null;//文件夹名(每次加载时自动生成)

    public string createTime = null;//存档创建时间
    public string updateTime = null;//最近一次更新存档时间

    //其他信息

    //存档列表, 每次调用 GetRecords后更新这个列表
    public static List<Record> records = new List<Record>();

    /// <summary>
    /// 查找存档目录,获得所有存档
    /// </summary>
    public static List<Record> GetRecords()
    {
        records.Clear();
        string savesRootPath = SaveManager.Instance.GetFolderPath(SaveManager.SAVES_FOLDER);
        if (!Directory.Exists(savesRootPath))
        {
            Debug.Log("Create Dir "+savesRootPath);
            Directory.CreateDirectory(savesRootPath);
        }
        DirectoryInfo savesRoot = new DirectoryInfo(savesRootPath);
        DirectoryInfo[] saveFolders = savesRoot.GetDirectories();
        foreach (DirectoryInfo dir in saveFolders)
        {
            //一个存档下的文件
            string recordPath = dir.FullName +"/"+ SaveManager.RECORD_FILE + SaveManager.FILE_EXTENSION;
            if (File.Exists(recordPath))
            {    
                Record record = SaveManager.Instance.LoadAtPath<Record>(SaveManager.SAVES_FOLDER + "/" + dir.Name + "/"+ SaveManager.RECORD_FILE);
                record.folderName = dir.Name;
                records.Add(record);
            }
        }
        return records;
    }
    public static Record GetRecord(string recordName)
    {
        if (records.Count == 0)
            GetRecords();
        foreach (Record rec in records)
        {
            if (rec.name == recordName)
                return rec;
        }
        return null;
    }

    /// <summary>
    /// 禁止生成同文件夹名存档!!!!
    /// </summary>
    public Record()
    {
        createTime = DateTime.Now.ToString();
        updateTime = createTime;
    }
    /// <summary>
    /// 在存档总文件夹savePath(saves)的位置,生成/保存一个存档
    /// 在更改存档信息(如重命名)后调用, 或者在新建存档时调用
    /// </summary>
    /// <param name="savePath">Save path.</param>
    public void SaveInfo()
    {
        Debug.Log("Save Record Data");
        Debug.Assert(folderName != null);
        updateTime = System.DateTime.Now.ToString();
        SaveManager.Instance.SaveAtPath(this,SaveManager.SAVES_FOLDER +"/"+ folderName +"/"+ SaveManager.RECORD_FILE);
        GetRecords();//更新存档列表
    }

    /// <summary>
    /// 只有在存档界面才能设置存档名
    /// 设置存档名, 成功返回新名称, 失败返回null
    /// 文件夹名不变
    /// </summary>
    public string SetName(string name)
    {
        this.name = CheckName(name);
        // 如果文件夹名称还没确定, 就设置他为这个名字
        if (folderName == null)
            folderName = this.name;
        if (this.name == null)
        {
            Debug.Log("Record Name ( "+ name +" ) Is Repeated.");
        }
        return this.name;
    }
    // 在设置存档名时检测名字是否重复
    private string CheckName(string newName)
    {
        if (records.Count == 0)
            GetRecords();
        foreach (Record rec in records)
        {
            if (newName == rec.name || newName == rec.folderName)
                return null;
        }
        return newName;
    }
}
